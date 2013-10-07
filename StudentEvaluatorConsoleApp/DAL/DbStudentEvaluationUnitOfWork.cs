using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Migrations;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// This represents local in-memory unit of work that does not support any persistence
	/// </summary>
	public class DbStudentEvaluationUnitOfWork : IStudentEvaluationUnitOfWork
	{
		protected DbStudentEvaluationContext _context;
		protected DbRepository<Student> _students;
		protected DbRepository<Category> _categories;
		protected DbRepository<Evaluation> _evaluations;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStudentEvaluationUnitOfWork"/> class with the default context.
		/// </summary>
		public DbStudentEvaluationUnitOfWork()
		{
			//Update automatically the database to the latest version
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbStudentEvaluationContext, Configuration>());

			this._context = new DbStudentEvaluationContext();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStudentEvaluationUnitOfWork"/> class.
		/// </summary>
		/// <param name="context">The data context.</param>
		public DbStudentEvaluationUnitOfWork(DbStudentEvaluationContext context)
		{
			this._context = context;
		}

		/// <summary>
		/// Gets the repository of students.
		/// </summary>
		/// <value>
		/// The students repository.
		/// </value>
		public IRepository<Student> Students
		{
			get 
			{
				if (this._students == null)
					this._students = new DbRepository<Student>(this._context);

				return this._students;
			}
		}

		/// <summary>
		/// Gets the repository of categories.
		/// </summary>
		/// <value>
		/// The categories repository.
		/// </value>
		public IRepository<Category> Categories
		{
			get 
			{
				if (this._categories == null)
					this._categories = new DbRepository<Category>(this._context);

				return this._categories;
			}
		}

		/// <summary>
		/// Gets the repository of evaluations.
		/// </summary>
		/// <value>
		/// The evaluations repository.
		/// </value>
		public IRepository<Evaluation> Evaluations
		{
			get 
			{
				if (this._evaluations == null)
					this._evaluations = new DbRepository<Evaluation>(this._context);

				return this._evaluations;
			}
		}

		/// <summary>
		/// Saves all changes done in repositories.
		/// </summary>		
		/// <remarks>
		/// Saves all changes into persistent stream.
		/// </remarks>
		public void Save()
		{			
			this._context.SaveChanges();
		}
	}
}
