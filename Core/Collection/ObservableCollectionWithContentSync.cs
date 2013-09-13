using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace Zcu.StudentEvaluator.Core.Collection
{
	/// <summary>
	/// This class represents a dynamic collection of items with subitems automatically being synchronized (from source to target direction only) with the global collection.
	/// </summary>
	/// <remarks>Each item in the collection consists of one or more subitems stored in an internal collection accessible via some property of the item class.
	/// Any change in the collection (no matter if on the item level or subitem level) is propagated to the global collection of subitem elements in
	/// order to ensure that the global collection is in fact an union of all collections of items. N.B. synchronization is done in one-way only, i.e., 
	/// from source (this collection) to the global collection. 
	/// 
	/// Items in the collection must be unique and not null.
	/// </remarks>
	/// <typeparam name="T">The type of elements in the collection.</typeparam>
	/// <typeparam name="TP">The type of the subitems element.</typeparam>
	public abstract class ObservableCollectionWithContentSync<T, TC> : ObservableCollection<T>
		where T : class 
	{
		/// <summary>
		/// Gets the global collection keeping all subitems for all items in this collection.
		/// </summary>		
		/// <value>
		/// The global collection of subitems for all items in this collection.
		/// </value>
		/// <exception cref="InvalidOperationException">This collection is not empty.</exception>
		public ICollection<TC> GlobalSubItemsCollection
		{
			get
			{
				return _globalSubItemsCollection;
			}
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidOperationException>(this.Count == 0);
				_globalSubItemsCollection = value;
			}
		}
		
		private ICollection<TC> _globalSubItemsCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithItemContentSync{TC}" /> class.
		/// </summary>
		/// <remarks>If gcoll is null, the global collection is automatically created. The reference to global collection
		/// can be modified only when this collection is empty, i.e., usually upon construction of collections.
		/// </remarks>
		/// <param name="gcoll">The global collection for the synchronization.</param>		
		public ObservableCollectionWithContentSync(ICollection<TC> gcoll = null) : base()
		{
			this.GlobalSubItemsCollection = gcoll ?? new ObservableCollection<TC>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithItemContentSync{TC}" /> class.
		/// </summary>
		/// <remarks>If gcoll is null, the global collection is automatically created. The reference to global collection
		/// can be modified only when this collection is empty, i.e., usually upon construction of collections.
		/// </remarks>
		/// <param name="gcoll">The global collection for the synchronization.</param>
		/// <param name="collection">The collection from which the elements are copied (may not be null).</param>
		public ObservableCollectionWithContentSync(ICollection<TC> gcoll, IEnumerable<T> collection) : base (collection)			
		{
			this.GlobalSubItemsCollection = gcoll ?? new ObservableCollection<TC>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithItemContentSync{TC}" /> class.
		/// </summary>
		/// <remarks>If gcoll is null, the global collection is automatically created. The reference to global collection
		/// can be modified only when this collection is empty, i.e., usually upon construction of collections.
		/// </remarks>
		/// <param name="gcoll">The global collection for the synchronization.</param>
		/// <param name="collection">The collection from which the elements are copied (may not be null).</param>
		public ObservableCollectionWithContentSync(ICollection<TC> gcoll, List<T> collection) : base(collection)
		{
			this.GlobalSubItemsCollection = gcoll ?? new ObservableCollection<TC>();
		}

		/// <summary>
		/// Inserts the item into the collection.
		/// </summary>
		/// <param name="index">The index at which the item is to insert.</param>
		/// <param name="item">The item to insert.</param>
		protected override void InsertItem(int index, T item)
		{
			Contract.Assume(index >= 0);
			Contract.Assume(index <= this.Count);

			this.CheckReentrancy();
			this.CheckArgument(item);

			base.InsertItem(index, item);

			AddSubItems(item);	//may recursively call InsertItem with another item
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

			T oldItem = base[index];
			base.SetItem(index, item);

			//remove evaluations of the old item and add evaluation of the newitem										
			RemoveSubItems(oldItem);	//does not call RemoveItem  (parent items are always valid)
			AddSubItems(item);			//may call InsertItem for another item referenced indirectly through one subitem.
		}

		/// <summary>
		/// Removes the item at the specific index.
		/// </summary>
		/// <param name="index">The index at which to remove the item.</param>
		protected override void RemoveItem(int index)
		{
			Contract.Assume(index >= 0);
			Contract.Assume(index < this.Count);
			Contract.Assume(base[index] != null);

			this.CheckReentrancy();

			T oldItem = base[index];
			base.RemoveItem(index);
			
			RemoveSubItems(oldItem);	//does not call RemoveItem (parent items are always valid)
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
		/// <exception cref="System.ArgumentException">item is already in the collection</exception>
		protected virtual void CheckArgument(T item)
		{
			Contract.Ensures(item != null);		//if the routine exits normally, it is guaranteed that item is not null and not in the collectiton
			Contract.Ensures(this.Contains(item) == false);

			if (item == null)
				throw new ArgumentNullException();

			if (this.Contains(item))
				throw new ArgumentException("item is already in the collection");
		}
		
		/// <summary>
		/// Adds the subitems of the specified parent item into the global collection and enables their synchronizations.
		/// </summary>
		/// <param name="item">The item.</param>
		protected void AddSubItems(T item)
		{
			Contract.Requires(item != null);

			//adds evaluations not already present in this.GlobalSubItemsCollection
			ObservableCollection<TC> col = GetSubItemsCollection(item);
			AddSubItems(col);

			//register ourselves to detect every change of the collection (enable synchronization)
			col.CollectionChanged += SubItems_CollectionChanged;
		}

		/// <summary>
		/// Removes the evaluations of the specified parent from the global collection and disables their synchronizations.
		/// </summary>
		/// <param name="oldItem">The old item.</param>
		protected void RemoveSubItems(T oldItem)
		{
			Contract.Requires(oldItem != null);

			//remove detection of every change of the collection (disable synchronization)
			ObservableCollection<TC> col = GetSubItemsCollection(oldItem);
			col.CollectionChanged -= SubItems_CollectionChanged;
			
			//remove evaluations of the oldItem
			RemoveSubItems(col.ToList());	//as the concrete implementation may lead to modification of SubItems collection, we need to copy it
		}
				
		/// <summary>
		/// Handles the CollectionChanged event of the GlobalSubItemsCollection control.
		/// </summary>
		/// <param name="sender">The source of the event (ObservableCollection&lt;Evaluation&gt;).</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
		private void SubItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.Assert(e.Action != NotifyCollectionChangedAction.Reset);

			//synchronize evaluations collection (sender) with the global collection of evaluations			
			if (e.NewItems != null)
			{
				//add evaluations in the global GlobalSubItemsCollection collection
				AddSubItems(e.NewItems);
			}
		}

		/// <summary>
		/// Adds the subitems from the give list into the global collection.
		/// </summary>
		/// <remarks>If a subitem already exists in the global collection, it is skipped.</remarks>
		/// <param name="toRemove">items to remove.</param>
		protected virtual void AddSubItems(IList toAdd)
		{
			Contract.Requires(toAdd != null);

			foreach (TC item in toAdd)
			{
				if (!this.GlobalSubItemsCollection.Contains(item))
					this.GlobalSubItemsCollection.Add(item);
			}
		}

		/// <summary>
		/// Gets the collection of sub items of the give item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The retrieved collection.</returns>
		virtual protected ObservableCollection<TC> GetSubItemsCollection(T item)
		{
			Contract.Requires(item != null);

			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the evaluations from the global collection.
		/// </summary>
		/// <remarks>A sub item cannot be removed from the global collection, if it is referenced from another collection
		/// that is being synchronized with the same global collection and, therefore, the implementation is left for concrete classes.</remarks>
		/// <param name="toRemove">items to remove.</param>
		virtual protected void RemoveSubItems(IList toRemove)
		{
			Contract.Requires(toRemove != null);

			throw new NotImplementedException();
		}
		
	}
}
