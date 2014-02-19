using System.Data.Entity;
using Zcu.StudentEvaluator.Model;

/*
  Databaze bude vytvorena automaticky s nazvem Zcu.StudentEvaluator.DAL.DbStudentEvaluationContext
  na Microsoft SQL Serveru: "(LocalDB)\v11.0" - toto je treba zadat do SQL Server Object Explorer
 */

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
        /// <summary>
        /// Gets or sets the repository of students.
        /// </summary>
        /// <value>
        /// The students repository.
        /// </value>
		public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Gets or sets the repository of student evaluations.
        /// </summary>
        /// <value>
        /// The evaluations repository.
        /// </value>
		public DbSet<Evaluation> Evaluations { get; set; }

        /// <summary>
        /// Gets or sets the repository of categories for the evaluation.
        /// </summary>
        /// <value>
        /// The categories repository.
        /// </value>
		public DbSet<Category> Categories { get; set; }

		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		//{
		//	base.OnModelCreating(modelBuilder);

		//	modelBuilder.Entity<Student>().Property(p => p.PersonalNumber).HasMaxLength(20);
		//}
	}
}
