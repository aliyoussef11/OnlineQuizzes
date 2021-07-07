namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMCQAnswersTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuizMCQsAnswers",
                c => new
                    {
                        mcqAnswerID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.mcqAnswerID)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: false)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuizId)
                .Index(t => t.QuestionID);
            
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.QuizMCQsAnswers", "Id", "dbo.Students");
            DropForeignKey("dbo.QuizMCQsAnswers", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizMCQsAnswers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.QuizMCQsAnswers", new[] { "QuestionID" });
            DropIndex("dbo.QuizMCQsAnswers", new[] { "QuizId" });
            DropIndex("dbo.QuizMCQsAnswers", new[] { "Id" });
            DropTable("dbo.QuizMCQsAnswers");
            CreateIndex("dbo.QuizMCQAnswers", "QuestionID");
            CreateIndex("dbo.QuizMCQAnswers", "QuizId");
            CreateIndex("dbo.QuizMCQAnswers", "Id");
            AddForeignKey("dbo.QuizMCQAnswers", "Id", "dbo.Students", "Id");
            AddForeignKey("dbo.QuizMCQAnswers", "QuizId", "dbo.Quizs", "QuizID", cascadeDelete: true);
            AddForeignKey("dbo.QuizMCQAnswers", "QuestionID", "dbo.Questions", "QuestionID", cascadeDelete: true);
        }
    }
}
