using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Core.Collection
{
	/// <summary>
	/// This collection represents a dynamic collection of evaluations belonging to one particular student (referenced as parent).
	/// </summary>
	public class EvaluationObservableCollectionWithParentStudent : ObservableCollectionWithParentReference<Evaluation, Student>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EvaluationCollectionWithParent" /> class.
		/// </summary>
		/// <param name="parent">The parent associated with this collection (may not be null).</param>
		/// <param name="TPropertyNameWithReferenceToParent">The name of the property of T that contains the reference to parent.
		/// May be null, in which case the name is assumed to be the name of TP class.</param>
		public EvaluationObservableCollectionWithParentStudent(Student parent, string TPropertyNameWithReferenceToParent = null)
			: base(parent, TPropertyNameWithReferenceToParent)
		{
			Contract.Requires(parent != null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentReference{TP}" /> class.
		/// </summary>
		/// <param name="parent">The parent associated with this collection (may not be null)..</param>
		/// <param name="TPropertyNameWithReferenceToParent">The name of the property of T that contains the reference to parent.
		/// May be null, in which case the name is assumed to be the name of TP class.</param>
		/// <param name="collection">The collection from which the elements are copied (may not be null).</param>
		public EvaluationObservableCollectionWithParentStudent(Student parent, string TPropertyNameWithReferenceToParent, 
			IEnumerable<Evaluation> collection) : base (parent, TPropertyNameWithReferenceToParent, collection)
		{
			Contract.Requires(parent != null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionWithParentReference{TP}" /> class.
		/// </summary>
		/// <param name="parent">The parent associated with this collection (may not be null)..</param>
		/// <param name="TPropertyNameWithReferenceToParent">The name of the property of T that contains the reference to parent.
		/// May be null, in which case the name is assumed to be the name of TP class.</param>
		/// <param name="collection">The collection from which the elements are copied (may not be null).</param>
		public EvaluationObservableCollectionWithParentStudent(Student parent, string TPropertyNameWithReferenceToParent,
			List<Evaluation> collection) : base(parent, TPropertyNameWithReferenceToParent, collection)
		{
			Contract.Requires(parent != null);	
		}
	}
}
