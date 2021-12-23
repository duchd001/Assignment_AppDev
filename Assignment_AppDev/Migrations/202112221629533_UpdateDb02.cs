namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb02 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trainees", "TraineeID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trainees", "TraineeID", c => c.Int(nullable: false));
        }
    }
}
