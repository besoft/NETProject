using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// In-memory repository of students
	/// </summary>
	public class StudentRepository : IStudentRepository
	{
		private StudentEvaluationContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentRepository"/> class.
		/// </summary>
		/// <param name="context">The context of this repository.</param>
		public StudentRepository(StudentEvaluationContext context)
		{
			this._context = context;
		}

		/// <summary>
		/// Gets the collection of all students.
		/// </summary>
		/// <returns>
		/// A collection of students in the repository.
		/// </returns>
		public IEnumerable<Student> GetStudents()
		{
			return this._context.Students;//.ToList();
		}

		/// <summary>
		/// Gets the student identified by personal number.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		/// <returns>
		/// null, if the student does not exist, otherwise valid object.
		/// </returns>
		public Student GetStudentByPersonalNumber(string personalNumber)
		{
			return this._context.Students.Where(x => x.PersonalNumber == personalNumber).SingleOrDefault();
		}

		/// <summary>
		/// Inserts a new student into the repository.
		/// </summary>
		/// <param name="student">The student to be inserted.</param>
		public void InsertStudent(Student student)
		{			
			this._context.Students.Add(student);
		}

		/// <summary>
		/// Deletes the student from the repository.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		public void DeleteStudent(string personalNumber)
		{
			var student = GetStudentByPersonalNumber(personalNumber);
			if (student != null)
				this._context.Students.Remove(student);
		}

		/// <summary>
		/// Deletes the student from the repository.
		/// </summary>
		/// <param name="student">The student to be deleted.</param>
		public void DeleteStudent(Student student)
		{
			this._context.Students.Remove(student);
		}

		/// <summary>
		/// Updates the student data in the repository.
		/// </summary>
		/// <param name="student">The new student data.</param>
		public void UpdateStudent(Student student)
		{
			//nothing to do			
		}

		/// <summary>
		/// Saves all changes.
		/// </summary>
		public void Save()
		{
			//nothing to do
		}
	}
}
