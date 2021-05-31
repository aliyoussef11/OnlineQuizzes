namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMCQAnswersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MCQAnswers",
                c => new
                    {
                        MCQID = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(nullable: false),
                        FirstPossibleAnswer = c.String(nullable: false),
                        SecondPossibleAnswer = c.String(nullable: false),
                        ThirdPossibleAnswer = c.String(),
                        FourthPossibleAnswer = c.String(),
                        CorrectAnswer = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MCQID)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MCQAnswers", "QuestionID", "dbo.Questions");
            DropIndex("dbo.MCQAnswers", new[] { "QuestionID" });
            DropTable("dbo.MCQAnswers");
        }
    }
}
