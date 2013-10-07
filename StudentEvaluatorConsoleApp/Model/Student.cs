using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;

namespace Zcu.StudentEvaluator.Model
{
	/// <summary>
	/// This structure keeps the number of points given and the reason for it.
	/// </summary>
	public class Student : IEntity
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the personal number of the student.
		/// </summary>
		/// <value>
		/// The personal number, e.g. A12B0012P.
		/// </value>		
		[Required]
		[MaxLength(10)]
		[RegularExpression(@"[A-Z]\d{2}[BN]\d+[PK]")]		
		public string PersonalNumber { get; set; }		///NOTE: Entity Framework does not support [Unique] attribute => this must be done in migrations

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name, e.g., "Josef".
		/// </value>
		[Required]
		[MaxLength(25)]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the surname.
		/// </summary>
		/// <value>
		/// The surname, e.g., "Kohout".
		/// </value>
		[Required]
		[MaxLength(25)]
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
				return Surname.ToUpper() + " " + FirstName;				
			}
		}

		/// <summary>
		/// Gets or sets the individual student evaluation.
		/// </summary>		
		public virtual ICollection<Evaluation> Evaluations { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Student"/> class.
		/// </summary>
		public Student()
		{
			this.Evaluations = new List<Evaluation>();
		}
	}
}
