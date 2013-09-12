using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Core.Collection
{
	public class EvaluationObservableCollectionWithParentSync : ObservableCollectionWithParentSync<Evaluation>
	{
		/// <summary>
		/// Gets the global collection keeping all Categories for all items in this collection.
		/// </summary>		
		/// <value>
		/// The global collection of categories.
		/// </value>
		/// <exception cref="InvalidOperationException">This collection is not empty.</exception>
		public ICollection<Category> GlobalCategoryCollection
		{
			get
			{
				return _globalCategoryCollection;
			}
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidOperationException>(this.Count == 0);
				_globalCategoryCollection = value;
			}
		}

		private ICollection<Category> _globalCategoryCollection;

		/// <summary>
		/// Gets the global collection keeping all students for all items in this collection.
		/// </summary>		
		/// <value>
		/// The global collection of students.
		/// </value>
		/// <exception cref="InvalidOperationException">This collection is not empty.</exception>
		public ICollection<Student> GlobalStudentCollection
		{
			get
			{
				return _globalStudentCollection;
			}
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires<InvalidOperationException>(this.Count == 0);
				_globalStudentCollection = value;
			}
		}

		private ICollection<Student> _globalStudentCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentSync{T}" /> class.
		/// </summary>
		/// <remarks>If a global collection is not specified (null),  default collection is automatically constructed (ObservableCollectionWithContentSync(this)). </remarks>
		/// <param name="catcol">The global collection of categories.</param>
		/// <param name="stcol">The global collection of students.</param>
		public EvaluationObservableCollectionWithParentSync(ICollection<Category> catcol = null, ICollection<Student> stcol = null) : base()
		{
			this.GlobalCategoryCollection = catcol ?? new CategoryObservableCollectionWithContentSync(this);
			this.GlobalStudentCollection = stcol ?? new StudentObservableCollectionWithContentSync(this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentSync{T}"/> class.
		/// </summary>
		/// <remarks>If a global collection is not specified (null),  default collection is automatically constructed (ObservableCollectionWithContentSync(this)). </remarks>
		/// <param name="catcol">The global collection of categories.</param>
		/// <param name="stcol">The global collection of students.</param>
		/// <param name="collection">The collection from which items are copied.</param>
		public EvaluationObservableCollectionWithParentSync(ICollection<Category> catcol, ICollection<Student> stcol, IEnumerable<Evaluation> collection)
			: base(collection)
		{
			this.GlobalCategoryCollection = catcol ?? new CategoryObservableCollectionWithContentSync(this);
			this.GlobalStudentCollection = stcol ?? new StudentObservableCollectionWithContentSync(this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentSync{T}"/> class.
		/// </summary>
		/// <remarks>If a global collection is not specified (null),  default collection is automatically constructed (ObservableCollectionWithContentSync(this)). </remarks>
		/// <param name="catcol">The global collection of categories.</param>
		/// <param name="stcol">The global collection of students.</param>
		/// <param name="collection">The collection from which items are copied.</param>
		public EvaluationObservableCollectionWithParentSync(ICollection<Category> catcol, ICollection<Student> stcol, List<Evaluation> collection)
			: base(collection)
		{
			this.GlobalCategoryCollection = catcol ?? new CategoryObservableCollectionWithContentSync(this);
			this.GlobalStudentCollection = stcol ?? new StudentObservableCollectionWithContentSync(this);
		}

		/// <summary>
		/// Adds the parents of the specified item into the global collections.
		/// </summary>
		/// <param name="item">The item.</param>
		protected override void AddParentItems(Evaluation item)
		{
			Contract.Assume(this.GlobalCategoryCollection != null);
			Contract.Assume(this.GlobalStudentCollection != null);

			if (item.Category != null && !this.GlobalCategoryCollection.Contains(item.Category))
				this.GlobalCategoryCollection.Add(item.Category);

			if (item.Student != null && !this.GlobalStudentCollection.Contains(item.Student))
				this.GlobalStudentCollection.Add(item.Student);
		}

		/// <summary>
		/// Removes the parents of the specified item from the global collection.
		/// </summary>
		/// <param name="oldItem">The old item.</param>
		protected override void RemoveParentItems(Evaluation oldItem)
		{
			oldItem.Category = null;   //this will cause a change of item.Category.Evaluations, which will be handled by CategoryCollection, which will call us recursively
			oldItem.Student = null;    //and this is similar
		}

		/// <summary>
		/// Called when some property of an item referencing to the parent item has changed.
		/// </summary>
		/// <remarks>Concrete classes are supposed to call this.Remove, if the item has no valid parent, and add  new parents into global collections.</remarks>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnParentReferencePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Contract.Assume(this.GlobalCategoryCollection != null);
			Contract.Assume(this.GlobalStudentCollection != null);

			Evaluation eval = (Evaluation)sender;
			Contract.Assume(eval != null);

			switch (e.PropertyName)
			{
				case "Category":
					if (eval.Category != null)
					{
						if (!this.GlobalCategoryCollection.Contains(eval.Category))						
							this.GlobalCategoryCollection.Add(eval.Category);						
					}
					else if (eval.Student == null)
					{
						this.Remove(eval);
					}
					break;

				case "Student":
					if (eval.Student != null)
					{
						if (!this.GlobalStudentCollection.Contains(eval.Student))						
							this.GlobalStudentCollection.Add(eval.Student);						
					}
					else if (eval.Category == null)
					{
						this.Remove(eval);
					}

					break;
			}
		}
	}
}
