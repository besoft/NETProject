using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Model;

/// <summary>
/// Data Abstract Layer
/// </summary>
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
		public ICollection<Student> Students { get; set; }
		public ICollection<Evaluation> Evaluations { get; set; }
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
	}
}
