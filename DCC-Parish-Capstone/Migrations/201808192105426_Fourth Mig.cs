namespace DCC_Parish_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CommentAuthor_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "CommentAuthor_Id");
            AddForeignKey("dbo.Comments", "CommentAuthor_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "CommentAuthor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "CommentAuthor_Id" });
            DropColumn("dbo.Comments", "CommentAuthor_Id");
        }
    }
}
