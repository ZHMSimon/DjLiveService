using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_transcodetemplate")]
    public class TranscodeTemplateEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"),Required]
        public string Name { get; set; }
        [MaxLength(50), DataType("VARCHAR")]
        public string AppName { get; set; }
        [MaxLength(2000), DataType("VARCHAR"), Required]
        public string VideoOption { get; set; }
        [MaxLength(2000), DataType("VARCHAR"), Required]
        public string AudioOption { get; set; }
        public virtual List<DomainEntity> Domains { get; set; } = new List<DomainEntity>();
        public virtual List<AccountEntity> AccountEntities { get; set; } = new List<AccountEntity>();
        public virtual LogoTemplateEntity LogoTemplate { get; set; } = null;
    }
}