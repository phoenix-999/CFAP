namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDescriptiontable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DescriptionItemUserGroups", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropForeignKey("dbo.DescriptionItemUserGroups", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Summaries", "DescriptionItem_Id", "dbo.DescriptionItems");
            DropIndex("dbo.Summaries", new[] { "DescriptionItem_Id" });
            DropIndex("dbo.DescriptionItemUserGroups", new[] { "DescriptionItem_Id" });
            DropIndex("dbo.DescriptionItemUserGroups", new[] { "UserGroup_Id" });
            AddColumn("dbo.Summaries", "Description", c => c.String());
            DropColumn("dbo.Summaries", "DescriptionItem_Id");
            DropTable("dbo.DescriptionItems");
            DropTable("dbo.DescriptionItemUserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DescriptionItemUserGroups",
                c => new
                    {
                        DescriptionItem_Id = c.Int(nullable: false),
                        UserGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DescriptionItem_Id, t.UserGroup_Id });
            
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
            
            AddColumn("dbo.Summaries", "DescriptionItem_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Summaries", "Description");
            CreateIndex("dbo.DescriptionItemUserGroups", "UserGroup_Id");
            CreateIndex("dbo.DescriptionItemUserGroups", "DescriptionItem_Id");
            CreateIndex("dbo.Summaries", "DescriptionItem_Id");
            AddForeignKey("dbo.Summaries", "DescriptionItem_Id", "dbo.DescriptionItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DescriptionItemUserGroups", "UserGroup_Id", "dbo.UserGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DescriptionItemUserGroups", "DescriptionItem_Id", "dbo.DescriptionItems", "Id", cascadeDelete: true);
        }
    }
}
