namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuizzesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizID = c.Int(nullable: false, identity: true),
                        TrainerName = c.String(nullable: false),
                        QuizName = c.String(nullable: false, maxLength: 30),
                        Category = c.String(nullable: false),
                        DurationOfQuiz = c.Int(nullable: false),
                        TimeOfQuiz = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuizID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Quizs");
        }
    }
}
