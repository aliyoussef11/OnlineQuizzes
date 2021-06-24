namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateStudentInterestsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentInterests",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CategoryID = c.Int(nullable: false),
                    })
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentInterests", "Id", "dbo.Students");
            DropForeignKey("dbo.StudentInterests", "CategoryID", "dbo.Categories");
            DropIndex("dbo.StudentInterests", new[] { "CategoryID" });
            DropIndex("dbo.StudentInterests", new[] { "Id" });
            DropTable("dbo.StudentInterests");
        }
    }
}
