namespace Wysnan.EIMOnline.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("PersonnelAttendance", "SecurityUser2ID", c => c.Int(nullable: false));
            AddColumn("PersonnelAttendance", "SecurityUser_ID", c => c.Int());
            AddForeignKey("PersonnelAttendance", "SecurityUser2ID", "SecurityUser", "ID");
            AddForeignKey("PersonnelAttendance", "SecurityUser_ID", "SecurityUser", "ID");
            CreateIndex("PersonnelAttendance", "SecurityUser2ID");
            CreateIndex("PersonnelAttendance", "SecurityUser_ID");
        }
        
        public override void Down()
        {
            DropIndex("PersonnelAttendance", new[] { "SecurityUser_ID" });
            DropIndex("PersonnelAttendance", new[] { "SecurityUser2ID" });
            DropForeignKey("PersonnelAttendance", "SecurityUser_ID", "SecurityUser");
            DropForeignKey("PersonnelAttendance", "SecurityUser2ID", "SecurityUser");
            DropColumn("PersonnelAttendance", "SecurityUser_ID");
            DropColumn("PersonnelAttendance", "SecurityUser2ID");
        }
    }
}
