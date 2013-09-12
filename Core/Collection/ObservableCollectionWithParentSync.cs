using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Zcu.StudentEvaluator.Core.Collection
{
	/// <summary>
	/// This class represents a dynamic collection of items with one or more parent links that are automatically being synchronized (from source to target direction only) with their global collections.
	/// </summary>
	/// <remarks>Each item in the collection contains one or more parent references (accessible via some property of the item class) whose change.
	/// is propagated automatically to the appropriate global collections (declared in concrete classes).	
	/// 
	/// Items in the collection must be unique and not null.
	/// </remarks>
	/// <typeparam name="T">The type of elements in the collection.</typeparam>	
	public abstract class ObservableCollectionWithParentSync<T> : ObservableCollection<T>
		where T : INotifyPropertyChanged
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentSync{T}"/> class.
		/// </summary>
		public ObservableCollectionWithParentSync() : base()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentSync{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection from which items are copied.</param>
		public ObservableCollectionWithParentSync(IEnumerable<T> collection) : base(collection)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentSync{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection from which items are copied.</param>
		public ObservableCollectionWithParentSync(List<T> collection)
			: base(collection)
		{

		}

		/// <summary>
		/// Inserts the item into the collection.
		/// </summary>
		/// <param name="index">The index at which the item is to insert.</param>
		/// <param name="item">The item to insert.</param>
		protected override void InsertItem(int index, T item)
		{
			Contract.Assume(index >= 0);
			Contract.Assume(index < this.Count);

			this.CheckReentrancy();
			this.CheckArgument(item);

			base.InsertItem(index, item);

			//register ourselves to detect every change of the property of the item
			item.PropertyChanged += OnParentReferencePropertyChanged;
			this.AddParentItems(item);
		}


		/// <summary>
		/// Sets the new item at the specific index.
		/// </summary>
		/// <param name="index">The index at which to modify the item.</param>
		/// <param name="item">The new item.</param>
		protected override void SetItem(int index, T item)
		{
			Contract.Assume(index >= 0);
			Contract.Assume(index < this.Count);
			Contract.Assume(base[index] != null);

			this.CheckReentrancy();
			this.CheckArgument(item);

			//remove parents of the old item and add parents of the new item
			T oldItem = base[index];
			Contract.Assume(oldItem != null);

			base.SetItem(index, item);

			//register ourselves to detect every change of the property of the item
			item.PropertyChanged += OnParentReferencePropertyChanged;

			this.AddParentItems(item);
			this.RemoveParentItems(oldItem);				
		}

		/// <summary>
		/// Removes the item at the specific index.
		/// </summary>
		/// <param name="index">The index at which to remove the item.</param>
		protected override void RemoveItem(int index)
		{
			Contract.Assume(index >= 0);
			Contract.Assume(index < this.Count);

			this.CheckReentrancy();

			//register ourselves to detect every change of the property of the item
			T oldItem = base[index];
			Contract.Assume(oldItem != null);

			oldItem.PropertyChanged -= OnParentReferencePropertyChanged;

			base.RemoveItem(index);

			RemoveParentItems(oldItem);
		}

		/// <summary>
		/// Clears the items.
		/// </summary>
		protected override void ClearItems()
		{
			while (this.Count > 0)
			{
				this.RemoveAt(this.Count - 1);
			}
		}

		/// <summary>
		/// Checks, if the input argument is valid.
		/// </summary>
		/// <remarks>The input argument is valid only and only if it is not null and it is not already a member of the internal
		/// this.Collection collection. If the argument is invalid, the method throws an exception.</remarks>
		/// <param name="item">The input argument to check.</param>
		/// <exception cref="System.ArgumentNullException">item is null </exception>
		/// <exception cref="System.ArgumentException">item is already </exception>
		protected virtual void CheckArgument(T item)
		{			
			Contract.Ensures(item != null);
			Contract.Ensures(this.Contains(item) == false);

			if (item == null)
				throw new ArgumentNullException();

			if (this.Contains(item))
				throw new ArgumentException("item is already in the collection");
		}

		/// <summary>
		/// Adds the parents of the specified item into the global collections.
		/// </summary>
		/// <param name="item">The item.</param>
		protected virtual void AddParentItems(T item)
		{
			Contract.Requires(item != null);

			throw new NotImplementedException();
		}


		/// <summary>
		/// Removes the parents of the specified item from the global collection.
		/// </summary>
		/// <param name="oldItem">The old item.</param>
		protected virtual void RemoveParentItems(T oldItem)
		{
			Contract.Requires(oldItem != null);

			throw new NotImplementedException();
		}

		/// <summary>
		/// Called when some property of an item referencing to the parent item has changed.
		/// </summary>
		/// <remarks>Concrete classes are supposed to call this.Remove, if the item has no valid parent, and add  new parents into global collections.</remarks>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnParentReferencePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Contract.Assume(sender != null);
			Contract.Assume(e != null && e.PropertyName != null);			
			
			throw new NotImplementedException();
		}
	}
}
