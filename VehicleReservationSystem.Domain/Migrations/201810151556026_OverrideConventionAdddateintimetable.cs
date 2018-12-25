namespace VehicleReservationSystem.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideConventionAdddateintimetable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeTables", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeTables", "Date");
        }
    }
}
