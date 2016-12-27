namespace VTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ActivityDate = c.DateTime(nullable: false),
                        Mileage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Miles = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gallons = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 100),
                        Comments = c.String(maxLength: 500),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicle", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        AccountId = c.String(),
                        BuiltIn = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicle",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleName = c.String(nullable: false, maxLength: 50),
                        AccountId = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activity", "VehicleId", "dbo.Vehicle");
            DropForeignKey("dbo.Activity", "CategoryId", "dbo.Category");
            DropIndex("dbo.Activity", new[] { "CategoryId" });
            DropIndex("dbo.Activity", new[] { "VehicleId" });
            DropTable("dbo.Vehicle");
            DropTable("dbo.Category");
            DropTable("dbo.Activity");
        }
    }
}
