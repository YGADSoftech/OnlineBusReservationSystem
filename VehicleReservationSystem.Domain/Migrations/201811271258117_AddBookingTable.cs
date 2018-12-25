namespace VehicleReservationSystem.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RememberPasswords", "Id", "dbo.Users");
            DropIndex("dbo.RememberPasswords", new[] { "Id" });
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        BookingDateTime = c.DateTime(nullable: false),
                        Phone = c.String(),
                        IdCard = c.String(),
                        email = c.String(),
                        Is_Paid = c.Boolean(nullable: false),
                        Is_Admin_Seen = c.Boolean(nullable: false),
                        TimeTables_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeTables", t => t.TimeTables_Id)
                .Index(t => t.TimeTables_Id);
            
            AddColumn("dbo.Seats", "Booking_Id", c => c.Int());
            AddColumn("dbo.Users", "RememberPassword_Id", c => c.Int());
            AlterColumn("dbo.RememberPasswords", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Seats", "Booking_Id");
            CreateIndex("dbo.Users", "RememberPassword_Id");
            AddForeignKey("dbo.Seats", "Booking_Id", "dbo.Bookings", "Id");
            AddForeignKey("dbo.Users", "RememberPassword_Id", "dbo.RememberPasswords", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RememberPassword_Id", "dbo.RememberPasswords");
            DropForeignKey("dbo.Bookings", "TimeTables_Id", "dbo.TimeTables");
            DropForeignKey("dbo.Seats", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.Users", new[] { "RememberPassword_Id" });
            DropIndex("dbo.Bookings", new[] { "TimeTables_Id" });
            DropIndex("dbo.Seats", new[] { "Booking_Id" });
            AlterColumn("dbo.RememberPasswords", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "RememberPassword_Id");
            DropColumn("dbo.Seats", "Booking_Id");
            DropTable("dbo.Bookings");
            CreateIndex("dbo.RememberPasswords", "Id");
            AddForeignKey("dbo.RememberPasswords", "Id", "dbo.Users", "Id");
        }
    }
}
