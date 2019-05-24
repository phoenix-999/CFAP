namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedUserCanChangeUsersData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CanChangeUsersData", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "CanAddNewUsers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CanAddNewUsers", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "CanChangeUsersData");
        }
    }
}
