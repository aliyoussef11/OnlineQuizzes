namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTrainerMajorsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerMajors",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CategoryID = c.Int(nullable: false),
                    })
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Trainers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerMajors", "Id", "dbo.Trainers");
            DropForeignKey("dbo.TrainerMajors", "CategoryID", "dbo.Categories");
            DropIndex("dbo.TrainerMajors", new[] { "CategoryID" });
            DropIndex("dbo.TrainerMajors", new[] { "Id" });
            DropTable("dbo.TrainerMajors");
        }
    }
}
