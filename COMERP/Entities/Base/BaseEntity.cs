using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMERP.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime? CreationDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
        public void SetUpdateDate(DateTime updateDate, string updatedBy)
        {
            UpdateDate = updateDate;
            UpdatedBy = updatedBy;
        }
        public void SetCreatedDate(DateTime creationDate, string createdBy)
        {
            CreationDate = creationDate;
            CreatedBy = createdBy;
        }
    }
}
