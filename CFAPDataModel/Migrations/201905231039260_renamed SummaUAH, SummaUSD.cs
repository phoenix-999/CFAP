namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedSummaUAHSummaUSD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Summaries", "SummaUAH", c => c.Double(nullable: false));
            AddColumn("dbo.Summaries", "SummaUSD", c => c.Double(nullable: false));
            DropColumn("dbo.Summaries", "SummaGrn");
            DropColumn("dbo.Summaries", "SummaDolar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Summaries", "SummaDolar", c => c.Double(nullable: false));
            AddColumn("dbo.Summaries", "SummaGrn", c => c.Double(nullable: false));
            DropColumn("dbo.Summaries", "SummaUSD");
            DropColumn("dbo.Summaries", "SummaUAH");
        }
    }
}
