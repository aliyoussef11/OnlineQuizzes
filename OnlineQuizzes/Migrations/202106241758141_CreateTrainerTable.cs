namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTrainerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TrainerName = c.String(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                        Degree = c.String(nullable: false),
                        Experience = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainers", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Trainers", new[] { "Id" });
            DropTable("dbo.Trainers");
        }
    }
}
