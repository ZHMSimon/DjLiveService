using System.Data.Entity.ModelConfiguration;
using DjLive.CPDao.Entity;

namespace DjLive.CPDao.Map
{
    public class TranscodeMap : EntityTypeConfiguration<TranscodeTemplateEntity>
    {

        public TranscodeMap()
        {
            HasOptional(t => t.LogoTemplate).WithMany(t => t.TranscodeTemplates).Map(m =>
            {
                m.ToTable("sys_transcodetemplate");
                m.MapKey("LogoTemplateId");
            });
        }
    }
}