using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using COMERP.Abstractions.Repository.Base;
using COMERP.DataContext;
using COMERP.Entities.Base;
using COMERP.Exceptions;

namespace COMERP.Implementation.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DbSet<T> _dbSet;
        private readonly PropertyInfo[] _properties;

        public Repository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor contextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _contextAccessor = contextAccessor;
            _dbSet = _applicationDbContext.Set<T>();
            _properties = typeof(T).GetProperties();
        }
        public async Task<(bool Success, string Message)> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return (true, "Entity added successfully");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(T entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
            return (true, "Entity Update successfully");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return (false, "Entity not found.");
            }
            _dbSet.Remove(entity);
            return (true, "Entity Delete successfully");

        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetByIdAsync(string id)
        {
            var entityId = Convert.ChangeType(id, GetKeyProperty().PropertyType);
            return await _dbSet.FindAsync(entityId);
        }
        private async Task<(bool Success, string id, string Message)> SaveEntityAsync(T entity, bool isUpdate)
        {
            return await ExecuteDbOperationAsync(async () =>
            {
                SetCommonFields(entity, isUpdate);
                if (isUpdate)
                {
                    UpdateNonNullProperties(entity);
                }
                else
                {
                    await _dbSet.AddAsync(entity);
                }
                await _applicationDbContext.SaveChangesAsync();
            }, GetEntityId(entity).ToString(), isUpdate ? "Entity updated successfully." : "Entity added successfully.");
        }
        private async Task<(bool Success, string id, string Message)> ExecuteDbOperationAsync(Func<Task> dbOperation, string id, string successMessage)
        {
            try
            {
                await dbOperation();
                return (true, id, successMessage);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return (false, null, ex.Message);
            }
        }
        private string GetUserName() =>
            _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        private void SetCommonFields(T entity, bool isUpdate)
        {
            var userName = GetUserName();
            var now = DateTime.UtcNow;

            if (entity is BaseEntity baseEntity)
            {
                if (!isUpdate)
                {
                    var id = Guid.NewGuid().ToString();
                    SetPropertyIfExists(entity, "Id", id);
                    baseEntity.SetCreatedDate(now, userName);
                }
                else
                {
                    baseEntity.SetUpdateDate(now, userName);
                }
            }
        }
        private void SetPropertyIfExists(T entity, string propertyName, object value)
        {
            var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                property.SetValue(entity, value);
            }
        }
        private object GetEntityId(T entity)
        {
            var keyProperty = GetKeyProperty();
            if (keyProperty == null)
                throw new InvalidOperationException("Entity must have a [Key] attribute on one property to retrieve its ID.");

            return keyProperty.GetValue(entity);
        }
        private PropertyInfo GetKeyProperty()
        {
            return _properties.FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
        }
        private void UpdateNonNullProperties(T entity)
        {
            foreach (var property in _properties)
            {
                if (Attribute.IsDefined(property, typeof(KeyAttribute))) continue; // Skip key property

                var value = property.GetValue(entity);
                if (value == null || value is string str && string.IsNullOrEmpty(str)) continue; // Skip null or empty

                // Update the property value in the database
                _applicationDbContext.Entry(entity).Property(property.Name).IsModified = true;
            }
        }
        public async Task<(bool Success, string id, string Message)> AddSqlAsync(T entity)
        {
            return await ExecuteSqlOperationAsync(entity, "INSERT");
        }
        public async Task<(bool Success, string id, string Message)> UpdateSqlAsync(T entity)
        {
            return await ExecuteSqlOperationAsync(entity, "UPDATE");
        }
        public async Task<(bool Success, string id, string Message)> DeleteSqlAsync(string id)
        {
            try
            {
                using var connection = _dapperDbContext.CreateConnection();
                var keyProperty = GetKeyProperty();
                if (keyProperty == null)
                    throw new InvalidOperationException("Entity must have a [Key] attribute on one property to perform deletes.");

                var query = $"DELETE FROM {typeof(T).Name}s WHERE {keyProperty.Name} = @Id";
                await connection.ExecuteAsync(query, new { Id = id });

                return (true, id, "Entity deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<IEnumerable<T>> GetAllSqlAsync()
        {
            using var connection = _dapperDbContext.CreateConnection();

            // Build the base query
            var query = $"SELECT * FROM {typeof(T).Name}s";

            // Get navigation properties
            var navigationProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string))
                .ToList();

            if (navigationProperties.Any())
            {
                // Build the JOIN part dynamically
                foreach (var navigationProperty in navigationProperties)
                {
                    var foreignTableName = navigationProperty.PropertyType.Name + "s"; // Assuming plural table names
                    var foreignKeyName = $"{typeof(T).Name}Id"; // Example foreign key
                    query += $" LEFT JOIN {foreignTableName} ON {typeof(T).Name}.{foreignKeyName} = {foreignTableName}.{foreignKeyName}";
                }
            }

            return await connection.QueryAsync<T>(query);
        }

        public async Task<T> GetByIdSqlAsync(string id)
        {
            using var connection = _dapperDbContext.CreateConnection();
            var keyProperty = GetKeyProperty();
            if (keyProperty == null)
                throw new InvalidOperationException("Entity must have a [Key] attribute on one property to retrieve by ID.");

            // Build the base query
            var query = $"SELECT * FROM {typeof(T).Name}s WHERE {keyProperty.Name} = @Id";

            // Get navigation properties for join
            var navigationProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string))
                .ToList();

            if (navigationProperties.Any())
            {
                foreach (var navigationProperty in navigationProperties)
                {
                    var foreignTableName = navigationProperty.PropertyType.Name + "s"; // Assuming plural table names
                    var foreignKeyName = $"{typeof(T).Name}Id"; // Example foreign key
                    query += $" LEFT JOIN {foreignTableName} ON {typeof(T).Name}.{foreignKeyName} = {foreignTableName}.{foreignKeyName}";
                }
            }

            var result = await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
            if (result != null) return result;

            throw new NotFoundException($"Entity of type {typeof(T).Name} with Id '{id}' was not found.");
        }

        private async Task<(bool Success, string id, string Message)> ExecuteSqlOperationAsync(T entity, string operationType)
        {
            try
            {
                using var connection = _dapperDbContext.CreateConnection();
                SetCommonFields(entity, operationType == "UPDATE");

                string query;
                if (operationType == "INSERT")
                {
                    var (columnNames, parameterNames) = GetInsertParameters(entity);
                    query = $"INSERT INTO {typeof(T).Name}s ({columnNames}) VALUES ({parameterNames})";
                }
                else
                {
                    var (updateFields, keyProperty) = GetUpdateParameters(entity);
                    query = $"UPDATE {typeof(T).Name}s SET {updateFields} WHERE {keyProperty.Name} = @{keyProperty.Name}";
                }

                var parameters = GetDynamicParameters(entity);
                await connection.ExecuteAsync(query, parameters);

                return (true, GetEntityId(entity).ToString(), $"{operationType} operation successful.");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        private (string columnNames, string parameterNames) GetInsertParameters(T entity)
        {
            var properties = _properties.Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute)) || p.Name == "Id").ToList();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parameterNames = string.Join(", ", properties.Select(p => "@" + p.Name));
            return (columnNames, parameterNames);
        }
        private (string updateFields, PropertyInfo keyProperty) GetUpdateParameters(T entity)
        {
            var properties = _properties.Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute))).ToList();
            var updateFields = string.Join(", ", properties.Where(p => IsValidForUpdate(entity, p)).Select(p => $"{p.Name} = @{p.Name}"));

            var keyProperty = GetKeyProperty();
            if (keyProperty == null)
                throw new InvalidOperationException("Entity must have a [Key] attribute on one property to perform updates.");

            return (updateFields, keyProperty);
        }
        private bool IsValidForUpdate(T entity, PropertyInfo property)
        {
            var value = property.GetValue(entity);
            if (value is string str)
                return !string.IsNullOrEmpty(str);
            return value != null;
        }
        private DynamicParameters GetDynamicParameters(T entity)
        {
            var parameters = new DynamicParameters();
            foreach (var property in _properties)
            {
                parameters.Add(property.Name, property.GetValue(entity));
            }
            return parameters;
        }
    }
}
