namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Summaries", new[] { "Project_Id" });
            AlterColumn("dbo.Summaries", "Project_Id", c => c.Int());
            CreateIndex("dbo.Summaries", "Project_Id");
            AddForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Summaries", new[] { "Project_Id" });
            AlterColumn("dbo.Summaries", "Project_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Summaries", "Project_Id");
            AddForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
