namespace StudentEvaluatorConsoleApp.Migrations
{
	using System.Data.Entity.Migrations;
	
	public partial class CategoryNameIsUnique : DbMigration
	{
		public override void Up()
		{
			AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 50));
			CreateIndex("dbo.Categories", "Name", true);
		}
		
		public override void Down()
		{
			DropIndex("dbo.Categories", "Name");
			AlterColumn("dbo.Categories", "Name", c => c.String());
		}
	}
}
