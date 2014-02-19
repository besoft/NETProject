using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// This represents local in-memory unit of work that does not support any persistence
	/// </summary>
	public class LocalStudentEvaluationUnitOfWork : IStudentEvaluationUnitOfWork
	{
        /// <summary>
        /// The database context
        /// </summary>
		protected LocalStudentEvaluationContext _context;

        /// <summary>
        /// The repository of students in the context
        /// </summary>
		protected LocalRepository<Student> _students;

        /// <summary>
        /// The repository of categories in the context
        /// </summary>
		protected LocalRepository<Category> _categories;

        /// <summary>
        /// The repository of evaluations in the context
        /// </summary>
        protected LocalRepository<Evaluation> _evaluations;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStudentEvaluationUnitOfWork"/> class with the default context.
		/// </summary>
		public LocalStudentEvaluationUnitOfWork()
		{
			this._context = new LocalStudentEvaluationContext();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStudentEvaluationUnitOfWork"/> class.
		/// </summary>
		/// <param name="context">The data context.</param>
		public LocalStudentEvaluationUnitOfWork(LocalStudentEvaluationContext context)
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
					this._students = new LocalRepository<Student>(this._context);

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
					this._categories = new LocalRepository<Category>(this._context);

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
					this._evaluations = new LocalRepository<Evaluation>(this._context);

				return this._evaluations;
			}
		}

		/// <summary>
		/// Saves all changes done in unitOfWork.
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
