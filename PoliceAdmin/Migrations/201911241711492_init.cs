namespace PoliceAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TraficPolice", "is_Admin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TraficPolice", "is_Admin", c => c.Boolean(nullable: false));
        }
    }
}
