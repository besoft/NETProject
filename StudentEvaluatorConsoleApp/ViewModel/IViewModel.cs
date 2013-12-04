using System;
using System.ComponentModel;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents a generic ViewModel
	/// </summary>
	public interface IViewModel : IViewModelBase
	{
		/// <summary>
		/// Gets the model unique identifier.
		/// </summary>		
		int Id { get; }		

		/// <summary>
		/// Gets a value indicating whether this object is marked for deletion.
		/// </summary>
		/// <remarks>Any operation with the ViewModel in this state is ignored.</remarks>
		/// <value>
		///   <c>true</c> if the object is marked to be deleted; otherwise, <c>false</c>.
		/// </value>
		bool IsModelDeleted { get; }

		/// <summary>
		/// Gets a value indicating whether any property of the underlying model has changed since the last save operation.
		/// </summary>
		/// <value>
		///   <c>true</c> if any model property has changed; otherwise, <c>false</c>.
		/// </value>
		bool IsModelDirty { get; }

		/// <summary>
		/// Gets a value indicating whether the underlying model is new.
		/// </summary>
		/// <remarks>A new model is such an object that has never been stored into the repository.</remarks>
		/// <value>
		///   <c>true</c> if the underlying model is new; otherwise, <c>false</c>.
		/// </value>
		bool IsModelNew { get; }

		/// <summary>
		/// Gets a value indicating whether the model is valid.
		/// </summary>
		/// <value>
		///   <c>true</c> if the model is valid; otherwise, <c>false</c>.
		/// </value>
		bool IsModelValid { get; }		
	}
}
