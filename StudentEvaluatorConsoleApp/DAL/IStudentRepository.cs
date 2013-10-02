using System;
using System.Collections.Generic;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Repository for students
	/// </summary>
	public interface IStudentRepository
	{
		/// <summary>
		/// Gets the collection of all students.
		/// </summary>
		/// <returns>A collection of students in the repository.</returns>
		IEnumerable<Student> GetStudents();

		/// <summary>
		/// Gets the student identified by personal number.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		/// <returns>null, if the student does not exist, otherwise valid object.</returns>
		Student GetStudentByPersonalNumber(string personalNumber);

		/// <summary>
		/// Inserts a new student into the repository.
		/// </summary>
		/// <param name="student">The student to be inserted.</param>
		void InsertStudent(Student student);

		/// <summary>
		/// Deletes the student from the repository.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		void DeleteStudent(string personalNumber);

		/// <summary>
		/// Deletes the student from the repository.
		/// </summary>
		/// <param name="student">The student to be deleted.</param>
		void DeleteStudent(Student student);

		/// <summary>
		/// Updates the student data in the repository.
		/// </summary>
		/// <param name="student">The new student data.</param>
		void UpdateStudent(Student student);

		/// <summary>
		/// Saves all changes.
		/// </summary>
		void Save();
	}
}
