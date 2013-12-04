using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zcu.StudentEvaluator.View
{
	/// <summary>
	/// Abstract class for Console Windows
	/// </summary>
	public abstract class WindowView : IWindowView, IDisposable
	{
		#region Fields
		protected bool? _dialogResult = null;	//dialog result
		protected bool _closed = false;			//denotes, if the Window is Closed
		#endregion

		/// <summary>
		/// Gets or sets the data context.
		/// </summary>
		/// <value>
		/// The data context implementing IViewModel and other interfaces.
		/// </value>
		public object DataContext { get; set; }

		/// <summary>
		/// Opens a window and returns without waiting for the newly opened window to close.
		/// </summary>
		public void Show()
		{
			DisplayContent();
		}

		/// <summary>
		/// Manually closes a Window.
		/// </summary>		
		public void Close()
		{
			_closed = true;
		}

		/// <summary>
		/// Gets or sets the dialog result value, which is the value that is returned from the ShowDialog method.
		/// </summary>
		/// <value>
		/// A Nullable<T> value of type Boolean. The default is false.
		/// </value>
		public bool? DialogResult
		{
			get
			{
				return _dialogResult;
			}
			set
			{
				if (this._dialogResult != value)
				{
					this._dialogResult = value;
					if (!this._closed)
					{
						this.Close();
						return;
					}
				}	
			}
		}

		/// <summary>
		/// Opens a window and returns only when the newly opened window is closed.
		/// </summary>
		/// <returns>A Nullable<T> value of type Boolean that specifies whether the activity was accepted (true) or cancelled (false). 
		/// The return value is the value of the DialogResult property before a window closes.</returns>
		public bool? ShowDialog()
		{
			while (!this._closed)
			{
				DisplayContent();			
				DispatchCommand();
			}

			return _dialogResult;
		}

		/// <summary>
		/// Displays the content of the window onto the console.
		/// </summary>
		protected abstract void DisplayContent();

		/// <summary>
		/// Displays the command menu, gets the next command from the user and processes it
		/// </summary>
		protected abstract void DispatchCommand();

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>		
		public void Dispose()
		{
			this.Close();
		}

		/// <summary>
		/// Gets the next command.
		/// </summary>
		/// <param name="menuCommandString">The menu command string.</param>
		/// <remarks>Displays menuCommandString and waits for user valid response. Valid keys are given in menuCommandString
		/// in brackets, e.g., "E(x)it". The method is not case sensitive, i.e., pressing 'x' and 'X' triggers the same command.</remarks>
		/// <returns>Capitalized letter of the command, e.g., 'X'</returns>
		protected char GetNextCommand(string menuCommandString)
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

		/// <summary>
		/// Gets the value for the variable from the user.
		/// </summary>
		/// <param name="variable">The variable name.</param>
		/// <param name="canBeNull">if set to <c>true</c>, the user may enter null value, otherwise they must specify valid value.</param>
		/// <returns>The value</returns>		
		protected string GetValue(string variable, bool canBeNull = false)
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
		/// Displays the validation error (if available).
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>		
		protected void DisplayValidationError(string propertyName)
		{
			IDataErrorInfo errorInfo = this.DataContext as IDataErrorInfo;
			if (errorInfo == null)
				return;

			var errorMsg = errorInfo[propertyName];
			if (errorMsg != String.Empty)
			{
				var oldColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("* " + errorMsg);
				Console.ForegroundColor = oldColor;
			}
		}

		/// <summary>
		/// Displays the validation error (if available).
		/// </summary>		
		protected void DisplayValidationError()
		{
			IDataErrorInfo errorInfo = this.DataContext as IDataErrorInfo;
			if (errorInfo == null)
				return;

			var errorMsg = errorInfo.Error;
			if (errorMsg != String.Empty)
			{
				var oldColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("* " + errorMsg);
				Console.ForegroundColor = oldColor;
			}
		}
	}
}
