namespace VehicleReservationSystem.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableForVehicleSystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RememberPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RememberToken = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Single(nullable: false),
                        TravelTime = c.Time(nullable: false, precision: 7),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Destination_Id = c.Int(),
                        Origin_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.Destination_Id)
                .ForeignKey("dbo.Cities", t => t.Origin_Id)
                .Index(t => t.Destination_Id)
                .Index(t => t.Origin_Id);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeatNumber = c.Int(nullable: false),
                        Is_Available = c.Boolean(nullable: false),
                        Vehicle_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id, cascadeDelete: true)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        ImageUrl = c.String(maxLength: 300, unicode: false),
                        TotalSeats = c.Int(nullable: false),
                        VehicleType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleType_Id)
                .Index(t => t.VehicleType_Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartureTime = c.Time(nullable: false, precision: 7),
                        Route_Id = c.Int(),
                        Vehicle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.Route_Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Route_Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginId = c.String(nullable: false, maxLength: 20, unicode: false),
                        Password = c.String(nullable: false, maxLength: 20, unicode: false),
                        FistName = c.String(maxLength: 20, unicode: false),
                        LastName = c.String(maxLength: 20, unicode: false),
                        ImageUrl = c.String(maxLength: 300, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        PhoneNumber = c.String(maxLength: 20, unicode: false),
                        Is_Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RememberPasswords", "Id", "dbo.Users");
            DropForeignKey("dbo.TimeTables", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.TimeTables", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.Seats", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "VehicleType_Id", "dbo.VehicleTypes");
            DropForeignKey("dbo.Routes", "Origin_Id", "dbo.Cities");
            DropForeignKey("dbo.Routes", "Destination_Id", "dbo.Cities");
            DropIndex("dbo.RememberPasswords", new[] { "Id" });
            DropIndex("dbo.TimeTables", new[] { "Vehicle_Id" });
            DropIndex("dbo.TimeTables", new[] { "Route_Id" });
            DropIndex("dbo.Seats", new[] { "Vehicle_Id" });
            DropIndex("dbo.Vehicles", new[] { "VehicleType_Id" });
            DropIndex("dbo.Routes", new[] { "Origin_Id" });
            DropIndex("dbo.Routes", new[] { "Destination_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.TimeTables");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Seats");
            DropTable("dbo.Routes");
            DropTable("dbo.RememberPasswords");
            DropTable("dbo.Cities");
        }
    }
}
