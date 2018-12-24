using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_recordtemplate")]
    public class RecordTemplateEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Name { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string Description { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string NamePolicy { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string UploadUrl { get; set; }
        public virtual List<DomainEntity> Domains { get; set; } = new List<DomainEntity>();
        public virtual List<AccountEntity> AccountEntities { get; set; } = new List<AccountEntity>();

    }
}