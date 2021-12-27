namespace Assignment_AppDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Trainees", name: "Trainees_Id", newName: "TraineeUser_Id");
            RenameIndex(table: "dbo.Trainees", name: "IX_Trainees_Id", newName: "IX_TraineeUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Trainees", name: "IX_TraineeUser_Id", newName: "IX_Trainees_Id");
            RenameColumn(table: "dbo.Trainees", name: "TraineeUser_Id", newName: "Trainees_Id");
        }
    }
}
