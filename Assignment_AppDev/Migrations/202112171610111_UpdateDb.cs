namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trainees", "TraineeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Trainers", "TrainerID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trainers", "TrainerID", c => c.String(nullable: false));
            AlterColumn("dbo.Trainees", "TraineeID", c => c.String(nullable: false));
        }
    }
}
