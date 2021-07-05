namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuizzesAnswersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuizAnswers",
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
            DropForeignKey("dbo.QuizAnswers", "Id", "dbo.Students");
            DropForeignKey("dbo.QuizAnswers", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizAnswers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.QuizAnswers", new[] { "QuestionID" });
            DropIndex("dbo.QuizAnswers", new[] { "QuizId" });
            DropIndex("dbo.QuizAnswers", new[] { "Id" });
            DropTable("dbo.QuizAnswers");
        }
    }
}
