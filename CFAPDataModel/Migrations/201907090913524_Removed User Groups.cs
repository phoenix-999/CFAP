namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUserGroups : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries");
            DropIndex("dbo.UserGroupSummaries", new[] { "UserGroup_Id" });
            DropIndex("dbo.UserGroupSummaries", new[] { "Summary_Id" });
            AddColumn("dbo.Summaries", "UserGroup_Id", c => c.Int());
            CreateIndex("dbo.Summaries", "UserGroup_Id");
            AddForeignKey("dbo.Summaries", "UserGroup_Id", "dbo.UserGroups", "Id");
            DropTable("dbo.UserGroupSummaries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGroupSummaries",
                c => new
                    {
                        UserGroup_Id = c.Int(nullable: false),
                        Summary_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_Id, t.Summary_Id });
            
            DropForeignKey("dbo.Summaries", "UserGroup_Id", "dbo.UserGroups");
            DropIndex("dbo.Summaries", new[] { "UserGroup_Id" });
            DropColumn("dbo.Summaries", "UserGroup_Id");
            CreateIndex("dbo.UserGroupSummaries", "Summary_Id");
            CreateIndex("dbo.UserGroupSummaries", "UserGroup_Id");
            AddForeignKey("dbo.UserGroupSummaries", "Summary_Id", "dbo.Summaries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroupSummaries", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
        }
    }
}
