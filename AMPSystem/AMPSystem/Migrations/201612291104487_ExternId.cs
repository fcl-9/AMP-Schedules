namespace AMPSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alert",
                c => new
                    {
                        AlertID = c.Int(nullable: false, identity: true),
                        Time = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.AlertID);
            
            CreateTable(
                "dbo.EvaluationMoment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        Color = c.String(),
                        Description = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Editable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        EvaluationMoment_ID = c.Int(),
                        Lesson_ID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EvaluationMoment", t => t.EvaluationMoment_ID)
                .ForeignKey("dbo.Lesson", t => t.Lesson_ID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.EvaluationMoment_ID)
                .Index(t => t.Lesson_ID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        Floor = c.Int(nullable: false),
                        Building_Id = c.Int(),
                        EvaluationMoment_ID = c.Int(),
                        Lesson_ID = c.Int(),
                        OfficeHours_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Building", t => t.Building_Id)
                .ForeignKey("dbo.EvaluationMoment", t => t.EvaluationMoment_ID)
                .ForeignKey("dbo.Lesson", t => t.Lesson_ID)
                .ForeignKey("dbo.OfficeHours", t => t.OfficeHours_ID)
                .Index(t => t.Building_Id)
                .Index(t => t.EvaluationMoment_ID)
                .Index(t => t.Lesson_ID)
                .Index(t => t.OfficeHours_ID);
            
            CreateTable(
                "dbo.Building",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lesson",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        Color = c.String(),
                        Type = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        Editable = c.Boolean(nullable: false),
                        Teacher_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.Teacher_UserID)
                .Index(t => t.Teacher_UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.OfficeHours",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExternId = c.Int(nullable: false),
                        Name = c.String(),
                        Color = c.String(),
                        Description = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Editable = c.Boolean(nullable: false),
                        Teacher_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.Teacher_UserID)
                .Index(t => t.Teacher_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OfficeHours", "Teacher_UserID", "dbo.User");
            DropForeignKey("dbo.Room", "OfficeHours_ID", "dbo.OfficeHours");
            DropForeignKey("dbo.Lesson", "Teacher_UserID", "dbo.User");
            DropForeignKey("dbo.Course", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Room", "Lesson_ID", "dbo.Lesson");
            DropForeignKey("dbo.Course", "Lesson_ID", "dbo.Lesson");
            DropForeignKey("dbo.Room", "EvaluationMoment_ID", "dbo.EvaluationMoment");
            DropForeignKey("dbo.Room", "Building_Id", "dbo.Building");
            DropForeignKey("dbo.Course", "EvaluationMoment_ID", "dbo.EvaluationMoment");
            DropIndex("dbo.OfficeHours", new[] { "Teacher_UserID" });
            DropIndex("dbo.Lesson", new[] { "Teacher_UserID" });
            DropIndex("dbo.Room", new[] { "OfficeHours_ID" });
            DropIndex("dbo.Room", new[] { "Lesson_ID" });
            DropIndex("dbo.Room", new[] { "EvaluationMoment_ID" });
            DropIndex("dbo.Room", new[] { "Building_Id" });
            DropIndex("dbo.Course", new[] { "User_UserID" });
            DropIndex("dbo.Course", new[] { "Lesson_ID" });
            DropIndex("dbo.Course", new[] { "EvaluationMoment_ID" });
            DropTable("dbo.OfficeHours");
            DropTable("dbo.User");
            DropTable("dbo.Lesson");
            DropTable("dbo.Building");
            DropTable("dbo.Room");
            DropTable("dbo.Course");
            DropTable("dbo.EvaluationMoment");
            DropTable("dbo.Alert");
        }
    }
}
