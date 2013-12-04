using System;
using System.Data.Entity.Validation;
using System.Linq;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.View;
using Zcu.StudentEvaluator.ViewModel;

namespace Zcu.StudentEvaluator.ConsoleApp
{
	public class BootStraper
	{
		public static void InitializeIOC()
		{
			DialogService.DialogService.Default.Register<INotificationView, NotificationView>(DialogService.DialogConstants.NotificationView);
			DialogService.DialogService.Default.Register<IConfirmationView, ConfirmationView>(DialogService.DialogConstants.ConfirmationView);
			DialogService.DialogService.Default.Register<IWindowView, StudentView>(DialogService.DialogConstants.EditStudentView);
			
			DialogService.DialogService.Default.Register<IWindowView, StudentListView>();	//main View
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			BootStraper.InitializeIOC();

			//Pokus();			
			var unitOfWork = 
				//new LocalStudentEvaluationUnitOfWork();
				new DbStudentEvaluationUnitOfWork();
			if (unitOfWork.Categories.Get().FirstOrDefault() == null)
			{
				try
				{
					unitOfWork.PopulateWithData();
				}
				catch (DbEntityValidationException excValidation)
				{
					foreach (var item in excValidation.EntityValidationErrors)
					{
						Console.WriteLine("Validation of '{0}' failed with these errors:", item.Entry.Entity.GetType().Name);
						foreach (var err in item.ValidationErrors)
						{
							Console.WriteLine("For '{0}' : {1}", err.PropertyName, err.ErrorMessage);
						}
					}
				}
			}

			var mainViewModel = new StudentListViewModel(unitOfWork);

			var mainView = new StudentListView();
			mainView.DataContext = mainViewModel;

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
