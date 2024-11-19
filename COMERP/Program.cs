using COMERP.Abstractions.Interfaces;
using COMERP.DataContext;
using COMERP.Entities;
using COMERP.Implementation.Services;
using COMERP.Middlewares;
using COMERP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using COMERP.SeedData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// For authentication
var _key = builder.Configuration["Jwt:Key"];
var _issuer = builder.Configuration["Jwt:ValidIssuer"];
var _audience = builder.Configuration["Jwt:ValidAudiance"];
var _expirtyMinutes = builder.Configuration["Jwt:ExpiryMinutes"];




// Configuration for token
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = _audience,
        ValidIssuer = _issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
        ClockSkew = TimeSpan.FromMinutes(Convert.ToDouble(_expirtyMinutes))
    };
});

// Dependency injection with key
builder.Services.AddSingleton<ITokenGenerator>(new TokenGenerator(_key, _issuer, _audience, _expirtyMinutes));

// Include Infrastructur Dependency
builder.Services.AddInfrastructure(builder.Configuration);
// Include Application Dependency
builder.Services.AddApplication();
// Include Infrastructur Dependency
builder.Services.AddInfrastructure(builder.Configuration);
// Configuration Add Controllers



// Configuration CORS Policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader() // Allow any header
            .AllowCredentials();
    });
});

// Configuration Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Basic Swagger document settings
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Android App API",
        Version = "v1",
        Description = "API for managing the backend services of the Android App. Provides endpoints for user authentication, data management, and more.",

        // Contact information
        Contact = new OpenApiContact
        {
            Name = "API Support Team",
            Email = "support@androidapp.com",
            Url = new Uri("https://www.androidapp.com/support")
        },

        // License information
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        },

        // Terms of service
        TermsOfService = new Uri("https://www.androidapp.com/terms"),

        // Optional custom extensions
        Extensions = { }
    });


    // JWT Bearer authentication configuration
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\nEnter 'Bearer' [space] and then your token in the text input below.\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Add global security requirement for JWT Bearer authentication
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();
//Configuration Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services).Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
