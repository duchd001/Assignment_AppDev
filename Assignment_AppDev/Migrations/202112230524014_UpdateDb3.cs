namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trainers", "TrainerID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Trainers", "TrainerID", c => c.Int(nullable: false));
        }
    }
}
