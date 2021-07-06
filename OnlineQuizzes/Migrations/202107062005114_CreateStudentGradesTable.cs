namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateStudentGradesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentGrades",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        Grade = c.Double(nullable: false),
                        TotalGrade = c.Double(nullable: false),
                        Result = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuizId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentGrades", "Id", "dbo.Students");
            DropForeignKey("dbo.StudentGrades", "QuizId", "dbo.Quizs");
            DropIndex("dbo.StudentGrades", new[] { "QuizId" });
            DropIndex("dbo.StudentGrades", new[] { "Id" });
            DropTable("dbo.StudentGrades");
        }
    }
}
