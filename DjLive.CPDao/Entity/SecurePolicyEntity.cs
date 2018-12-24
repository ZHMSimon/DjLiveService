using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_securepolicy")]
    public class SecurePolicyEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"),Required]
        public string Name { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthPublishUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthPlayUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthConnectUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthCloseUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthUnPublishUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthDvrUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string AuthStopUrl { get; set; }
        [MaxLength(200), DataType("VARCHAR")]
        public string NotifyHlsUrl { get; set; }

        [MaxLength(2000), DataType("VARCHAR")]
        public string PublishReferAllow { get; set; }
        [MaxLength(2000), DataType("VARCHAR")]
        public string PlayReferAllow { get; set; }
        [MaxLength(2000), DataType("VARCHAR")]
        public string PublishIpAllow { get; set; }
        [MaxLength(2000), DataType("VARCHAR")]
        public string PlayIpAllow { get; set; }
        public virtual List<DomainEntity> Domains { get; set; } = new List<DomainEntity>();
        public virtual List<AccountEntity>  AccountEntities { get; set; } = new List<AccountEntity>();
    }
}