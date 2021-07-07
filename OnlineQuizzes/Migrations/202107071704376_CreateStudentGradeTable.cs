namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateStudentGradeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Student_Grade",
                c => new
                    {
                        AttemptID = c.Int(nullable: false, identity: true),
                        Id = c.String(maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        Grade = c.Double(nullable: false),
                        TotalGrade = c.Double(nullable: false),
                        Result = c.String(),
                    })
                .PrimaryKey(t => t.AttemptID)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuizId);
            
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Student_Grade", "Id", "dbo.Students");
            DropForeignKey("dbo.Student_Grade", "QuizId", "dbo.Quizs");
            DropIndex("dbo.Student_Grade", new[] { "QuizId" });
            DropIndex("dbo.Student_Grade", new[] { "Id" });
            DropTable("dbo.Student_Grade");
            CreateIndex("dbo.StudentGrades", "QuizId");
            CreateIndex("dbo.StudentGrades", "Id");
            AddForeignKey("dbo.StudentGrades", "Id", "dbo.Students", "Id");
            AddForeignKey("dbo.StudentGrades", "QuizId", "dbo.Quizs", "QuizID", cascadeDelete: true);
        }
    }
}
