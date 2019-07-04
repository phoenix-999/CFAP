namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeriods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Month, t.Year });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Periods");
        }
    }
}
