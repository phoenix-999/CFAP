namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AletrcolumntoUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accountables", "AccountableName", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.BudgetItems", "ItemName", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Projects", "ProjectName", c => c.String(nullable: false, maxLength: 70));
            CreateIndex("dbo.BudgetItems", "ItemName", unique: true);
            CreateIndex("dbo.Projects", "ProjectName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", new[] { "ProjectName" });
            DropIndex("dbo.BudgetItems", new[] { "ItemName" });
            AlterColumn("dbo.Projects", "ProjectName", c => c.String(nullable: false));
            AlterColumn("dbo.BudgetItems", "ItemName", c => c.String(nullable: false));
            AlterColumn("dbo.Accountables", "AccountableName", c => c.String(nullable: false));
        }
    }
}
