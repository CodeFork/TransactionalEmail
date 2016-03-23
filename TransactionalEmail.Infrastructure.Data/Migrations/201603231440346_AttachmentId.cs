namespace TransactionalEmail.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachmentId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Attachments");
            AlterColumn("dbo.Attachments", "AttachmentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Attachments", "AttachmentId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Attachments");
            AlterColumn("dbo.Attachments", "AttachmentId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Attachments", "AttachmentId");
        }
    }
}
