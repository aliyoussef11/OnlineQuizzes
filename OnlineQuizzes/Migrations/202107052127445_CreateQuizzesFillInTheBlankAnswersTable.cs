namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuizzesFillInTheBlankAnswersTable : DbMigration
    {
        public override void Up()
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
            DropForeignKey("dbo.QuizFillInTheBlankAnswers", "Id", "dbo.Students");
            DropForeignKey("dbo.QuizFillInTheBlankAnswers", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizFillInTheBlankAnswers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.QuizFillInTheBlankAnswers", new[] { "QuestionID" });
            DropIndex("dbo.QuizFillInTheBlankAnswers", new[] { "QuizId" });
            DropIndex("dbo.QuizFillInTheBlankAnswers", new[] { "Id" });
            DropTable("dbo.QuizFillInTheBlankAnswers");
        }
    }
}
