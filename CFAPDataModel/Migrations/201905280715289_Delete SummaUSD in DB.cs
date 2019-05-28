namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSummaUSDinDB : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Summaries", "SummaUSD");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Summaries", "SummaUSD", c => c.Double(nullable: false));
        }
    }
}
