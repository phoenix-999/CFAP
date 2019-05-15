namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Summaries", new[] { "Project_Id1" });
            DropColumn("dbo.Summaries", "Project_Id");
            RenameColumn(table: "dbo.Summaries", name: "Project_Id1", newName: "Project_Id");
            AlterColumn("dbo.Summaries", "Project_Id", c => c.Int());
            CreateIndex("dbo.Summaries", "Project_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Summaries", new[] { "Project_Id" });
            AlterColumn("dbo.Summaries", "Project_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Summaries", name: "Project_Id", newName: "Project_Id1");
            AddColumn("dbo.Summaries", "Project_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Summaries", "Project_Id1");
        }
    }
}
