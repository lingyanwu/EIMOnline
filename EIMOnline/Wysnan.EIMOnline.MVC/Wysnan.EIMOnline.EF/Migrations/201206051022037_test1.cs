namespace Wysnan.EIMOnline.EF.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("zMetaLookup", "Code", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("zMetaLookup", "Code", c => c.String(maxLength: 5));
        }
    }
}
