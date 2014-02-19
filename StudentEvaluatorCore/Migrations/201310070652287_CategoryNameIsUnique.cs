namespace StudentEvaluatorConsoleApp.Migrations
{
	using System.Data.Entity.Migrations;

    /// <summary>
    /// Database upgrade to ensure that the name of a cateogry is unique
    /// </summary>
	public partial class CategoryNameIsUnique : DbMigration
	{
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
		public override void Up()
		{
			AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 50));
			CreateIndex("dbo.Categories", "Name", true);
		}

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
		public override void Down()
		{
			DropIndex("dbo.Categories", "Name");
			AlterColumn("dbo.Categories", "Name", c => c.String());
		}
	}
}
