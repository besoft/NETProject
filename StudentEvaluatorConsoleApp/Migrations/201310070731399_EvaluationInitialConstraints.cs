namespace StudentEvaluatorConsoleApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluationInitialConstraints : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Evaluations", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Evaluations", "Student_Id", "dbo.Students");
            DropIndex("dbo.Evaluations", new[] { "Category_Id" });
            DropIndex("dbo.Evaluations", new[] { "Student_Id" });
            AlterColumn("dbo.Evaluations", "Category_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Evaluations", "Student_Id", c => c.Int(nullable: false));
            AddForeignKey("dbo.Evaluations", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Evaluations", "Student_Id", "dbo.Students", "Id", cascadeDelete: true);
            CreateIndex("dbo.Evaluations", "Category_Id");
            CreateIndex("dbo.Evaluations", "Student_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Evaluations", new[] { "Student_Id" });
            DropIndex("dbo.Evaluations", new[] { "Category_Id" });
            DropForeignKey("dbo.Evaluations", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Evaluations", "Category_Id", "dbo.Categories");
            AlterColumn("dbo.Evaluations", "Student_Id", c => c.Int());
            AlterColumn("dbo.Evaluations", "Category_Id", c => c.Int());
            CreateIndex("dbo.Evaluations", "Student_Id");
            CreateIndex("dbo.Evaluations", "Category_Id");
            AddForeignKey("dbo.Evaluations", "Student_Id", "dbo.Students", "Id");
            AddForeignKey("dbo.Evaluations", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
