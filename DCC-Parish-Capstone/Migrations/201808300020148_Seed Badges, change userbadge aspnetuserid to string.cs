namespace DCC_Parish_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedBadgeschangeuserbadgeaspnetuseridtostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserBadges", "AspNetUserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserBadges", "AspNetUserId", c => c.Int(nullable: false));
        }
    }
}
