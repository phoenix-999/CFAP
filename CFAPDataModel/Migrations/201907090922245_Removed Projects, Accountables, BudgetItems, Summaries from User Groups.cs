namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedProjectsAccountablesBudgetItemsSummariesfromUserGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accountables", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.BudgetItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Projects", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Summaries", "UserGroup_Id", "dbo.UserGroups");
            DropIndex("dbo.Accountables", new[] { "UserGroup_Id" });
            DropIndex("dbo.Summaries", new[] { "UserGroup_Id" });
            DropIndex("dbo.BudgetItems", new[] { "UserGroup_Id" });
            DropIndex("dbo.Projects", new[] { "UserGroup_Id" });
            DropColumn("dbo.Accountables", "UserGroup_Id");
            DropColumn("dbo.Summaries", "UserGroup_Id");
            DropColumn("dbo.BudgetItems", "UserGroup_Id");
            DropColumn("dbo.Projects", "UserGroup_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "UserGroup_Id", c => c.Int());
            AddColumn("dbo.BudgetItems", "UserGroup_Id", c => c.Int());
            AddColumn("dbo.Summaries", "UserGroup_Id", c => c.Int());
            AddColumn("dbo.Accountables", "UserGroup_Id", c => c.Int());
            CreateIndex("dbo.Projects", "UserGroup_Id");
            CreateIndex("dbo.BudgetItems", "UserGroup_Id");
            CreateIndex("dbo.Summaries", "UserGroup_Id");
            CreateIndex("dbo.Accountables", "UserGroup_Id");
            AddForeignKey("dbo.Summaries", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.Projects", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.BudgetItems", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.Accountables", "UserGroup_Id", "dbo.UserGroups", "Id");
        }
    }
}
