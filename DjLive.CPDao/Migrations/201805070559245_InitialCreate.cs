namespace DjLive.CPDao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sys_account",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        UserName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        RoleType = c.Int(nullable: false),
                        StatType = c.Int(nullable: false),
                        Token = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sys_domain",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        SourceDomain = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        Description = c.String(maxLength: 500, storeType: "nvarchar"),
                        SourceType = c.Int(nullable: false),
                        StateType = c.Int(nullable: false),
                        RtmpPlayDomain = c.String(maxLength: 500, storeType: "nvarchar"),
                        FlvPlayDomain = c.String(maxLength: 500, storeType: "nvarchar"),
                        HlsPlayDomain = c.String(maxLength: 500, storeType: "nvarchar"),
                        AccountId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        RecordTemplateId = c.String(maxLength: 50, storeType: "nvarchar"),
                        SecureId = c.String(maxLength: 50, storeType: "nvarchar"),
                        ServerId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.sys_account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.sys_recordtemplate", t => t.RecordTemplateId)
                .ForeignKey("dbo.sys_securepolicy", t => t.SecureId)
                .ForeignKey("dbo.sys_servernode", t => t.ServerId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.RecordTemplateId)
                .Index(t => t.SecureId)
                .Index(t => t.ServerId);
            
            CreateTable(
                "dbo.sys_recordtemplate",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Description = c.String(maxLength: 200, storeType: "nvarchar"),
                        NamePolicy = c.String(maxLength: 200, storeType: "nvarchar"),
                        UploadUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sys_securepolicy",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        AuthPublishUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        AuthPlayUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        AuthConnectUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        AuthCloseUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        AuthUnPublishUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        AuthDvrUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        AuthStopUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        NotifyHlsUrl = c.String(maxLength: 200, storeType: "nvarchar"),
                        PublishReferAllow = c.String(maxLength: 2000, storeType: "nvarchar"),
                        PlayReferAllow = c.String(maxLength: 2000, storeType: "nvarchar"),
                        PublishIpAllow = c.String(maxLength: 2000, storeType: "nvarchar"),
                        PlayIpAllow = c.String(maxLength: 2000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sys_servernode",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Url = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        Ip = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        UserName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Option = c.String(nullable: false, maxLength: 2000, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sys_transcodetemplate",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        AppName = c.String(maxLength: 50, storeType: "nvarchar"),
                        VideoOption = c.String(nullable: false, maxLength: 2000, storeType: "nvarchar"),
                        AudioOption = c.String(nullable: false, maxLength: 2000, storeType: "nvarchar"),
                        LogoTemplateId = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.sys_logotemplate", t => t.LogoTemplateId)
                .Index(t => t.LogoTemplateId);
            
            CreateTable(
                "dbo.sys_logotemplate",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Base64Vale = c.String(nullable: false, unicode: false),
                        FilePath = c.String(maxLength: 500, storeType: "nvarchar"),
                        Option = c.String(maxLength: 2000, storeType: "nvarchar"),
                        AccountId = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.sys_account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.sys_boardcastroom",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        State = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Domain = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        AppName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        StreamName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        ExpireTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sys_voditem",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        RecordDateTime = c.DateTime(nullable: false, precision: 0),
                        TimeLength = c.Int(nullable: false),
                        DownLoadTimes = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Ip = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        HostName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        AppName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        StreamName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sys_alli_domain_transcode",
                c => new
                    {
                        DomainId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        TranscodeId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.DomainId, t.TranscodeId })
                .ForeignKey("dbo.sys_domain", t => t.DomainId, cascadeDelete: true)
                .ForeignKey("dbo.sys_transcodetemplate", t => t.TranscodeId, cascadeDelete: true)
                .Index(t => t.DomainId)
                .Index(t => t.TranscodeId);
            
            CreateTable(
                "dbo.sys_alli_account_recordtemplate",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        TemplateId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.AccountId, t.TemplateId })
                .ForeignKey("dbo.sys_account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.sys_recordtemplate", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.TemplateId);
            
            CreateTable(
                "dbo.sys_alli_account_securepolicy",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        PolicyId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.AccountId, t.PolicyId })
                .ForeignKey("dbo.sys_account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.sys_securepolicy", t => t.PolicyId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.PolicyId);
            
            CreateTable(
                "dbo.sys_alli_account_servernode",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        NodeId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.AccountId, t.NodeId })
                .ForeignKey("dbo.sys_account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.sys_servernode", t => t.NodeId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.NodeId);
            
            CreateTable(
                "dbo.sys_alli_account_transcodetemplate",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        TemplateId = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.AccountId, t.TemplateId })
                .ForeignKey("dbo.sys_account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.sys_transcodetemplate", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.TemplateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sys_alli_account_transcodetemplate", "TemplateId", "dbo.sys_transcodetemplate");
            DropForeignKey("dbo.sys_alli_account_transcodetemplate", "AccountId", "dbo.sys_account");
            DropForeignKey("dbo.sys_alli_account_servernode", "NodeId", "dbo.sys_servernode");
            DropForeignKey("dbo.sys_alli_account_servernode", "AccountId", "dbo.sys_account");
            DropForeignKey("dbo.sys_alli_account_securepolicy", "PolicyId", "dbo.sys_securepolicy");
            DropForeignKey("dbo.sys_alli_account_securepolicy", "AccountId", "dbo.sys_account");
            DropForeignKey("dbo.sys_alli_account_recordtemplate", "TemplateId", "dbo.sys_recordtemplate");
            DropForeignKey("dbo.sys_alli_account_recordtemplate", "AccountId", "dbo.sys_account");
            DropForeignKey("dbo.sys_logotemplate", "AccountId", "dbo.sys_account");
            DropForeignKey("dbo.sys_alli_domain_transcode", "TranscodeId", "dbo.sys_transcodetemplate");
            DropForeignKey("dbo.sys_alli_domain_transcode", "DomainId", "dbo.sys_domain");
            DropForeignKey("dbo.sys_transcodetemplate", "LogoTemplateId", "dbo.sys_logotemplate");
            DropForeignKey("dbo.sys_domain", "ServerId", "dbo.sys_servernode");
            DropForeignKey("dbo.sys_domain", "SecureId", "dbo.sys_securepolicy");
            DropForeignKey("dbo.sys_domain", "RecordTemplateId", "dbo.sys_recordtemplate");
            DropForeignKey("dbo.sys_domain", "AccountId", "dbo.sys_account");
            DropIndex("dbo.sys_alli_account_transcodetemplate", new[] { "TemplateId" });
            DropIndex("dbo.sys_alli_account_transcodetemplate", new[] { "AccountId" });
            DropIndex("dbo.sys_alli_account_servernode", new[] { "NodeId" });
            DropIndex("dbo.sys_alli_account_servernode", new[] { "AccountId" });
            DropIndex("dbo.sys_alli_account_securepolicy", new[] { "PolicyId" });
            DropIndex("dbo.sys_alli_account_securepolicy", new[] { "AccountId" });
            DropIndex("dbo.sys_alli_account_recordtemplate", new[] { "TemplateId" });
            DropIndex("dbo.sys_alli_account_recordtemplate", new[] { "AccountId" });
            DropIndex("dbo.sys_alli_domain_transcode", new[] { "TranscodeId" });
            DropIndex("dbo.sys_alli_domain_transcode", new[] { "DomainId" });
            DropIndex("dbo.sys_logotemplate", new[] { "AccountId" });
            DropIndex("dbo.sys_transcodetemplate", new[] { "LogoTemplateId" });
            DropIndex("dbo.sys_domain", new[] { "ServerId" });
            DropIndex("dbo.sys_domain", new[] { "SecureId" });
            DropIndex("dbo.sys_domain", new[] { "RecordTemplateId" });
            DropIndex("dbo.sys_domain", new[] { "AccountId" });
            DropTable("dbo.sys_alli_account_transcodetemplate");
            DropTable("dbo.sys_alli_account_servernode");
            DropTable("dbo.sys_alli_account_securepolicy");
            DropTable("dbo.sys_alli_account_recordtemplate");
            DropTable("dbo.sys_alli_domain_transcode");
            DropTable("dbo.sys_voditem");
            DropTable("dbo.sys_boardcastroom");
            DropTable("dbo.sys_logotemplate");
            DropTable("dbo.sys_transcodetemplate");
            DropTable("dbo.sys_servernode");
            DropTable("dbo.sys_securepolicy");
            DropTable("dbo.sys_recordtemplate");
            DropTable("dbo.sys_domain");
            DropTable("dbo.sys_account");
        }
    }
}
