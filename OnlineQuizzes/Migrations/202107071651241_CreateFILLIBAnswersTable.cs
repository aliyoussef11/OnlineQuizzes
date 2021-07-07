namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFILLIBAnswersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuizFillIBAnswers",
                c => new
                    {
                        FillIBAnswerID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.FillIBAnswerID)
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
                "dbo.QuizFillInTheBlankAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.QuizFillIBAnswers", "Id", "dbo.Students");
            DropForeignKey("dbo.QuizFillIBAnswers", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizFillIBAnswers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.QuizFillIBAnswers", new[] { "QuestionID" });
            DropIndex("dbo.QuizFillIBAnswers", new[] { "QuizId" });
            DropIndex("dbo.QuizFillIBAnswers", new[] { "Id" });
            DropTable("dbo.QuizFillIBAnswers");
            CreateIndex("dbo.QuizFillInTheBlankAnswers", "QuestionID");
            CreateIndex("dbo.QuizFillInTheBlankAnswers", "QuizId");
            CreateIndex("dbo.QuizFillInTheBlankAnswers", "Id");
            AddForeignKey("dbo.QuizFillInTheBlankAnswers", "Id", "dbo.Students", "Id");
            AddForeignKey("dbo.QuizFillInTheBlankAnswers", "QuizId", "dbo.Quizs", "QuizID", cascadeDelete: true);
            AddForeignKey("dbo.QuizFillInTheBlankAnswers", "QuestionID", "dbo.Questions", "QuestionID", cascadeDelete: true);
        }
    }
}
