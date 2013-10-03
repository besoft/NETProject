namespace Zcu.StudentEvaluator.Model
{
	/// <summary>
	/// Evaluation object describes one particular evaluation in one evaluation parent (Category) for one student (Student)
	/// </summary>
	public class Evaluation : IEntity
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		public int Id { get; set; }

		/// <summary>
		/// Gets the number of points.
		/// </summary>
		/// <value>
		/// The number of points.
		/// </value>
		public decimal? Points { get; set; }

		/// <summary>
		/// Gets the reason for the points given.
		/// </summary>
		/// <value>
		/// The reason for the points give, e.g. "the solution lacks OO design".
		/// </value>
		public string Reason { get; set; }

		/// <summary>
		/// Gets or sets the evaluation parent.
		/// </summary>
		/// <value>
		/// The definition.
		/// </value>
		public Category Category {get; set; }
		
		/// <summary>
		/// Gets or sets the student to whom this evaluation belongs.
		/// </summary>
		/// <value>
		/// The student.
		/// </value>
		public Student Student {get; set; }			
	}
}