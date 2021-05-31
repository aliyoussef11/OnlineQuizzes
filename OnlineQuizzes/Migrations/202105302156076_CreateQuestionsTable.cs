namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuestionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        QuizId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        QuestionText = c.String(nullable: false),
                        GradeOfQuestion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.QuestionTypes", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId)
                .Index(t => t.TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Questions", "TypeId", "dbo.QuestionTypes");
            DropIndex("dbo.Questions", new[] { "TypeId" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropTable("dbo.Questions");
        }
    }
}
