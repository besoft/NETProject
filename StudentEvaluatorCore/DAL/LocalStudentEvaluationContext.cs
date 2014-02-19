using System.Collections.Generic;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Data context for student evaluations.
	/// </summary>
	/// <remarks>
	/// This is the main class that coordinates functionality for a given data model. Later we will update it to EF database context.
	/// </remarks>
	public class LocalStudentEvaluationContext
	{
        /// <summary>
        /// Gets or sets the repository of students.
        /// </summary>
        /// <value>
        /// The students repository.
        /// </value>
		public ICollection<Student> Students { get; set; }

        /// <summary>
        /// Gets or sets the repository of student evaluations.
        /// </summary>
        /// <value>
        /// The evaluations repository.
        /// </value>
		public ICollection<Evaluation> Evaluations { get; set; }

        /// <summary>
        /// Gets or sets the repository of categories for the evaluation.
        /// </summary>
        /// <value>
        /// The categories repository.
        /// </value>
		public ICollection<Category> Categories { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStudentEvaluationContext"/> class.
		/// </summary>
		public LocalStudentEvaluationContext()
		{
			this.Students = new HashSet<Student>();
			this.Evaluations = new HashSet<Evaluation>();
			this.Categories = new HashSet<Category>();
		}

		/// <summary>
		/// Saves all changes made in this context to the underlying physical stuff. 
		/// </summary>
		/// <returns>The number of objects written to the underlying physical stuff.</returns>
		public virtual int SaveChanges()
		{
			return 0;
		}
	}
}
