namespace DCC_Parish_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minusnotificationsaddarticlenotications : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "CommentNotificationId", "dbo.CommentNotifications");
            DropForeignKey("dbo.Notifications", "SubscriptionId", "dbo.Subscriptions");
            DropIndex("dbo.Notifications", new[] { "SubscriptionId" });
            DropIndex("dbo.Notifications", new[] { "CommentNotificationId" });
            CreateTable(
                "dbo.ArticleNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        AspNetUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            DropTable("dbo.Notifications");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriptionId = c.Int(nullable: false),
                        CommentNotificationId = c.Int(nullable: false),
                        AspNetUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ArticleNotifications", "ArticleId", "dbo.Articles");
            DropIndex("dbo.ArticleNotifications", new[] { "ArticleId" });
            DropTable("dbo.ArticleNotifications");
            CreateIndex("dbo.Notifications", "CommentNotificationId");
            CreateIndex("dbo.Notifications", "SubscriptionId");
            AddForeignKey("dbo.Notifications", "SubscriptionId", "dbo.Subscriptions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Notifications", "CommentNotificationId", "dbo.CommentNotifications", "Id", cascadeDelete: true);
        }
    }
}
