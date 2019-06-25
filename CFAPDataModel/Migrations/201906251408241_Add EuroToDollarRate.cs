namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEuroToDollarRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rates", "EuroToDollarRate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rates", "EuroToDollarRate");
        }
    }
}
