using System;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents a ViewModel that can be selected and/or focused in Views
	/// </summary>
	public interface ISelectableViewModel : IViewModelBase
	{
		/// <summary>
		/// Gets/sets whether this item is focused in the UI.
		/// </summary>
		bool IsFocused { get; set; }

		/// <summary>
		/// Gets/sets whether this item is selected in the UI.
		/// </summary>
		bool IsSelected { get; set; }
	}
}
