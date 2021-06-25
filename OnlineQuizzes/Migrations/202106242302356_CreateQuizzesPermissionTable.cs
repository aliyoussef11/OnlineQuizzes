namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuizzesPermissionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuizPermissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizID = c.Int(nullable: false),
                    })
                .ForeignKey("dbo.Quizs", t => t.QuizID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuizID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizPermissions", "Id", "dbo.Students");
            DropForeignKey("dbo.QuizPermissions", "QuizID", "dbo.Quizs");
            DropIndex("dbo.QuizPermissions", new[] { "QuizID" });
            DropIndex("dbo.QuizPermissions", new[] { "Id" });
            DropTable("dbo.QuizPermissions");
        }
    }
}
