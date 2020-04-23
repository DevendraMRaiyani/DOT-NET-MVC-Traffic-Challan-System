namespace PoliceAdmin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init9 : DbMigration
    {
        public override void Up()
        {
           
          
            CreateTable(
                "dbo.ChallanOnDLs",
                c => new
                    {
                        ChallanNo = c.String(nullable: false, maxLength: 128),
                        IssueDate = c.DateTime(nullable: false),
                        LicenceNo = c.String(),
                        RCNo = c.String(),
                        totalFine = c.Int(nullable: false),
                        Paid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChallanNo);
            
           
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Challans",
                c => new
                    {
                        ChallanNo = c.String(nullable: false, maxLength: 128),
                        IssueDate = c.DateTime(nullable: false),
                        totalFine = c.Int(nullable: false),
                        Paid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChallanNo);
            
            DropTable("dbo.ChallanOnDLs");
            CreateIndex("dbo.Challans", "ChallanNo");
            AddForeignKey("dbo.Challans", "ChallanNo", "dbo.DLs", "LicenceNo");
        }
    }
}
