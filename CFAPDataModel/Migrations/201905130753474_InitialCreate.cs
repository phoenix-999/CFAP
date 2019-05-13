namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accountables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountableName = c.String(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ReadOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BudgetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ReadOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DescriptionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ReadOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ReadOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Summaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        SummaGrn = c.Double(nullable: false),
                        SummaDolar = c.Double(nullable: false),
                        CashFlowType = c.Boolean(nullable: false),
                        ActionDate = c.DateTime(nullable: false),
                        ReadOnly = c.Boolean(nullable: false),
                        Accountable_Id = c.Int(),
                        BudgetItem_Id = c.Int(),
                        Description_Id = c.Int(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accountables", t => t.Accountable_Id)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id)
                .ForeignKey("dbo.DescriptionItems", t => t.Description_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Accountable_Id)
                .Index(t => t.BudgetItem_Id)
                .Index(t => t.Description_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 70),
                        Password = c.String(nullable: false),
                        CanAddNewUsers = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateRate = c.DateTime(nullable: false),
                        Dolar = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        ReadOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BudgetItemAccountables",
                c => new
                    {
                        BudgetItem_Id = c.Int(nullable: false),
                        Accountable_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BudgetItem_Id, t.Accountable_Id })
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accountables", t => t.Accountable_Id, cascadeDelete: true)
                .Index(t => t.BudgetItem_Id)
                .Index(t => t.Accountable_Id);
            
            CreateTable(
                "dbo.DescriptionItemAccountables",
                c => new
                    {
                        DescriptionItem_Id = c.Int(nullable: false),
                        Accountable_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DescriptionItem_Id, t.Accountable_Id })
                .ForeignKey("dbo.DescriptionItems", t => t.DescriptionItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accountables", t => t.Accountable_Id, cascadeDelete: true)
                .Index(t => t.DescriptionItem_Id)
                .Index(t => t.Accountable_Id);
            
            CreateTable(
                "dbo.DescriptionItemBudgetItems",
                c => new
                    {
                        DescriptionItem_Id = c.Int(nullable: false),
                        BudgetItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DescriptionItem_Id, t.BudgetItem_Id })
                .ForeignKey("dbo.DescriptionItems", t => t.DescriptionItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id, cascadeDelete: true)
                .Index(t => t.DescriptionItem_Id)
                .Index(t => t.BudgetItem_Id);
            
            CreateTable(
                "dbo.ProjectAccountables",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Accountable_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Accountable_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accountables", t => t.Accountable_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Accountable_Id);
            
            CreateTable(
                "dbo.ProjectBudgetItems",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        BudgetItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.BudgetItem_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.BudgetItem_Id);
            
            CreateTable(
                "dbo.ProjectDescriptionItems",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        DescriptionItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.DescriptionItem_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.DescriptionItems", t => t.DescriptionItem_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.DescriptionItem_Id);
            
            CreateTable(
                "dbo.UserGroupAccountables",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        Accountable_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.Accountable_Id })
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accountables", t => t.Accountable_Id, cascadeDelete: true)
                .Index(t => t.UserGroup_Id)
                .Index(t => t.Accountable_Id);
            
            CreateTable(
                "dbo.UserGroupBudgetItems",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        BudgetItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.BudgetItem_Id })
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItem_Id, cascadeDelete: true)
                .Index(t => t.UserGroup_Id)
                .Index(t => t.BudgetItem_Id);
            
            CreateTable(
                "dbo.UserGroupDescriptionItems",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        DescriptionItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.DescriptionItem_Id })
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.DescriptionItems", t => t.DescriptionItem_Id, cascadeDelete: true)
                .Index(t => t.UserGroup_Id)
                .Index(t => t.DescriptionItem_Id);
            
            CreateTable(
                "dbo.UserGroupProjects",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.Project_Id })
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.UserGroup_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.UserGroupSummaries",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        Summary_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.Summary_Id })
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Summaries", t => t.Summary_Id, cascadeDelete: true)
                .Index(t => t.UserGroup_Id)
                .Index(t => t.Summary_Id);
            
            CreateTable(
                "dbo.UserUserGroups",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        UserGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.UserGroup_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.UserGroup_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserUserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries");
            DropForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.UserGroupProjects", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupDescriptionItems", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.UserGroupDescriptionItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Summaries", "Description_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.Summaries", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.ProjectDescriptionItems", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.ProjectDescriptionItems", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectBudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.ProjectBudgetItems", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.ProjectAccountables", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DescriptionItemBudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.DescriptionItemBudgetItems", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.DescriptionItemAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.DescriptionItemAccountables", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.BudgetItemAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.BudgetItemAccountables", "BudgetItem_Id", "dbo.BudgetItems");
            DropIndex("dbo.UserUserGroups", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserUserGroups", new[] { "User_Id" });
            DropIndex("dbo.UserGroupSummaries", new[] { "Summary_Id" });
            DropIndex("dbo.UserGroupSummaries", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupProjects", new[] { "Project_Id" });
            DropIndex("dbo.UserGroupProjects", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupDescriptionItems", new[] { "DescriptionItem_Id" });
            DropIndex("dbo.UserGroupDescriptionItems", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupBudgetItems", new[] { "BudgetItem_Id" });
            DropIndex("dbo.UserGroupBudgetItems", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupAccountables", new[] { "Accountable_Id" });
            DropIndex("dbo.UserGroupAccountables", new[] { "UserGroup_Id" });
            DropIndex("dbo.ProjectDescriptionItems", new[] { "DescriptionItem_Id" });
            DropIndex("dbo.ProjectDescriptionItems", new[] { "Project_Id" });
            DropIndex("dbo.ProjectBudgetItems", new[] { "BudgetItem_Id" });
            DropIndex("dbo.ProjectBudgetItems", new[] { "Project_Id" });
            DropIndex("dbo.ProjectAccountables", new[] { "Accountable_Id" });
            DropIndex("dbo.ProjectAccountables", new[] { "Project_Id" });
            DropIndex("dbo.DescriptionItemBudgetItems", new[] { "BudgetItem_Id" });
            DropIndex("dbo.DescriptionItemBudgetItems", new[] { "DescriptionItem_Id" });
            DropIndex("dbo.DescriptionItemAccountables", new[] { "Accountable_Id" });
            DropIndex("dbo.DescriptionItemAccountables", new[] { "DescriptionItem_Id" });
            DropIndex("dbo.BudgetItemAccountables", new[] { "Accountable_Id" });
            DropIndex("dbo.BudgetItemAccountables", new[] { "BudgetItem_Id" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Summaries", new[] { "Project_Id" });
            DropIndex("dbo.Summaries", new[] { "Description_Id" });
            DropIndex("dbo.Summaries", new[] { "BudgetItem_Id" });
            DropIndex("dbo.Summaries", new[] { "Accountable_Id" });
            DropTable("dbo.UserUserGroups");
            DropTable("dbo.UserGroupSummaries");
            DropTable("dbo.UserGroupProjects");
            DropTable("dbo.UserGroupDescriptionItems");
            DropTable("dbo.UserGroupBudgetItems");
            DropTable("dbo.UserGroupAccountables");
            DropTable("dbo.ProjectDescriptionItems");
            DropTable("dbo.ProjectBudgetItems");
            DropTable("dbo.ProjectAccountables");
            DropTable("dbo.DescriptionItemBudgetItems");
            DropTable("dbo.DescriptionItemAccountables");
            DropTable("dbo.BudgetItemAccountables");
            DropTable("dbo.Rates");
            DropTable("dbo.Users");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Summaries");
            DropTable("dbo.Projects");
            DropTable("dbo.DescriptionItems");
            DropTable("dbo.BudgetItems");
            DropTable("dbo.Accountables");
        }
    }
}
