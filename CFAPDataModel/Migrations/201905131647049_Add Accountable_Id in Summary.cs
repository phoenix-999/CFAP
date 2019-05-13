namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountable_IdinSummary : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables");
            DropIndex("dbo.Summaries", new[] { "Accountable_Id" });
            AlterColumn("dbo.Summaries", "Accountable_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Summaries", "Accountable_Id");
            AddForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables");
            DropIndex("dbo.Summaries", new[] { "Accountable_Id" });
            AlterColumn("dbo.Summaries", "Accountable_Id", c => c.Int());
            CreateIndex("dbo.Summaries", "Accountable_Id");
            AddForeignKey("dbo.Summaries", "Accountable_Id", "dbo.Accountables", "Id");
        }
    }
}
