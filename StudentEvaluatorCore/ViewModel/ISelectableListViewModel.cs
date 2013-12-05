using System;
using System.Collections.ObjectModel;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents a ViewModel that can be selected and/or focused in Views
	/// </summary>
	public interface ISelectableListViewModel<T> : IListViewModel<T>
	{
		/// <summary>
		/// Gets the selected items in the collection (list).
		/// </summary>
		ObservableCollection<T> SelectedItems { get; }

		/// <summary>
		/// Gets the currently focused item in the collection (list).
		/// </summary>
		T FocusedItem { get; }
	}
}
