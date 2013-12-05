using System;
using System.ComponentModel;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents a very simple generic ViewModel
	/// </summary>
	public interface IViewModelBase
	{		
		/// <summary>
		/// Returns the user-friendly name of this object.
		/// Child classes can set this property to a new value,
		/// or override it to determine the value on-demand.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Gets a value indicating whether the ViewModel is read only.
		/// </summary>
		/// <value>
		///   <c>true</c> if the ViewModel is read only; otherwise, <c>false</c>.
		/// </value>		
		bool IsReadOnly { get; }
	}
}
