using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjLive.CPDao.Entity
{
    [Table("sys_account")]
    public class AccountEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"),Required]
        public string UserName { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Password { get; set; }
        [DataType("Int"), Required]
        public int RoleType { get; set; }
        [DataType("Int"), Required]
        public int StatType { get; set; }
        [MaxLength(50), DataType("VARCHAR")]
        public string Token { get; set; }

        public virtual List<DomainEntity> Domains { get; set; } = new List<DomainEntity>();
        public virtual List<SecurePolicyEntity> SecurePolicyEntities { get; set; } = new List<SecurePolicyEntity>();
        public virtual List<RecordTemplateEntity> RecordTemplateEntities { get; set; } = new List<RecordTemplateEntity>();
        public virtual List<TranscodeTemplateEntity> TranscodeTemplateEntities { get; set; } = new List<TranscodeTemplateEntity>();
        public virtual List<LogoTemplateEntity> LogoTemplateEntities { get; set; } = new List<LogoTemplateEntity>();
        public virtual List<ServerEntity> ServerEntities { get; set; } = new List<ServerEntity>();
        //public virtual List<AppEntity> AppEntities { get; set; }
    }
}
