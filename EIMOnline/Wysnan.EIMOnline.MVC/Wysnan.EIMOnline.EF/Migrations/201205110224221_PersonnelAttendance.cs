namespace Wysnan.EIMOnline.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PersonnelAttendance : DbMigration
    {
        public override void Up()
        {
            AddColumn("PersonnelAttendance", "IsPunchCard", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PersonnelAttendance", "IsPunchCard");
        }
    }
}
