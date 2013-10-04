using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// ViewModel / Controller for manipulation with students.
	/// </summary>
	public interface IStudentViewModel
	{		
		/// <summary>
		/// Displays the list of students.
		/// </summary>
		void DisplayList();

		/// <summary>
		/// Create a new student.
		/// </summary>
		void Create();

		/// <summary>
		/// Display the detail of a student.
		/// </summary>
		/// <param name="personalNumber">The personal number of student.</param>
		void DisplayDetail(string personalNumber);

		/// <summary>
		/// Edits the detail.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		void EditDetail(string personalNumber);

		/// <summary>
		/// Accepts the changes done to the currently edited student (must be called after Create or EditDetail).
		/// </summary>
		void AcceptChanges();

		/// <summary>
		/// Cancel the changes done to the currently edited student (must be called after Create or EditDetail).
		/// </summary>
		void CancelChanges();

		/// <summary>
		/// Deletes the specified personal number.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		void Delete(string personalNumber);

		/// <summary>
		/// Exit the application.
		/// </summary>
		void Exit();
	}
}
