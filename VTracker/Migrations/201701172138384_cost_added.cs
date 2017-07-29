namespace VTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cost_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "Cost", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activity", "Cost");
        }
    }
}
