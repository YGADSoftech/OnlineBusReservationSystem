namespace VehicleReservationSystem.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideConventionAdddatenullableintimetable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeTables", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeTables", "Date", c => c.DateTime(nullable: false));
        }
    }
}
