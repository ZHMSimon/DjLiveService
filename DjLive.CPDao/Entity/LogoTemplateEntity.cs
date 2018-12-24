using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_logotemplate")]
    public class LogoTemplateEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"),Required]
        public string Name { get; set; }
        [DataType("Text"), Required]
        public string Base64Vale { get; set; }
        [MaxLength(500), DataType("VARCHAR")]
        public string FilePath { get; set; }
        [MaxLength(2000), DataType("VARCHAR")]
        public string Option { get; set; }
        public virtual List<TranscodeTemplateEntity> TranscodeTemplates { get; set; }
        public virtual AccountEntity AccountEntity { get; set; }

    }
}