namespace OnlineQuizzes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateCategoriesTable : DbMigration
    {
        public override void Up()
        {
            Sql("insert into Categories Values('Computer Science - Computing')");
            Sql("insert into Categories Values('Computer Science - Security')");
            Sql("insert into Categories Values('Networking')");
            Sql("insert into Categories Values('Data Science')");
            Sql("insert into Categories Values('Machine Learning')");
            Sql("insert into Categories Values('English Language')");
            Sql("insert into Categories Values('Arabic Language')");
            Sql("insert into Categories Values('Biology')");
            Sql("insert into Categories Values('Chemistry')");
            Sql("insert into Categories Values('Math')");
            Sql("insert into Categories Values('Science')");
            Sql("insert into Categories Values('General Information')");
            Sql("insert into Categories Values('Physiology')");
            Sql("insert into Categories Values('Accounting')");
            Sql("insert into Categories Values('Auditing')");
            Sql("insert into Categories Values('Business')");
            Sql("insert into Categories Values('Finance')");
            Sql("insert into Categories Values('Human Resources')");
            Sql("insert into Categories Values('Insurance')");
            Sql("insert into Categories Values('Journalism')");
            Sql("insert into Categories Values('Publishing')");
            Sql("insert into Categories Values('Law')");
            Sql("insert into Categories Values('Technology')");
            Sql("insert into Categories Values('Electrician')");
            Sql("insert into Categories Values('Education')");
            Sql("insert into Categories Values('Teacher Education')");
            Sql("insert into Categories Values('History')");
            Sql("insert into Categories Values('Physics')");
            Sql("insert into Categories Values('Construction Engineering')");
            Sql("insert into Categories Values('Robotics')");
            Sql("insert into Categories Values('Survey')");
            Sql("insert into Categories Values('Mechanical Engineering')");
            Sql("insert into Categories Values('Petroleum')");
            Sql("insert into Categories Values('Nuclear Engineering')");
            Sql("insert into Categories Values('Foods and Nutrition')");
            Sql("insert into Categories Values('Medicine')");
            Sql("insert into Categories Values('Pharmacy')");
            Sql("insert into Categories Values('Nursing')");
            Sql("insert into Categories Values('CyberSecurity')");
            Sql("insert into Categories Values('Art')");
            Sql("insert into Categories Values('Economic')");
            Sql("insert into Categories Values('Religious')");
            Sql("insert into Categories Values('Transportation')");
            Sql("insert into Categories Values('Dance')");
            Sql("insert into Categories Values('Human Biology')");
            Sql("insert into Categories Values('Somatic BodyWork')");
            Sql("insert into Categories Values('Optometry')");
            Sql("insert into Categories Values('Dietetics')");
            Sql("insert into Categories Values('Medical')");
            Sql("insert into Categories Values('Technician')");
            Sql("insert into Categories Values('Data Processing')");
            Sql("insert into Categories Values('Genetics')");
        }
        
        public override void Down()
        {
        }
    }
}
