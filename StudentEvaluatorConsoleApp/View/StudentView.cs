using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.ViewModel;

namespace Zcu.StudentEvaluator.View.ConsoleApp
{
	public class StudentView : IStudentView, IConfirmationView, INotificationView
	{
		protected IStudentViewModel _viewModel;
		protected bool _closed = false;

		protected bool _stateInSummary = true;
		protected Student _currentStudent = null;		

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentView"/> class.
		/// </summary>		
		public StudentView(IStudentEvaluationUnitOfWork unitOfWork)
		{
			this._viewModel = new StudentViewModel(this, this, this, unitOfWork);
		}

		#region INotificationView		
		/// <summary>
		/// Displays the notification message to the user.
		/// </summary>
		/// <param name="type">The type of the notification.</param>
		/// <param name="caption">The caption of the message, i.e., this is a short summary of what has happened.</param>
		/// <param name="message">The message to be displayed containing the detailed explanation of what has happened.</param>
		/// <param name="exc">The exception containing all the details (may be null).</param>
		public void DisplayNotification(NotificationType type, string caption, string message, Exception exc = null)
		{			
			var oldColor = Console.ForegroundColor;
			switch (type)
			{
				case NotificationType.Message: 
					Console.ForegroundColor = ConsoleColor.DarkGreen; break;					
				case NotificationType.Warning: 
					Console.ForegroundColor = ConsoleColor.DarkYellow; break;
				default: 
					Console.ForegroundColor = ConsoleColor.Red; break;			
			}

			Console.WriteLine("{0} : {1}\n\n{2}", type.ToString().ToUpper(), caption, message);

			if (exc != null)
			{
				Console.WriteLine("\nException:");
				Console.WriteLine(exc.ToString());
			}

			Console.WriteLine();
			Console.ForegroundColor = oldColor;
		}
		#endregion

		#region IConfirmationView
		/// <summary>
		/// Confirms the action to be done.
		/// </summary>
		/// <param name="options">Options available during the confirmation.</param>
		/// <param name="caption">The caption, i.e., a short summary of what is needed to be confirmed.</param>
		/// <param name="message">The detailed explanation of what is to be confirmed.</param>
		/// <returns>
		/// User decision.
		/// </returns>
		public ConfirmationResult ConfirmAction(ConfirmationOptions options, string caption, string message)
		{
			Console.WriteLine("----->\n{0}\n\n{1}\n", caption, message);
			while (true)
			{
				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Abort))
					Console.Write("(A)bort ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Retry))
					Console.Write("(R)etry ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Ignore))
					Console.Write("(I)gnore ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.OK))
					Console.Write("(O)K ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Yes))
					Console.Write("(Y)es ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.No))
					Console.Write("(N)o ");

				if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Cancel))
					Console.Write("(C)ancel ");

				Console.WriteLine();
				switch (Char.ToUpper(Console.ReadKey(true).KeyChar))
				{
					case 'A':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Abort))
							return ConfirmationResult.Abort;
						break;

					case 'R':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Retry))
							return ConfirmationResult.Retry;
						break;

