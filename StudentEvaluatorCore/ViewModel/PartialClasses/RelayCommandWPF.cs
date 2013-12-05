using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Zcu.StudentEvaluator.ViewModel
{
	partial class RelayCommand<T> 
	{
		/// <summary>
		/// Occurs when changes occur that affect whether the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			//subscribe the caller to WPF command base so that WPF handles everything
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		/// <summary>
		/// Raises the <see cref="CanExecuteChanged" /> event.
		/// </summary>		
		public void RaiseCanExecuteChanged()
		{			
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
