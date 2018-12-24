using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DjLive.CPDao.Entity
{
    [Table("sys_voditem")]
    public class VodItemEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Name { get; set; }
        [DataType("DATETIME")]
        public DateTime RecordDateTime { get; set; }
        [DataType("INTEGER")]
        public int TimeLength { get; set; }
        [DataType("INTEGER")]
        public int DownLoadTimes { get; set; }
        [DataType("INTEGER")]
        public int State { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string Ip { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string HostName { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string AppName { get; set; }
        [MaxLength(50), DataType("VARCHAR"), Required]
        public string StreamName { get; set; }
    }
}