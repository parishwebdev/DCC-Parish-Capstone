namespace DCC_Parish_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsubtoartnotific : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleNotifications", "SubscriptionId", c => c.Int(nullable: false));
            CreateIndex("dbo.ArticleNotifications", "SubscriptionId");
            AddForeignKey("dbo.ArticleNotifications", "SubscriptionId", "dbo.Subscriptions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleNotifications", "SubscriptionId", "dbo.Subscriptions");
            DropIndex("dbo.ArticleNotifications", new[] { "SubscriptionId" });
            DropColumn("dbo.ArticleNotifications", "SubscriptionId");
        }
    }
}
