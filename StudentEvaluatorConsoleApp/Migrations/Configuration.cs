using System.Data.Entity.Migrations;
using Zcu.StudentEvaluator.DAL;

namespace Zcu.StudentEvaluator.Migrations
{    
	internal sealed class Configuration : DbMigrationsConfiguration<DbStudentEvaluationContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;	//changed to automatically update database to the latest version - see http://msdn.microsoft.com/en-us/data/jj591621
			//AutomaticMigrationDataLossAllowed = true;
		}

		protected override void Seed(DbStudentEvaluationContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//
		}
	}
}
