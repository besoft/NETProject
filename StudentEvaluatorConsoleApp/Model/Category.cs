using System.Collections.Generic;

namespace Zcu.StudentEvaluator.Model
{
	/// <summary>
	/// This class contains settings for an evaluation
	/// </summary>
	public class Category
	{
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
		/// Gets or sets the collection of evaluations.
		/// </summary>        
		public ICollection<Evaluation> Evaluations { get; set; }		
	}
}
