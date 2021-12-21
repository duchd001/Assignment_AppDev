namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDb01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainees", "Trainees_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Trainers", "Email", c => c.String());
            AddColumn("dbo.Trainers", "Trainers_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Trainees", "Trainees_Id");
            CreateIndex("dbo.Trainers", "Trainers_Id");
            AddForeignKey("dbo.Trainees", "Trainees_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Trainers", "Trainers_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainers", "Trainers_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainees", "Trainees_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Trainers", new[] { "Trainers_Id" });
            DropIndex("dbo.Trainees", new[] { "Trainees_Id" });
            DropColumn("dbo.Trainers", "Trainers_Id");
            DropColumn("dbo.Trainers", "Email");
            DropColumn("dbo.Trainees", "Trainees_Id");
        }
    }
}
