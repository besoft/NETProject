using System;
namespace Zcu.StudentEvaluator.ViewModel
{
	public interface IStudentViewModel : IViewModel
	{						
		/// <summary>
		/// Gets or sets the personal number of the student.
		/// </summary>
		/// <value>
		/// The personal number, e.g. A12B0012P.
		/// </value>	
		string PersonalNumber { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name, e.g., "Josef".
		/// </value>	
		string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the surname.
		/// </summary>
		/// <value>
		/// The surname, e.g., "Kohout".
		/// </value>
		string Surname { get; set; }

		/// <summary>
		/// Gets the full name of the student.
		/// </summary>
		/// <value>
		/// The full name.
		/// </value>
		string FullName { get; }
	}
}
