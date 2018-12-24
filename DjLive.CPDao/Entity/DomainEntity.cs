using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjLive.CPDao.Entity
{
    [Table("sys_domain")]
    public class DomainEntity
    {
        [Key, MaxLength(50), DataType("VARCHAR")]
        public string Id { get; set; }

        [MaxLength(500), DataType("VARCHAR"), Required]
        public string SourceDomain { get; set; }
        [MaxLength(500), DataType("VARCHAR")]
        public string Description { get; set; }
        [DataType("INT")]
        public int SourceType { get; set; }
        [DataType("INT")]
        public int StateType { get; set; }

        #region 播放Domain
        [MaxLength(500), DataType("VARCHAR")]
        public string RtmpPlayDomain { get; set; }
        [MaxLength(500), DataType("VARCHAR")]
        public string FlvPlayDomain { get; set; }
        [MaxLength(500), DataType("VARCHAR")]
        public string HlsPlayDomain { get; set; }
        #endregion

        public virtual ServerEntity Server { get; set; } = null;
        public virtual AccountEntity Account { get; set; }
        public virtual List<TranscodeTemplateEntity> TranscodeList { get; set; } = new List<TranscodeTemplateEntity>();
        public virtual SecurePolicyEntity SecurePolicy { get; set; } = null;
        public virtual RecordTemplateEntity RecordTemplate { get; set; } = null;
        //public virtual List<AppEntity> AppList { get; set; }
    }
}
