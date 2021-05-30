namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateQuestionTypesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        TypeID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.TypeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QuestionTypes");
        }
    }
}
