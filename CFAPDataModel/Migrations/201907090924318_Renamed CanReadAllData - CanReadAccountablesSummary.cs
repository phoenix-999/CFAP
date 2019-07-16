namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedCanReadAllDataCanReadAccountablesSummary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserGroups", "CanReadAccountablesSummary", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserGroups", "CanReadAllData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserGroups", "CanReadAllData", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserGroups", "CanReadAccountablesSummary");
        }
    }
}
