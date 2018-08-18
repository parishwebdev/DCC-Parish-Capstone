namespace DCC_Parish_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustedArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Body", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Body");
        }
    }
}
