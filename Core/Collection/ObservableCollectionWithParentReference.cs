using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zcu.StudentEvaluator.Core.Collection
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed, and
    /// maintain a relationship between every item and parent of this collection.
    /// </summary>
    /// <remarks>An item is supposed to have a reference to the parent of this collection and parent of this collection is supposed to
    /// have reference to this collection, i.e., it is possible to have Parent->ChildrenCollection and each Child in ChildrenCollection has the reference 
    /// to Parent. Adding a child into the collection automatically sets its reference to Parent (in a concrete class), removing a child from the 
    /// collection disrupts this link as well. No child item can be inserted twice. No null items my be inserted. 
    /// The reference in the child is stored in a property named ParentReferencePropertyName. ParentReferencePropertyName may be null
    /// in which case the property name must be the same as the name of class TP.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <typeparam name="TP">The type of the parent element associated with the collection (and with any element).</typeparam>
    public class ObservableCollectionWithParentReference<T, TP> : ObservableCollection<T> 
        where T : class where TP: class
    {
                /// <summary>
        /// Gets the parent associated with this collection.
        /// </summary>
        /// <value>
        /// The parent associated with this collection.
        /// </value>
        public TP Parent { get; private set; }

        /// <summary>
        /// Gets the name of the property (of T) that references to the parent.
        /// </summary>
        /// <value>
        /// The name of  the property (of T) that references to the parent (may be null)
        /// </value>
        public string ParentReferencePropertyName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationCollectionWithParent" /> class.
        /// </summary>
        /// <param name="parent">The parent associated with this collection (may not be null).</param>
        /// <param name="TPropertyNameWithReferenceToParent">The name of the property of T that contains the reference to parent.
        /// May be null, in which case the name is assumed to be the name of TP class.</param>
        public ObservableCollectionWithParentReference(TP parent, string TPropertyNameWithReferenceToParent = null) : base()
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.Parent = parent;
            this.ParentReferencePropertyName = TPropertyNameWithReferenceToParent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionWithParentReference{TP}" /> class.
        /// </summary>
        /// <param name="parent">The parent associated with this collection (may not be null)..</param>
        /// <param name="TPropertyNameWithReferenceToParent">The name of the property of T that contains the reference to parent.
        /// May be null, in which case the name is assumed to be the name of TP class.</param>
        /// <param name="collection">The collection from which the elements are copied (may not be null).</param>
        public ObservableCollectionWithParentReference(TP parent, string TPropertyNameWithReferenceToParent, IEnumerable<T> collection) : base (collection)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.Parent = parent;
            this.ParentReferencePropertyName = TPropertyNameWithReferenceToParent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionWithParentReference{TP}" /> class.
        /// </summary>
        /// <param name="parent">The parent associated with this collection (may not be null)..</param>
        /// <param name="TPropertyNameWithReferenceToParent">The name of the property of T that contains the reference to parent.
        /// May be null, in which case the name is assumed to be the name of TP class.</param>
        /// <param name="collection">The collection from which the elements are copied (may not be null).</param>
        public ObservableCollectionWithParentReference(TP parent, string TPropertyNameWithReferenceToParent, List<T> collection)
            : base(collection)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            this.Parent = parent;
            this.ParentReferencePropertyName = TPropertyNameWithReferenceToParent;
        }
        
        /// <summary>
        /// Inserts the item into the collection.
        /// </summary>
        /// <param name="index">The index at which the item is to insert.</param>
        /// <param name="item">The item to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            this.CheckReentrancy();             //modifying collection from OnCollectionChange is not allowed 
            this.CheckArgument(item);           //check validity of the item

            using (var rp = new RecursionPoint(this))
            {
                if (rp.IsRecursive())
                    return; //recursion, so exit

                SetEvaluationParent(item, this.Parent);   //this may recursively call InsertItem
                base.InsertItem(index, item);
            }
        }
        
        /// <summary>
        /// Sets the new item at the specific index.
        /// </summary>
        /// <param name="index">The index at which to modify the item.</param>
        /// <param name="item">The new item.</param>
        protected override void SetItem(int index, T item)
        {
            this.CheckReentrancy();             //modifying collection from OnCollectionChange is not allowed 
            this.CheckArgument(item);

            using (var rp = new RecursionPoint(this, "RemoveItem"))
            {                
                SetEvaluationParent(base[index], null);    //this may recursively call RemoveItem but we prevent it
            }

            using (var rp = new RecursionPoint(this, "InsertItem"))
            {
                SetEvaluationParent(item, this.Parent);  //this may recursively call InsertItem but we prevent it
                base.SetItem(index, item);
            }
        }

        /// <summary>
        /// Removes the item at the specific index.
        /// </summary>
        /// <param name="index">The index at which to remove the item..</param>
        protected override void RemoveItem(int index)
        {
            this.CheckReentrancy();  //modifying the collection from OnCollectionChange is not allowed 

            using (var rp = new RecursionPoint(this))
            {
                if (rp.IsRecursive())
                    return; //recursion, so exit

                SetEvaluationParent(base[index], null); //may recursively call RemoveItem
                base.RemoveItem(index);
            }
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
            if (item == null)
                throw new ArgumentNullException();

            if (this.Parent == GetEvaluationParent(item)) //if the item is already in this collection
            {
                if (this.Contains(item))
                    throw new ArgumentException("item is already in the collection");
            }
        }

        /// <summary>
        /// Gets the parent object associated with the given evaluation item .
        /// </summary>
        /// <param name="item">The item whose parent is to be retrieved.</param>        
        protected virtual TP GetEvaluationParent(T item)
        {
            return (TP)item.GetType().InvokeMember(
                (this.ParentReferencePropertyName ?? typeof(TP).Name), //(this.ParentReferencePropertyName != null ? this.ParentReferencePropertyName : typeof(TP).Name), 
                System.Reflection.BindingFlags.GetProperty, null,  item, null);
        }

        /// <summary>
        /// Sets the parent object to the given evaluation item .
        /// </summary>
        /// <param name="item">The item whose parent is to be set.</param>
        /// <param name="parent">The object to be associated with the item as a parent.</param>
        protected virtual void SetEvaluationParent(T item, TP parent)
        {
            item.GetType().InvokeMember(
                (this.ParentReferencePropertyName ?? typeof(TP).Name), 
                System.Reflection.BindingFlags.SetProperty, null, item, new object[]{parent});
        }
    }
}
