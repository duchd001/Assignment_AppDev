namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Trainers", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trainers", "Email", c => c.String());
        }
    }
}
