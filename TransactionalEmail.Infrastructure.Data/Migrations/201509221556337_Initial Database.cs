namespace TransactionalEmail.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        EmailId = c.Int(nullable: false, identity: true),
                        EmailReference = c.String(),
                        AccountName = c.String(),
                        EmailUid = c.Long(),
                        Subject = c.String(),
                        PlainTextBody = c.String(),
                        HtmlBody = c.String(),
                        Date = c.DateTime(),
                        Direction = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmailId);
            
            CreateTable(
                "dbo.AppliedRules",
                c => new
                    {
                        AppliedRuleId = c.Int(nullable: false, identity: true),
                        RuleName = c.String(),
                        Email_EmailId = c.Int(),
                    })
                .PrimaryKey(t => t.AppliedRuleId)
                .ForeignKey("dbo.Emails", t => t.Email_EmailId, cascadeDelete: true)
                .Index(t => t.Email_EmailId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentId = c.String(nullable: false, maxLength: 128),
                        AttachmentName = c.String(),
                        MimeType = c.String(),
                        ByteArray = c.Binary(),
                        Email_EmailId = c.Int(),
                    })
                .PrimaryKey(t => t.AttachmentId)
                .ForeignKey("dbo.Emails", t => t.Email_EmailId, cascadeDelete: true)
                .Index(t => t.Email_EmailId);
            
            CreateTable(
                "dbo.EmailAddresses",
                c => new
                    {
                        EmailAddressId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Type = c.Int(nullable: false),
                        Email_EmailId = c.Int(),
                    })
                .PrimaryKey(t => t.EmailAddressId)
                .ForeignKey("dbo.Emails", t => t.Email_EmailId, cascadeDelete: true)
                .Index(t => t.Email_EmailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailAddresses", "Email_EmailId", "dbo.Emails");
            DropForeignKey("dbo.Attachments", "Email_EmailId", "dbo.Emails");
            DropForeignKey("dbo.AppliedRules", "Email_EmailId", "dbo.Emails");
            DropIndex("dbo.EmailAddresses", new[] { "Email_EmailId" });
            DropIndex("dbo.Attachments", new[] { "Email_EmailId" });
            DropIndex("dbo.AppliedRules", new[] { "Email_EmailId" });
            DropTable("dbo.EmailAddresses");
            DropTable("dbo.Attachments");
            DropTable("dbo.AppliedRules");
            DropTable("dbo.Emails");
        }
    }
}
