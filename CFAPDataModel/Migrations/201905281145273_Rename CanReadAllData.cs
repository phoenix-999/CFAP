namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCanReadAllData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserGroups", "CanReadAllData", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserGroups", "CanUserAllData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserGroups", "CanUserAllData", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserGroups", "CanReadAllData");
        }
    }
}
