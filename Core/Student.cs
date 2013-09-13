using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Text;
using Zcu.StudentEvaluator.Core.Collection;

namespace Zcu.StudentEvaluator.Core.Data
{
	/// <summary>
	/// This structure keeps the number of points given and the reason for it.
	/// </summary>
	public class Student
	{
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the personal number of the student.
		/// </summary>
		/// <value>
		/// The personal number, e.g. A12B0012P.
		/// </value>
		public string PersonalNumber { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name, e.g., "Josef".
		/// </value>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the surname.
		/// </summary>
		/// <value>
		/// The surname, e.g., "Kohout".
		/// </value>
		public string Surname { get; set; }

		/// <summary>
		/// Gets the full name of the student.
		/// </summary>
		/// <value>
		/// The full name.
		/// </value>
		public string FullName {
			get
			{
				//TODO: odkomentovat puvodni kod a ukazat moznosti Diggeru
				//return Surname.ToUpper() + " " + FirstName;
				if (this.Surname != null)
				{
					return (this.FirstName != null) ? Surname.ToUpper() + " " + FirstName : Surname.ToUpper();
				}
				else if (this.FirstName != null)
				{
					return FirstName;
				}
				else 
					return null;               
			}
		}

		/// <summary>
		/// Gets the individual student evaluation.
		/// </summary>
		/// <remarks>This collection is private to each student.</remarks>        
		public ObservableCollection<Evaluation> Evaluations { get; private set; }
	   
		/// <summary>
		/// Gets the total points the student obtained.
		/// </summary>
		/// <value>
		/// The total points.
		/// </value>
		public decimal? TotalPoints
		{
			get
			{
				decimal? sum = null;
				if (this.Evaluations != null)
				{
					foreach (var item in this.Evaluations)
					{
						//TODO: zakomentovat tento test a ukazat moznosti Diggeru
						if (item == null)
							continue;

						if (sum == null)
							sum = item.ValidPoints;
						else
							sum += item.ValidPoints ?? 0;
					}
				}

				return sum;
			}
		}

		/// <summary>
		/// Gets the reason for the total points given
		/// </summary>
		/// <value>
		/// The evaluation details.
		/// </value>
		public string TotalPointsReason { 
			get
			{
				Contract.Ensures(Contract.Result<string>() != null);

				var sb = new StringBuilder();
				if (this.Evaluations != null)
				{
					foreach (var item in this.Evaluations)
					{
						//TODO: zakomentovat tento test a ukazat moznosti Diggeru
						if (item == null)
							continue;

						sb.AppendFormat("{0}, ", item);
					}
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Student" /> class.
		/// </summary>
		public Student()
		{
			this.Evaluations = new EvaluationObservableCollectionWithParentStudent(this);                   
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
			Contract.Requires<ArgumentException>(evaluation.Student == null || evaluation.Student == this,
				"Cannot add an evaluation assigned already to another student");
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
			Contract.Requires<ArgumentException>(evaluation.Student == null || evaluation.Student == this,
				"Cannot remove an evaluation assigned to another student");
				
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
			return string.Format("{0}\t{1}\t{2}\n\t{3}",
				this.PersonalNumber, this.FullName, this.TotalPoints, this.TotalPointsReason);
		}
	}
}
