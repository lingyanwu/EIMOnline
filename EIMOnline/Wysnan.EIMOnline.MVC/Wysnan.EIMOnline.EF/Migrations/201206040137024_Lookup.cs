namespace Wysnan.EIMOnline.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Lookup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "zMetaLookup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SystemStatus = c.Byte(),
                        TimeStamp = c.Binary(),
                        Name = c.String(maxLength: 20),
                        Code = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("SecurityUser", "TestLookupID", c => c.Int());
            AddForeignKey("SecurityUser", "TestLookupID", "zMetaLookup", "ID");
            CreateIndex("SecurityUser", "TestLookupID");
        }
        
        public override void Down()
        {
            DropIndex("SecurityUser", new[] { "TestLookupID" });
            DropForeignKey("SecurityUser", "TestLookupID", "zMetaLookup");
            DropColumn("SecurityUser", "TestLookupID");
            DropTable("zMetaLookup");
        }
    }
}
