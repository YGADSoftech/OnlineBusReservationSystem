namespace VehicleReservationSystem.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideConventionaddboolinSeat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Seats", "Is_Available", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Seats", "Is_Available", c => c.Byte(nullable: false));
        }
    }
}
