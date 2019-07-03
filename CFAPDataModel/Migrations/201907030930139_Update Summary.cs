namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummary : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Summaries", "UserLastChangedId", "dbo.Users");
            DropIndex("dbo.Summaries", new[] { "UserLastChangedId" });
            AddColumn("dbo.Summaries", "SummaUSD", c => c.Double(nullable: false));
            AddColumn("dbo.Summaries", "SummaEuro", c => c.Double(nullable: false));
            AlterColumn("dbo.Summaries", "UserLastChangedId", c => c.Int(nullable: false));
            CreateIndex("dbo.Summaries", "UserLastChangedId");
            AddForeignKey("dbo.Summaries", "UserLastChangedId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Summaries", "UserLastChangedId", "dbo.Users");
            DropIndex("dbo.Summaries", new[] { "UserLastChangedId" });
            AlterColumn("dbo.Summaries", "UserLastChangedId", c => c.Int());
            DropColumn("dbo.Summaries", "SummaEuro");
            DropColumn("dbo.Summaries", "SummaUSD");
            CreateIndex("dbo.Summaries", "UserLastChangedId");
            AddForeignKey("dbo.Summaries", "UserLastChangedId", "dbo.Users", "Id");
        }
    }
}
