using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_boardcastroom")]
    public class BoardCastRoomEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [DataType("INTEGER")]
        public int State { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Name { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Domain { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string AppName { get; set; } = "live";
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string StreamName { get; set; }
         public string PublishNotifyUrl { get; set; }
        [MaxLength(500), DataType("VARCHAR"), Required]
        public string PlayNotifyUrl { get; set; }
        [MaxLength(500), DataType("VARCHAR"), Required]
        public string PublishEndNotifyUrl { get; set; }
        [MaxLength(500), DataType("VARCHAR"), Required]
        public string PlayEndNotifyUrl { get; set; }
        [DataType("DATETIME"), Required]
        public DateTime ExpireTime { get; set; }

    }
}