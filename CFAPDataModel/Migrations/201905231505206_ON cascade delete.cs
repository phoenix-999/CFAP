namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ONcascadedelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.Summaries", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.Summaries", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries");
            DropForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.DescriptionItemUserGroups", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.DescriptionItemUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.ProjectUserGroups", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserUserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserUserGroups", "UserGroup_Id", "dbo.UserGroups");
            AddForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Summaries", "BudgetItem_Id", "dbo.BudgetItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Summaries", "DescriptionItem_Id", "dbo.DescriptionItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DescriptionItemUserGroups", "DescriptionItem_Id", "dbo.DescriptionItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DescriptionItemUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectUserGroups", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserUserGroups", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserUserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.ProjectUserGroups", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DescriptionItemUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.DescriptionItemUserGroups", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries");
            DropForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Summaries", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.Summaries", "BudgetItem_Id", "dbo.BudgetItems");
            DropForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables");
            DropForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables");
            AddForeignKey("dbo.UserUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.UserUserGroups", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.ProjectUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.ProjectUserGroups", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.DescriptionItemUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.DescriptionItemUserGroups", "DescriptionItem_Id", "dbo.DescriptionItems", "Id");
            AddForeignKey("dbo.UserGroupBudgetItems", "BudgetItem_Id", "dbo.BudgetItems", "Id");
            AddForeignKey("dbo.UserGroupBudgetItems", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries", "Id");
            AddForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Summaries", "DescriptionItem_Id", "dbo.DescriptionItems", "Id");
            AddForeignKey("dbo.Summaries", "BudgetItem_Id", "dbo.BudgetItems", "Id");
            AddForeignKey("dbo.UserGroupAccountables", "Accountable_Id", "dbo.Accountables", "Id");
            AddForeignKey("dbo.UserGroupAccountables", "UserGroup_Id", "dbo.UserGroups", "Id");
            AddForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables", "Id");
        }
    }
}
