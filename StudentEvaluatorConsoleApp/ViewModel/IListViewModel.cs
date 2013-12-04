using System;
using System.Collections.ObjectModel;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents a ViewModel that is a list (collection) of ViewModel items.
	/// </summary>
	public interface IListViewModel<T> : IViewModelBase
	{
		/// <summary>
		/// Gets the items in the collection (list).
		/// </summary>
		ObservableCollection<T> Items { get; }
	}
}
