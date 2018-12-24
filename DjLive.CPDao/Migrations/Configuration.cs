using System.Collections.Generic;
using DjLive.Control.Service.Impl;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using Newtonsoft.Json;

namespace DjLive.CPDao.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    internal sealed class Configuration : DbMigrationsConfiguration<DjLive.CPDao.Context.DjLiveCpContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DjLive.CPDao.Context.DjLiveCpContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var account = context.Account.FirstOrDefault(item => item.Id == "4c6c9a453a8c4c3280f34e4b95ddd9f2") ??
                          context.Account.Add(new AccountEntity()
                          {
                              Id = "4c6c9a453a8c4c3280f34e4b95ddd9f2",
                              UserName = "admin",
                              Password = "admin",
                              RoleType = 4096,
                              StatType = 1,
                              Token = "",
                              Domains = new List<DomainEntity>(),
                          });
            var server = context.Server.FirstOrDefault(item => item.Id == "fdd5659abd28416d91b7711581cfa6a0") ??
            context.Server.Add(new ServerEntity()
            {
                Id = "fdd5659abd28416d91b7711581cfa6a0",
                Name = "出版集团节点1",
                Ip = "http://172.28.10.136",
                Url = "http://172.28.10.136",
                UserName = "root",
                Password = "P@96*sword",
                Option = JsonConvert.SerializeObject(new LiveServiceConfig
                {
                    Host = "172.28.10.136",
                    HostUrl = "http://172.28.10.136",
                    SshPort = 25512,
                    UserName = "root",
                    Password = "P@96*sword",
                    ApiPort = 1985,
                    HttpPort = 8080,
                    RtmpPort = 1935,
                }),
                AccountEntities = new List<AccountEntity>() { account },
            });
            var policy = context.SecurePolicy.FirstOrDefault(item => item.Id == "76eb7d3b40b64370939e02f04ad6b3a8") ??
            context.SecurePolicy.Add(new SecurePolicyEntity()
            {
                Id = "76eb7d3b40b64370939e02f04ad6b3a8",
                Name = "DefaultSecure",
                AuthPlayUrl = $"http://172.28.10.137:62000/api/State/PlayVertify",
                AuthPublishUrl = "http://172.28.10.137:62000/api/State/PublishVertify",
                AuthConnectUrl = "http://172.28.10.137:62000/api/State/ConnectVertify",
                AuthDvrUrl = "http://172.28.10.137:62000/api/State/DvrCallback",
                AuthCloseUrl = "http://172.28.10.137:62000/api/State/DisConnectVertify",
                AuthStopUrl = "http://172.28.10.137:62000/api/State/PlayDone",
                AuthUnPublishUrl = "http://172.28.10.137:62000/api/State/PublishDone",
                AccountEntities = new List<AccountEntity>() { account },
            });
            context.SaveChanges();

        }
    }
}