					case 'I':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Ignore))
							return ConfirmationResult.Ignore;
						break;

					case 'O':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.OK))
							return ConfirmationResult.OK;
						break;

					case 'Y':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Yes))
							return ConfirmationResult.Yes;
						break;

					case 'N':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.No))
							return ConfirmationResult.No;
						break;

					case 'C':
						if (options.HasFlag((ConfirmationOptions)ConfirmationResult.Cancel))
							return ConfirmationResult.Cancel;
						break;
				}
			}
		}
		#endregion		

		#region IStudentView
		/// <summary>
		/// Displays the list of students.
		/// </summary>
		/// <param name="students">The list of students.</param>
		public void Display(IEnumerable<Student> students)
		{
			Console.WriteLine("---------------------------------");
			foreach (var st in students)
			{
				Console.WriteLine("{0}, {1} - evaluations: {2}",
					st.PersonalNumber, st.FullName, st.Evaluations.Count);
			}
			Console.WriteLine("---------------------------------");

			_currentStudent = null;
			_stateInSummary = true;
		}

		/// <summary>
		/// Displays the specified student.
		/// </summary>
		/// <param name="student">The student whose details are to be displayed.</param>
		/// <param name="displayForEdit">if set to <c>false</c> the student data is displayed for read-only.</param>
		public void Display(Student student, bool displayForEdit = true)
		{
			Console.WriteLine("---------------------------------");
			Console.WriteLine("{1}ersonalNumber:\t{0}", student.PersonalNumber, displayForEdit ? "(P)" : "P");
			Console.WriteLine("{1}irstName:\t{0}", student.FirstName, displayForEdit ? "(F)" : "F");
			Console.WriteLine("{1}urname:\t{0}", student.Surname, displayForEdit ? "(S)" : "S");
			foreach (var item in student.Evaluations)
			{
				Console.WriteLine("---");
				Console.WriteLine("{0}\t{1} b",
					(item.Category != null ? item.Category.Name : ""),
					(item.Points != null ? item.Points.ToString() : "N/A"));

				if (item.Reason != null)
					Console.WriteLine("\t" + item.Reason);
			}
			Console.WriteLine("---------------------------------");

			_currentStudent = student;
			_stateInSummary = !displayForEdit;
		} 
		#endregion
		
		/// <summary>
		/// Closes the view.
		/// </summary>		
		public void Close()
		{
			_closed = true;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>		
		public void Dispose()
		{
			this.Close();	
		}
		

		/// <summary>
		/// Shows the view and starts processing the user interactions.
		/// </summary>
		/// <remarks>Does not returned until the view is closed.</remarks>
		public void ShowDialog()
		{			
			while (!this._closed)
			{
				if (_stateInSummary)
				{
					this._viewModel.DisplayList();
					switch (GetNextCommand("(C)reate, (R)ead, (U)pdate, (D)elete, or E(x)it"))
					{
						case 'C':
							this._viewModel.Create();							
							break;
						case 'R':
							this._viewModel.DisplayDetail(GetPersonalNumber());
							break;
						case 'U':
							this._viewModel.EditDetail(GetPersonalNumber());														
							break;
						case 'D':
							this._viewModel.Delete(GetPersonalNumber());							
							break;						
						case 'X':
							this._viewModel.Exit();
							break;
					}
				}
				else
				{
					switch (GetNextCommand("Edit (P)(F)(S), or (B)ack"))
					{
						case 'P':
							this._currentStudent.PersonalNumber = GetPersonalNumber();
							this._viewModel.EditDetail(this._currentStudent.PersonalNumber);
							break;
						case 'F':
							this._currentStudent.FirstName = GetFirstName();
							this._viewModel.EditDetail(this._currentStudent.PersonalNumber);
							break;
						case 'S':
							this._currentStudent.Surname = GetSurname();
							this._viewModel.EditDetail(this._currentStudent.PersonalNumber);
							break;
						case 'B':
							_stateInSummary = true;
							break;						
					}					
				}				
			}			
		}

		/// <summary>
		/// Gets the surname from the user.
		/// </summary>
		/// <returns>Surname of the person</returns>
		private string GetSurname()
		{
			return GetValue("surname");
		}

		/// <summary>
		/// Gets the first name from the user.
		/// </summary>
		/// <returns>First name of the person</returns>
		private string GetFirstName()
		{
			return GetValue("first name");
		}

		/// <summary>
		/// Gets the personal number from the user.
		/// </summary>
		/// <returns>Personal number</returns>
		private string GetPersonalNumber()
		{
			return GetValue("personal number");
		}

		/// <summary>
		/// Gets the value for the variable from the user.
		/// </summary>
		/// <param name="variable">The variable name.</param>
		/// <param name="canBeNull">if set to <c>true</c>, the user may enter null value, otherwise they must specify valid value.</param>
		/// <returns>The value</returns>		
		private string GetValue(string variable, bool canBeNull = false)
		{
			while (true)
			{
				Console.Write("Enter " + variable + ": ");
				string value = Console.ReadLine();

				if (value != null)
				{
					if ((value = value.Trim()).Length == 0)
						value = null;
				}

				if (value == null && !canBeNull)
					Console.WriteLine("{0} cannot be null.", variable);
				else
					return value;
			}
			
		}

		/// <summary>
		/// Gets the next command.
		/// </summary>
		/// <param name="menuCommandString">The menu command string.</param>
		/// <remarks>Displays menuCommandString and waits for user valid response. Valid keys are given in menuCommandString
		/// in brackets, e.g., "E(x)it". The method is not case sensitive, i.e., pressing 'x' and 'X' triggers the same command.</remarks>
		/// <returns>Capitalized letter of the command, e.g., 'X'</returns>
		private char GetNextCommand(string menuCommandString)
		{
			//parse menuCommandString
			Regex rex = new Regex(@"\((.)\)");
			var matches = rex.Matches(menuCommandString);
			char[] validChars = new char[matches.Count];
			for (int i = 0; i < validChars.Length; i++)
			{
				validChars[i] = Char.ToUpper(matches[i].Groups[1].Value[0]);
			}
						
			while (true)
			{
				Console.WriteLine("MENU: {0}", menuCommandString);
				char pressed = Char.ToUpper(Console.ReadKey(true).KeyChar);

				if (Array.IndexOf<char>(validChars, pressed) >= 0)
					return pressed;	//it exists, so return
			}			
		}
	}
}
