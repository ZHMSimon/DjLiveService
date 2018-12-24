using System.Data.Entity;
using DjLive.CPDao.Context;

namespace DjLive.CPDao.Migrations
{
    public static class DataBaseInitialize
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DjLiveCpContext, DjLive.CPDao.Migrations.Configuration>());
            using (var context = new DjLiveCpContext())
            {
                context.Database.Initialize(true);
            }
        }
    }
}