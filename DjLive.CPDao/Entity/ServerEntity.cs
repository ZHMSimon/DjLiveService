using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_servernode")]
    public class ServerEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"),Required]
        public string Name { get; set; }
        [MaxLength(500), DataType("VARCHAR"), Required]
        public string Url { get; set; }
        [MaxLength(20), DataType("VARCHAR"), Required]
        public string Ip { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string UserName { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Password { get; set; }
        [MaxLength(2000), DataType("VARCHAR"), Required]
        public string Option { get; set; }
        public virtual List<DomainEntity> Domains { get; set; } = new List<DomainEntity>();
        public virtual List<AccountEntity> AccountEntities { get; set; } = new List<AccountEntity>();
    }
}