using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.ViewModel;
using System.Diagnostics.Contracts;

namespace Zcu.StudentEvaluator.View
{
	public class StudentListView : WindowView
	{		
		/// <summary>
		/// Displays the list of students.
		/// </summary>
		/// <param name="students">The list of students.</param>
		public void Display(IEnumerable<IStudentListItemViewModel> students)
		{
            Contract.Requires(students != null);

			Console.WriteLine("---------------------------------");
			foreach (var st in students)
			{
				var oldBkColor = Console.BackgroundColor;
				var oldFgColor = Console.ForegroundColor;

				if (st.IsSelected)
					Console.BackgroundColor = ConsoleColor.DarkBlue;

				if (st.IsFocused)
					Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine("{0}, {1} {2} - evaluations: {3}",
					st.PersonalNumber, st.Surname, st.FirstName, st.Evaluations.Count);

				Console.BackgroundColor = oldBkColor;
				Console.ForegroundColor = oldFgColor;
			}
			Console.WriteLine("---------------------------------");
		}		

		/// <summary>
		/// Displays the content of the window onto the console.
		/// </summary>		
		protected override void DisplayContent()
		{
			var studentListViewModel = this.DataContext as IStudentListViewModel;
			Console.WriteLine(studentListViewModel.DisplayName);
			Display(studentListViewModel.Items);
			Console.WriteLine("Total students: " + studentListViewModel.AllStudentsCount);
			Console.WriteLine();
		}

		/// <summary>
		/// Displays the command menu, gets the next command from the user and processes it
		/// </summary>
		protected override void DispatchCommand()
		{
			var studentListViewModel = this.DataContext as IStudentListViewModel;
			var sb = new StringBuilder();

			if (studentListViewModel.Items.Count != 0)
				sb.Append("(S)elect, ");

			if (studentListViewModel.CreateCommand.CanExecute(null))
				sb.Append("(C)reate, ");

			if (studentListViewModel.EditCommand.CanExecute(null))
				sb.Append("(E)dit, ");

			if (studentListViewModel.DeleteCommand.CanExecute(null))
				sb.Append("(D)elete, ");

			if (studentListViewModel.RefreshListCommand.CanExecute(null))
				sb.Append("(R)efresh, ");

			sb.Append("E(x)it");

			switch (GetNextCommand(sb.ToString()))
			{
				case 'S':
					{
						var pn = GetValue("personal number");
						var item = studentListViewModel.Items.FirstOrDefault(x => x.PersonalNumber == pn);
						if (item != null)
						{
							//we support single selection only
							foreach (var it in studentListViewModel.SelectedItems.ToList())
							{
								it.IsSelected = false;
							}

							item.IsFocused = true;
							item.IsSelected = true;
						}						
					}
					break;

				case 'C':
					studentListViewModel.CreateCommand.Execute(null);
					break;
				case 'R':
					studentListViewModel.RefreshListCommand.Execute(null);
					break;
				case 'E':
					studentListViewModel.EditCommand.Execute(null);
					break;
				case 'D':
					studentListViewModel.DeleteCommand.Execute(null);
					break;
				case 'X':
					this.Close();
					break;
			}			
		}
	}
}
