namespace CFAPDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AletrcolumntoUniqueAccountables : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Accountables", "AccountableName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accountables", new[] { "AccountableName" });
        }
    }
}
