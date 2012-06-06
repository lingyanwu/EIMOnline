namespace Wysnan.EIMOnline.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("PersonnelAttendance", "TestLookupID", c => c.Int(nullable: false));
            AddForeignKey("PersonnelAttendance", "TestLookupID", "zMetaLookup", "ID");
            CreateIndex("PersonnelAttendance", "TestLookupID");
        }
        
        public override void Down()
        {
            DropIndex("PersonnelAttendance", new[] { "TestLookupID" });
            DropForeignKey("PersonnelAttendance", "TestLookupID", "zMetaLookup");
            DropColumn("PersonnelAttendance", "TestLookupID");
        }
    }
}
