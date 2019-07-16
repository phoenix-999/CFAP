namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUserGroupsfromall : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserUserGroups", newName: "UserGroupUsers");
            DropForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.ProjectUserGroups", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropIndex("dbo.UserGroupAccountables", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupAccountables", new[] { "Accountable_Id" });
            DropIndex("dbo.UserGroupBudgetItems", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupBudgetItems", new[] { "BudgetItem_Id" });
            DropIndex("dbo.ProjectUserGroups", new[] { "Project_Id" });
            DropIndex("dbo.ProjectUserGroups", new[] { "UserGroup_Id" });
            DropPrimaryKey("dbo.UserGroupUsers");
            AddColumn("dbo.Accountables", "UserGroup_Id", c => c.Int());
            AddColumn("dbo.BudgetItems", "UserGroup_Id", c => c.Int());
            AddColumn("dbo.Projects", "UserGroup_Id", c => c.Int());
            AddPrimaryKey("dbo.UserGroupUsers", new[] { "UserGroup_Id", "User_Id" });
            CreateIndex("dbo.Accountables", "UserGroup_Id");
            CreateIndex("dbo.BudgetItems", "UserGroup_Id");
            CreateIndex("dbo.Projects", "UserGroup_Id");
            AddForeignKey("dbo.Accountables", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.BudgetItems", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.Projects", "UserGroup_Id", "dbo.UserGroups", "Id");
            DropTable("dbo.UserGroupAccountables");
            DropTable("dbo.UserGroupBudgetItems");
            DropTable("dbo.ProjectUserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectUserGroups",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        UserGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.UserGroup_Id });
            
            CreateTable(
                "dbo.UserGroupBudgetItems",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        BudgetItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.BudgetItem_Id });
            
            CreateTable(
                "dbo.UserGroupAccountables",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        Accountable_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.Accountable_Id });
            
            DropForeignKey("dbo.Projects", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.BudgetItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Accountables", "UserGroup_Id", "dbo.UserGroups");
            DropIndex("dbo.Projects", new[] { "UserGroup_Id" });
            DropIndex("dbo.BudgetItems", new[] { "UserGroup_Id" });
            DropIndex("dbo.Accountables", new[] { "UserGroup_Id" });
            DropPrimaryKey("dbo.UserGroupUsers");
            DropColumn("dbo.Projects", "UserGroup_Id");
            DropColumn("dbo.BudgetItems", "UserGroup_Id");
            DropColumn("dbo.Accountables", "UserGroup_Id");
            AddPrimaryKey("dbo.UserGroupUsers", new[] { "User_Id", "UserGroup_Id" });
            CreateIndex("dbo.ProjectUserGroups", "UserGroup_Id");
            CreateIndex("dbo.ProjectUserGroups", "Project_Id");
            CreateIndex("dbo.UserGroupBudgetItems", "BudgetItem_Id");
            CreateIndex("dbo.UserGroupBudgetItems", "UserGroup_Id");
            CreateIndex("dbo.UserGroupAccountables", "Accountable_Id");
            CreateIndex("dbo.UserGroupAccountables", "UserGroup_Id");
            AddForeignKey("dbo.ProjectUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectUserGroups", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UserGroupUsers", newName: "UserUserGroups");
        }
    }
}
