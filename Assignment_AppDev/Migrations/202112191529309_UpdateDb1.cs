namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignTraineeToCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TraineeID = c.String(maxLength: 128),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeID)
                .Index(t => t.TraineeID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.AssignTrainerToCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TrainerID = c.String(maxLength: 128),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerID)
                .Index(t => t.TrainerID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignTrainerToCourses", "TrainerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssignTrainerToCourses", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.AssignTraineeToCourses", "TraineeID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssignTraineeToCourses", "CourseID", "dbo.Courses");
            DropIndex("dbo.AssignTrainerToCourses", new[] { "CourseID" });
            DropIndex("dbo.AssignTrainerToCourses", new[] { "TrainerID" });
            DropIndex("dbo.AssignTraineeToCourses", new[] { "CourseID" });
            DropIndex("dbo.AssignTraineeToCourses", new[] { "TraineeID" });
            DropTable("dbo.AssignTrainerToCourses");
            DropTable("dbo.AssignTraineeToCourses");
        }
    }
}
