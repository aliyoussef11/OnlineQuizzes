namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSomeQuestionsTypes : DbMigration
    {
        public override void Up()
        {
            //Seeding the Questions Types Table
            Sql("insert into QuestionTypes values ('Multiple Choice')");
            Sql("insert into QuestionTypes values ('Fill In The Blank')");
        }
        
        public override void Down()
        {
        }
    }
}
