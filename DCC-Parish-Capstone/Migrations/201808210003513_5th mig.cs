namespace DCC_Parish_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5thmig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleAuthor_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Articles", "ArticleAuthor_Id");
            AddForeignKey("dbo.Articles", "ArticleAuthor_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "ArticleAuthor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Articles", new[] { "ArticleAuthor_Id" });
            DropColumn("dbo.Articles", "ArticleAuthor_Id");
        }
    }
}
