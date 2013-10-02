using System;
using System.Collections.Generic;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.View
{
	/// <summary>
	/// Represents the main view of students.
	/// </summary>
	public interface IStudentView : IDisposable
	{
		/// <summary>
		/// Displays the list of students.
		/// </summary>
		/// <param name="students">The list of students.</param>
		void Display(IEnumerable<Student> students);

		/// <summary>
		/// Displays the specified student.
		/// </summary>
		/// <param name="student">The student whose details are to be displayed.</param>
		/// <param name="displayForEdit">if set to <c>false</c> the student data is displayed for read-only.</param>
		void Display(Student student, bool displayForEdit = true);

		/// <summary>
		/// Closes the view.
		/// </summary>
		void Close();
	}
}
