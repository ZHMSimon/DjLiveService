using System.Data.Entity.ModelConfiguration;
using DjLive.CPDao.Entity;

namespace DjLive.CPDao.Map
{
    public class AccountMap : EntityTypeConfiguration<AccountEntity>
    {
        public AccountMap()
        {
            HasMany(t => t.ServerEntities).WithMany(t => t.AccountEntities).Map(m =>
            {
                m.ToTable("sys_alli_account_servernode");
                m.MapLeftKey("AccountId");
                m.MapRightKey("NodeId");
            });
            HasMany(t => t.SecurePolicyEntities).WithMany(t => t.AccountEntities).Map(m =>
            {
                m.ToTable("sys_alli_account_securepolicy");
                m.MapLeftKey("AccountId");
                m.MapRightKey("PolicyId");
            });

            HasMany(t => t.RecordTemplateEntities).WithMany(t => t.AccountEntities).Map(m =>
            {
                m.ToTable("sys_alli_account_recordtemplate");
                m.MapLeftKey("AccountId");
                m.MapRightKey("TemplateId");
            });

            HasMany(t => t.TranscodeTemplateEntities).WithMany(t => t.AccountEntities).Map(m =>
            {
                m.ToTable("sys_alli_account_transcodetemplate");
                m.MapLeftKey("AccountId");
                m.MapRightKey("TemplateId");
            });

            HasMany(t => t.LogoTemplateEntities).WithOptional(t => t.AccountEntity).Map(m =>
            {
                m.ToTable("sys_logotemplate");
                m.MapKey("AccountId");
            });
            //HasMany(t => t.AppEntities).WithOptional(t => t.AccountEntity).Map(m =>
            //{
            //    m.ToTable("sys_logotemplate");
            //    m.MapKey("AccountId");
            //});
        }

    }
}