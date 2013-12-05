using System;

namespace Zcu.StudentEvaluator.ViewModel
{
	partial class RelayCommand<T> 
	{
		/// <summary>
		/// Occurs when changes occur that affect whether the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Raises the <see cref="CanExecuteChanged" /> event.
		/// </summary>		
		public void RaiseCanExecuteChanged()
		{			
			if (CanExecuteChanged != null) {
				CanExecuteChanged(this, EventArgs.Empty);
			}		
		}
	}
}
