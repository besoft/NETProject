using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Core.Collection
{
	/// <summary>
	/// This class represents a dynamic collection of category items 
	/// </summary>
	public class CategoryObservableCollectionWithContentSync : ObservableCollectionWithContentSync<Category,Evaluation>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithItemContentSync{TC}" /> class.
		/// </summary>
		/// <remarks>If gcoll is null, the global collection is automatically created. The reference to global collection
		/// can be modified only when this collection is empty, i.e., usually upon construction of collections.
		/// </remarks>
		/// <param name="gcoll">The global collection for the synchronization.</param>		
		public CategoryObservableCollectionWithContentSync(ICollection<Evaluation> gcoll = null) : base(gcoll)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithItemContentSync{TC}" /> class.
		/// </summary>
		/// <remarks>If gcoll is null, the global collection is automatically created. The reference to global collection
		/// can be modified only when this collection is empty, i.e., usually upon construction of collections.
		/// </remarks>
		/// <param name="gcoll">The global collection for the synchronization.</param>
		/// <param name="collection">The collection from which the elements are copied (may not be null).</param>
		public CategoryObservableCollectionWithContentSync(ICollection<Evaluation> gcoll, IEnumerable<Category> collection) : 
			base (gcoll, collection)			
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithItemContentSync{TC}" /> class.
		/// </summary>
		/// <remarks>If gcoll is null, the global collection is automatically created. The reference to global collection
		/// can be modified only when this collection is empty, i.e., usually upon construction of collections.
		/// </remarks>
		/// <param name="gcoll">The global collection for the synchronization.</param>
		/// <param name="collection">The collection from which the elements are copied (may not be null).</param>
		public CategoryObservableCollectionWithContentSync(ICollection<Evaluation> gcoll, List<Category> collection)
			: base(gcoll, collection)
		{
			
		}

		/// <summary>
		/// Gets the collection of sub items of the give item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The retrieved collection.</returns>
		protected override ObservableCollection<Evaluation> GetSubItemsCollection(Category item)
		{
			return item.Evaluations;
		}

		/// <summary>
		/// Removes the evaluations from the global collection.
		/// </summary>
		/// <remarks>A sub item cannot be removed from the global collection, if it is referenced from another collection
		/// that is being synchronized with the same global collection and, therefore, the implementation is left for concrete classes.</remarks>
		/// <param name="toRemove">items to remove.</param>
		protected override void RemoveSubItems(IList toRemove)
		{
			foreach (Evaluation item in toRemove)
			{
				item.Category = null; 
				
				//will cause PropertyChanged notification, which will be handled by EvaluationCollection
				//and, if item.Student is also null, the subitem will be removed physically				
			}	
		}
	}
}
