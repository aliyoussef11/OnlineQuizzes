namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            AddColumn("dbo.Quizs", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Quizs", "CategoryID");
            AddForeignKey("dbo.Quizs", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
            DropColumn("dbo.Quizs", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Quizs", "Category", c => c.String(nullable: false));
            DropForeignKey("dbo.Quizs", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Quizs", new[] { "CategoryID" });
            DropColumn("dbo.Quizs", "CategoryID");
            DropTable("dbo.Categories");
        }
    }
}
