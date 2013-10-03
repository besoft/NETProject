using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.View.ConsoleApp;
using Zcu.StudentEvaluator.ViewModel;

namespace Zcu.StudentEvaluator.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			//Pokus();

			var unitOfWork = new LocalStudentEvaluationUnitOfWork(new XmlStudentEvaluationContext());
			if (unitOfWork.Categories.Get().FirstOrDefault() == null)
			{
				unitOfWork.PopulateWithData();
			}

			var mainView = new StudentView(unitOfWork);
			mainView.ShowDialog();
		}

		/*
		private static void Pokus()
		{			
			int nextId = 0;
			Student[] students = new Student[30];
			for (int i = 0; i < students.Length; i++)						
			{
				var student = new Student();
				student.Id = ++nextId;				
				student.Surname = (student.Id % 2 == 0) ? "A" : "B";
				student.FirstName = (student.Id % 3 == 0) ? "X" : "Y";
				students[i] = student;
			}

			var col = GetStudents2(students, x => x.Id % 2 == 0, x => x.OrderBy(s => s.FullName).ThenByDescending(s => s.Id),"1", "2", "3");
			foreach (var item in col)
			{
				Console.WriteLine("{0}\t{1}", item.FullName, item.Id);
			}

			Console.ReadLine();
		}
		

		private static IEnumerable<Student> GetStudents2(IEnumerable<Student> students, Expression<Func<Student, bool>> filter,
			Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy = null, params string[] includes)
		{
			foreach (var item in includes)
			{
				Console.WriteLine("Include({0}).", item);
			}		

			return orderBy(students.AsQueryable<Student>().Where(filter));
		}
		 * */
	}
}
