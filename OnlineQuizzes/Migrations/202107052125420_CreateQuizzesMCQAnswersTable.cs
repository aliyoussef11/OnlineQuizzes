namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuizzesMCQAnswersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuizMCQAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: false)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuizId)
                .Index(t => t.QuestionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizMCQAnswers", "Id", "dbo.Students");
            DropForeignKey("dbo.QuizMCQAnswers", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizMCQAnswers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.QuizMCQAnswers", new[] { "QuestionID" });
            DropIndex("dbo.QuizMCQAnswers", new[] { "QuizId" });
            DropIndex("dbo.QuizMCQAnswers", new[] { "Id" });
            DropTable("dbo.QuizMCQAnswers");
        }
    }
}
