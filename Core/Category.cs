using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Text;
using Zcu.StudentEvaluator.Core.Collection;

namespace Zcu.StudentEvaluator.Core.Data
{
	/// <summary>
	/// This class contains settings for an evaluation
	/// </summary>
	public class Category
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the evaluation.
		/// </summary>
		/// <value>
		/// The name of the evaluation, e.g. "Comments in code".
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the minimal number of points required to pass.
		/// </summary>
		/// <value>
		/// The number of points to pass.
		/// </value>
		public decimal? MinPoints { get; set; }

		/// <summary>
		/// Gets or sets the maximal number of points that will count.
		/// </summary>
		/// <value>
		/// The maximal number of points that counts
		/// </value>
		public decimal? MaxPoints { get; set; }

		/// <summary>
		/// Gets the collection of evaluations.
		/// </summary>        
		public ObservableCollection<Evaluation> Evaluations { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Category" /> class.
		/// </summary>
		public Category()
		{
			this.Evaluations = new EvaluationObservableCollectionWithParentCategory(this);            
		}

		/// <summary>
		/// Adds the new evaluation for this student.
		/// </summary>
		/// <remarks>The evaluation may not be null and must be not assigned to any other student, i.e., evaluation.Student refers either to
		/// this object or to null; otherwise exception is raised. </remarks>
		/// <param name="evaluation">The evaluation.</param>
		public void AddEvaluation(Evaluation evaluation)
		{
			Contract.Requires<ArgumentNullException>(evaluation != null);
			Contract.Requires<ArgumentException>(evaluation.Category == null || evaluation.Category == this,
				"Cannot add an evaluation assigned already to another category");
			Contract.Ensures(this.Evaluations.Contains(evaluation));

			if (!this.Evaluations.Contains(evaluation))
				this.Evaluations.Add(evaluation);
		}

		/// <summary>
		/// Removes the evaluation.
		/// </summary>
		/// <param name="evaluation">The evaluation.</param>
		public void RemoveEvaluation(Evaluation evaluation)
		{
			Contract.Requires<ArgumentNullException>(evaluation != null);
			Contract.Requires<ArgumentException>(evaluation.Category == null || evaluation.Category == this,
				"Cannot remove an evaluation assigned to another category");
			
			this.Evaluations.Remove(evaluation);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			var sb = new StringBuilder(this.Name ?? "<?>");
						   
			if (this.MinPoints.HasValue && this.MaxPoints.HasValue)
				sb.AppendFormat(" [{0}-{1}b]", this.MinPoints, this.MaxPoints);
			else if (this.MinPoints.HasValue)
				sb.AppendFormat(" [min {0}b]", this.MinPoints);
			else if (this.MaxPoints.HasValue)
				sb.AppendFormat(" [max {0}b]", this.MaxPoints);
						
			return sb.ToString();
		}
	}
}
