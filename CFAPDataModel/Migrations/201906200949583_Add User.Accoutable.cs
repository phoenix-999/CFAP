namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAccoutable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsAccountable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "AccountableId", c => c.Int());
            CreateIndex("dbo.Users", "AccountableId");
            AddForeignKey("dbo.Users", "AccountableId", "dbo.Accountables", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "AccountableId", "dbo.Accountables");
            DropIndex("dbo.Users", new[] { "AccountableId" });
            DropColumn("dbo.Users", "AccountableId");
            DropColumn("dbo.Users", "IsAccountable");
        }
    }
}
