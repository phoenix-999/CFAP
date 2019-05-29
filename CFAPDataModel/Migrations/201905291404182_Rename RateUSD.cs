namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameRateUSD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rates", "RateUSD", c => c.Double(nullable: false));
            AlterColumn("dbo.Rates", "DateRate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.Rates", "Dolar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rates", "Dolar", c => c.Double(nullable: false));
            AlterColumn("dbo.Rates", "DateRate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Rates", "RateUSD");
        }
    }
}
