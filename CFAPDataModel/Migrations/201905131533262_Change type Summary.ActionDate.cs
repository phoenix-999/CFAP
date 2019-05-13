namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangetypeSummaryActionDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Summaries", "ActionDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Summaries", "ActionDate", c => c.DateTime(nullable: false));
        }
    }
}
