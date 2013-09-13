using System.Collections.ObjectModel;
using Zcu.StudentEvaluator.Core.Collection;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Domain
{
	/// <summary>
	/// This represents in-memory basic repository of students and their evaluations
	/// </summary>
	public abstract class StudentEvaluationRepository :  IStudentEvaluationRepository
	{
		/// <summary>
		/// Gets the list of evaluation categories.
		/// </summary>
		/// <value>
		/// The list of categories to be contained.
		/// </value>
		public ObservableCollection<Category> Categories { get; private set; }

		/// <summary>
		/// Gets the list of students.
		/// </summary>
		/// <value>
		/// The list of students to be contained.
		/// </value>
		public ObservableCollection<Student> Students { get; private set; }

		/// <summary>
		/// Gets the list of evaluations.
		/// </summary>
		/// <value>
		/// The list of evaluations to be contained.
		/// </value>
		public ObservableCollection<Evaluation> Evaluations { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentEvaluationRepository"/> class.
		/// </summary>
		public StudentEvaluationRepository()
		{
			var evCol = new EvaluationObservableCollectionWithParentSync();

			this.Categories = new CategoryObservableCollectionWithContentSync(evCol);
			this.Students = new StudentObservableCollectionWithContentSync(evCol);

			evCol.GlobalCategoryCollection = this.Categories;
			evCol.GlobalStudentCollection = this.Students;
			this.Evaluations = evCol;
		}		
	}
}
