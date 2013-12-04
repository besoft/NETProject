using System;
using System.Windows.Input;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents ViewModel that supports CRUD commands.
	/// </summary>
	public interface IEditableViewModel : IViewModelBase
	{
		/// <summary>
		/// Gets the Create command.
		/// </summary>				
		/// <remarks>Supports creating a new model. The model is set to be the current one and marked as being edited.</remarks>
		ICommand CreateCommand { get; }
		
		/// <summary>
		/// Gets the edit command.
		/// </summary>		
		/// <remarks>Supports editing the current model. The changes are typically not stored until SaveCommand is executed.</remarks>
		ICommand EditCommand { get; }

		/// <summary>
		/// Gets the save command.
		/// </summary>
		/// <remarks>Supports saving the changes done to the current model into the repository.</remarks>
		ICommand SaveCommand { get; }

		/// <summary>
		/// Gets the cancel command.
		/// </summary>
		/// <remarks>Supports cancelling the changes done to the current model.</remarks>
		ICommand CancelCommand { get; }

		/// <summary>
		/// Gets the delete command.
		/// </summary>
		/// <remarks>Supports deleting the current model from the repository.</remarks>
		ICommand DeleteCommand { get; }
	}
}
