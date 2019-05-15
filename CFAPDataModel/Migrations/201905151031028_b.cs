namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Summaries", new[] { "Project_Id" });
            AddColumn("dbo.Summaries", "Project_Id1", c => c.Int());
            CreateIndex("dbo.Summaries", "Project_Id1");
            AddForeignKey("dbo.Summaries", "Project_Id1", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Summaries", "Project_Id1", "dbo.Projects");
            DropIndex("dbo.Summaries", new[] { "Project_Id1" });
            DropColumn("dbo.Summaries", "Project_Id1");
            CreateIndex("dbo.Summaries", "Project_Id");
            AddForeignKey("dbo.Summaries", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
