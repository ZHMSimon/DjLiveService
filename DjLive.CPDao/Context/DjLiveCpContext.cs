using System.Data.Entity;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Map;

namespace DjLive.CPDao.Context
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DjLiveCpContext : DbContext
    {
        public DbSet<AccountEntity> Account { set; get; }
        public DbSet<ServerEntity> Server { set; get; }
        public DbSet<DomainEntity> Domain { set; get; }
        //public DbSet<AppEntity> App { set; get; }
        //public DbSet<StreamEntity> Stream { set; get; }
        public DbSet<LogoTemplateEntity> LogoTemplate { set; get; }
        public DbSet<SecurePolicyEntity> SecurePolicy { set; get; }
        public DbSet<TranscodeTemplateEntity> TranscodeTemplate { set; get; }
        public DbSet<RecordTemplateEntity> RecordTemplate { set; get; }
        public DbSet<VodItemEntity> VodItem { set; get; }
        public DbSet<BoardCastRoomEntity> BoardCastRoom { set; get; }
        public DjLiveCpContext() : base("DjLiveSystem")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DomainMap());
            modelBuilder.Configurations.Add(new AccountMap());
            modelBuilder.Configurations.Add(new TranscodeMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}