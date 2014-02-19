using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Zcu.StudentEvaluator.ViewModel
{
    /// <summary>
    /// This namespace contains classes/interfaces implementing ViewModels.
    /// </summary>       
    /// <remarks>A ViewModel represents the logic related to its underlying Model (data) 
    /// as well as the logic related to the visual presentation of the data to the user (via Views).    
    /// </remarks>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
        //Trick to document a namespace
    }

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
