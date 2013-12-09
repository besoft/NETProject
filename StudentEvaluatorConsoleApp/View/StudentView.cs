using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.RegularExpressions;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.ViewModel;

namespace Zcu.StudentEvaluator.View
{
	public class StudentView : WindowView
	{				
		/// <summary>
		/// Displays the specified student.
		/// </summary>
		/// <param name="student">The student whose details are to be displayed.</param>
		/// <param name="displayForEdit">if set to <c>false</c> the student data is displayed for read-only.</param>
		protected void Display(IStudentViewModel student, bool displayForEdit = true)
		{
            Contract.Requires(student != null);

            Console.WriteLine("---------------------------------");
			Console.WriteLine("{1}ersonalNumber:\t{0}", student.PersonalNumber, displayForEdit ? "(P)" : "P");
			if (displayForEdit) DisplayValidationError("PersonalNumber");

			Console.WriteLine("{1}irstName:\t{0}", student.FirstName, displayForEdit ? "(F)" : "F");
			if (displayForEdit) DisplayValidationError("FirstName");

			Console.WriteLine("{1}urname:\t{0}", student.Surname, displayForEdit ? "(S)" : "S");
			if (displayForEdit) DisplayValidationError("Surname");

			Console.WriteLine("---------------------------------");
			if (displayForEdit) DisplayValidationError();
		}

		/// <summary>
		/// Displays the content of the window onto the console.
		/// </summary>		
		protected override void DisplayContent()
		{
			var viewModel = this.DataContext as IStudentViewModel;
			Console.WriteLine(viewModel.DisplayName);
			Display(viewModel, !viewModel.IsReadOnly);
			Console.WriteLine();
		}

		/// <summary>
		/// Displays the command menu, gets the next command from the user and processes it
		/// </summary>
		protected override void DispatchCommand()
		{
			var errorInfo = this.DataContext as IDataErrorInfo;
			var viewModel = this.DataContext as IViewModel;
			var studentViewModel = this.DataContext as IStudentViewModel;
			var viewModelCommands = this.DataContext as IEditableViewModel;

			var sb = new StringBuilder();
			if (viewModel.IsReadOnly)
			{
				//change vs. cancel
				if (viewModelCommands.EditCommand.CanExecute(null))
					sb.Append("(E)dit, ");				
			}
			else
			{
				sb.Append("Edit (P)(F)(S), ");
				if (viewModelCommands.SaveCommand.CanExecute(null))
					sb.Append("(O)K, ");
			}

			sb.Append("(C)ancel");

			switch (GetNextCommand(sb.ToString()))
			{
				case 'E':
					viewModelCommands.EditCommand.Execute(null);
					break;
				case 'P':
					studentViewModel.PersonalNumber = GetPersonalNumber();
					break;
				case 'F':
					studentViewModel.FirstName = GetFirstName();
					break;
				case 'S':
					studentViewModel.Surname = GetSurname();
					break;
				case 'O':
					viewModelCommands.SaveCommand.Execute(null);
					if (!viewModel.IsModelDirty)
						this.DialogResult = true;
					break;
				case 'C':
					viewModelCommands.CancelCommand.Execute(null);
					this.DialogResult = false;
					break;
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
	}
}
