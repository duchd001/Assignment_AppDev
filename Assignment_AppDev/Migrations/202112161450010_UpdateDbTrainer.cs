namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDbTrainer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Trainers", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainers", "Address");
            DropColumn("dbo.Trainers", "Age");
        }
    }
}
