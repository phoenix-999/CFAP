namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSummary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Summaries", "SummaryDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Summaries", "UserLastChangedId", c => c.Int());
            CreateIndex("dbo.Summaries", "UserLastChangedId");
            AddForeignKey("dbo.Summaries", "UserLastChangedId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Summaries", "UserLastChangedId", "dbo.Users");
            DropIndex("dbo.Summaries", new[] { "UserLastChangedId" });
            DropColumn("dbo.Summaries", "UserLastChangedId");
            DropColumn("dbo.Summaries", "SummaryDate");
        }
    }
}
