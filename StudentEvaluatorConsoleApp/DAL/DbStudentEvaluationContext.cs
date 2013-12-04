using System.Data.Entity;
using Zcu.StudentEvaluator.Model;

/**
 * Databaze bude vytvorena automaticky s nazvem Zcu.StudentEvaluator.DAL.DbStudentEvaluationContext
 * na Microsoft SQL Serveru: "(LocalDB)\v11.0" - toto je treba zadat do SQL Server Object Explorer
 */

/// <summary>
/// Data Abstract Layer
/// </summary>
namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Database Data context for student evaluations.
	/// </summary>
	/// <remarks>
	/// This is the main class that coordinates functionality for a given data model.
	/// </remarks>
	public class DbStudentEvaluationContext : DbContext
	{
		public DbSet<Student> Students { get; set; }
		public DbSet<Evaluation> Evaluations { get; set; }
		public DbSet<Category> Categories { get; set; }

		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		//{
		//	base.OnModelCreating(modelBuilder);

		//	modelBuilder.Entity<Student>().Property(p => p.PersonalNumber).HasMaxLength(20);
		//}
	}
}
