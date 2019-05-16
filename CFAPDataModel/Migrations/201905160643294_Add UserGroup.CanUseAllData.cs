namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserGroupCanUseAllData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserGroups", "CanUserAllData", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserGroups", "CanUserAllData");
        }
    }
}
