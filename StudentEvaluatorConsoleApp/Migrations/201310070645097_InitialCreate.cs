namespace StudentEvaluatorConsoleApp.Migrations
{
	using System.Data.Entity.Migrations;
	
	public partial class InitialCreate : DbMigration
	{
		public override void Up()
		{
			CreateTable(
				"dbo.Students",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						PersonalNumber = c.String(nullable: false, maxLength: 10),
						FirstName = c.String(nullable: false, maxLength: 25),
						Surname = c.String(nullable: false, maxLength: 25),
					})
				.PrimaryKey(t => t.Id)
				.Index(t => t.PersonalNumber, true);	//PersonalNumber is unique
			
			CreateTable(
				"dbo.Evaluations",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Points = c.Decimal(precision: 18, scale: 2),
						Reason = c.String(),
						Category_Id = c.Int(),
						Student_Id = c.Int(),
					})
				.PrimaryKey(t => t.Id)
				.ForeignKey("dbo.Categories", t => t.Category_Id)
				.ForeignKey("dbo.Students", t => t.Student_Id)
				.Index(t => t.Category_Id)
				.Index(t => t.Student_Id);
			
			CreateTable(
				"dbo.Categories",
				c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Name = c.String(),
						MinPoints = c.Decimal(precision: 18, scale: 2),
						MaxPoints = c.Decimal(precision: 18, scale: 2),
					})
				.PrimaryKey(t => t.Id);
			
		}
		
		public override void Down()
		{
			DropIndex("dbo.Evaluations", new[] { "Student_Id" });
			DropIndex("dbo.Evaluations", new[] { "Category_Id" });
			DropForeignKey("dbo.Evaluations", "Student_Id", "dbo.Students");
			DropForeignKey("dbo.Evaluations", "Category_Id", "dbo.Categories");
			DropTable("dbo.Categories");
			DropTable("dbo.Evaluations");
			DropTable("dbo.Students");
		}
	}
}
