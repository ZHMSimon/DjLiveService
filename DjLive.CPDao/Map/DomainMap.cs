using System.Data.Entity.ModelConfiguration;
using DjLive.CPDao.Entity;

namespace DjLive.CPDao.Map
{
    public class DomainMap : EntityTypeConfiguration<DomainEntity>
    {
       
        public DomainMap()
        {
            //HasMany(t => t.AppList).WithMany(t => t.TranscodeTemplates).Map(m =>
            //{
            //    m.ToTable("sys_alli_domain_app");
            //    m.MapLeftKey("DomainId");
            //    m.MapRightKey("AppId");
            //});
            HasMany(t => t.TranscodeList).WithMany(t => t.Domains).Map(m =>
            {
                m.ToTable("sys_alli_domain_transcode");
                m.MapLeftKey("DomainId");
                m.MapRightKey("TranscodeId");
            });
            HasRequired(t => t.Account).WithMany(t => t.Domains).Map(m =>
            {
                m.ToTable("sys_domain");
                m.MapKey("AccountId");
            });
            HasRequired(t=>t.Server).WithMany(t=>t.Domains).Map(m =>
            {
                m.ToTable("sys_domain");
                m.MapKey("ServerId");
            });

            HasOptional(t => t.SecurePolicy).WithMany(t => t.Domains).Map(m =>
            {
                m.ToTable("sys_domain");
                m.MapKey("SecureId");
            });
            HasOptional(t => t.RecordTemplate).WithMany(t => t.Domains).Map(m =>
            {
                m.ToTable("sys_domain");
                m.MapKey("RecordTemplateId");
            });
        }
    }
}