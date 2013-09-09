using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Domain;
using Zcu.StudentEvaluator.Core;
using Zcu.StudentEvaluator.Core.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace Zcu.StudentEvaluator.Domain.Test
{
	/// <summary>
	/// Repository of Students, Categories and Evaluations that is automatically synchronized.
	/// </summary>
	/// <remarks>
	/// It is expected that the caller Adds valid items to these collections, i.e., these are not neither null nor already existing!
	/// Only one collection may change at the same time, which allows synchronization of the data in the repository with the data 
	/// changed outside (by manipulating with Student, Category, Evaluation members or directly with Students, Categories, Evaluations.
	/// 
	/// Add (this.Students)
	/// => add student into this.Students [internal]
	/// => add its evaluations not already in this.Evaluations into that collection
	/// => register handler of student.Evaluations collection change
	/// 
	/// Add (this.Categories)
	/// => add parent into this.Categories [internal]
	/// => add its evaluations not already in this.Evaluations into that collection
	/// => register handler of parent.Evaluations collection change
	/// 
	/// Add (this.Evaluations)
	/// => add evaluation into this.Evaluations [internal]
	/// * should have both Student and Category not null!
	/// => add its Student not already in this.Students into that collection and register handler for Student property change
	/// => add its Category not already in this.Categoris into that collection and register handler for Category property change
	/// 
	/// Add (Student or Category.Evaluations)
	/// => the same as Add (this.Evaluations)
	/// 
	/// Remove  (this.Students)
	/// => remove student from this.Students [internal]
	/// => unregister handler of student.Evaluations collection change
	/// => for each of its evaluations (they are inthis.Evaluations) set its Student to null     
	/// 
	/// Remove  (this.Categories)
	/// => remove parent from this.Categories [internal]
	/// => unregister handler of parent.Evaluations collection change
	/// => for each of its evaluations (they are inthis.Evaluations) set its Category to null 
	/// 
	/// Remove  (this.Evaluations)
	/// => remove evaluation from this.Evaluations [internal]
	/// => unregister handler for its Student and Category property change
	/// => set its Student = null and its Category = null
	/// 
	/// Remove (student or parent.Evaluations)
	/// => the same as Remove (this.Evaluations)
	/// 
	/// Change (evaluation.Student)
	/// * old value is not known
	/// => if evaluation.Student is not null and it is not already in this.Students, do Add(this.Students) for evaluation.Student
	/// => if evaluation.Student  is null and evaluation.Category is null, do Remove(this.Evaluations) for evaluation
	/// 
	/// Change (evaluation.Category)
	/// * old value is not known
	/// => if evaluation.Category is not null and it is not already in this.Categories, do Add(this.Categories) for evaluation.Category
	/// => if evaluation.Student is null and evaluation.Category is null, do Remove(this.Evaluations) for evaluation
	/// 
	/// Replace => Remove old + Add new
	/// 
	/// Reset (this.Students)
	/// * neither old not new items are available
	/// * some students from this.Students were removed [internal]
	/// * this.Evaluation is still not modified => it contains all students
	/// => for each this.Evaluations.Student not in this.Students plan Remove(this.Student)
	/// => for each this.Students not this.Evaluations.Student plan Add(this.Student)
	/// => perform both plans
	/// 
	/// Reset (this.Categories)
	/// * neither old not new items are available
	/// * some categories from this.Categories were removed [internal]
	/// * this.Evaluation is still not modified => it contains all categories
	/// => for each this.Evaluations.Category not in this.Categories plan Remove(this.Category)
	/// => for each this.Categories not this.Evaluations.Category plan Add(this.Category)
	/// => perform both plans
	/// 
	/// Reset (Student.Evaluations)  
	/// * collection of this.Students[?].Evaluation has changed significantly
	/// * this is only a theoretical case since currently dramatic changes *e.g., Clear) are not supported
	/// * this.Evaluations is unchanged
	/// => for each this.Evaluations having Student not null that is not in student.Evaluations, add it to plan Remove(this.Evaluations)
	/// => for each student.Evaluations not this.Evaluations plan Add(this.Evaluations)
	/// => perform both plans
	///  
	/// Reset (Category.Evaluations)  
	/// * collection of this.Categories[?].Evaluation has changed significantly
	/// * this is only a theoretical case since currently dramatic changes *e.g., Clear) are not supported
	/// * this.Evaluations is unchanged
	/// => for each this.Evaluations having Category not null that is not in parent.Evaluations, add it to plan Remove(this.Evaluations)
	/// => for each parent.Evaluations not this.Evaluations plan Add(this.Evaluations)
	/// => perform both plans
	/// 
	/// Reset (this.Evaluations)
	/// * neither old not new items are available
	/// * some evaluations from this.Evaluations were removed [internal]
	/// * this.Students and this.Categories are still not modified and they contain all evaluations in their .Evaluations
	/// => for each this.Students.Evaluations not in this.Evaluations, add it to remoCol
	/// => for each this.Categories.Evaluations not in this.Evaluations, add it to remoCol2
	///  => for each this.Evaluations not in neither this.Students.Evaluations nor this.Caregories.Evaluations, 
	///  i.e., these were internally added, do Add(this.Evaluations) for it
	/// => do Remove(this.Evaluations) for (remCol XOR remCol2), i.e., for items either present in remCol or remCol2 but not in both
	/// 
	
	
	/// </remarks>
	public class TestRepository
	{        
		protected class CategoriesCollection : ObservableCollection<Category>
		{
			/// <summary>
			/// Gets the collection keeping all evaluations for all items in this collection.
			/// </summary>
			/// <value>
			/// The evaluation collection associated with this collection.
			/// </value>
			public ObservableCollection<Evaluation> Evaluations { get; private set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="EvaluationCollectionWithParent" /> class.
			/// </summary>
			/// <param name="evaluations">The collection that should contain evaluations of every parent in this collection.</param>
			public CategoriesCollection(ObservableCollection<Evaluation> evaluations)
				: base()
			{
				Contract.Requires<ArgumentNullException>(evaluations != null);

				this.Evaluations = evaluations;
			}

			/// <summary>
			/// Inserts the item into the collection.
			/// </summary>
			/// <param name="index">The index at which the item is to insert.</param>
			/// <param name="item">The item to insert.</param>
			protected override void InsertItem(int index, Category item)
			{
				if (item == null) 
					throw new ArgumentNullException();
				if (this.Contains(item)) 
					throw new ArgumentException("The item is already present in the collection");
								
				base.InsertItem(index, item);
				AddCategoryEvaluations(item);
			}
		   
			/// <summary>
			/// Sets the new item at the specific index.
			/// </summary>
			/// <param name="index">The index at which to modify the item.</param>
			/// <param name="item">The new item.</param>
			protected override void SetItem(int index, Category item)
			{
				if (item == null) 
					throw new ArgumentNullException();
				if (this.Contains(item)) 
					throw new ArgumentException("The item is already present in the collection");

				//remove evaluations of the old item and add evaluation of the newitem
				Category oldItem = base[index];				
				RemoveCategoryEvaluations(oldItem);				
				AddCategoryEvaluations(item);
				base.SetItem(index, item);
			}

			/// <summary>
			/// Removes the item at the specific index.
			/// </summary>
			/// <param name="index">The index at which to remove the item.</param>
			protected override void RemoveItem(int index)
			{
				Category oldItem = base[index];
				base.RemoveItem(index);

				RemoveCategoryEvaluations(oldItem);
			}

			/// <summary>
			/// Clears the items.
			/// </summary>
			protected override void ClearItems()
			{
				foreach (var item in this.Items)
				{
					RemoveCategoryEvaluations(item);
				}
				base.ClearItems();
			}

			/// <summary>
			/// Adds the evaluations of the specified parent into the global collection and enables their synchronizations.
			/// </summary>
			/// <param name="item">The item.</param>
			protected void AddCategoryEvaluations(Category item)
			{
				Contract.Requires(item != null);

				//adds evaluations not already present in this.Evaluations
				AddEvaluations(item.Evaluations.ToList());
				
				//register ourselves to detect every change of the collection (enable synchronization)
				item.Evaluations.CollectionChanged += Evaluations_CollectionChanged;
			}
		   
			/// <summary>
			/// Removes the evaluations of the specified parent from the global collection and disables their synchronizations.
			/// </summary>
			/// <param name="oldItem">The old item.</param>
			protected void RemoveCategoryEvaluations(Category oldItem)
			{
				Contract.Requires(oldItem != null);

				//remove detection of every change of the collection (disable synchronization)
				oldItem.Evaluations.CollectionChanged -= Evaluations_CollectionChanged;

				//remove evaluations of the oldItem
				RemoveEvaluations(oldItem.Evaluations.ToList());
			}

			/// <summary>
			/// Adds the evaluations into the global collection.
			/// </summary>
			/// <remarks>If an evaluation already exists in the global collection, it is skipped.</remarks>
			/// <param name="toRemove">items to remove.</param>
			private void AddEvaluations(IList toAdd)
			{
				Contract.Requires(toAdd != null);

				foreach (Evaluation item in toAdd)
				{
					if (!this.Evaluations.Contains(item))
						this.Evaluations.Add(item);
				}
			}

			/// <summary>
			/// Removes the evaluations from the global collection.
			/// </summary>
			/// <param name="toRemove">items to remove.</param>
			private void RemoveEvaluations(IList toRemove)
			{
				Contract.Requires(toRemove != null);

				foreach (Evaluation item in toRemove)
				{                    
					this.Evaluations.Remove(item);
				}
			}

			/// <summary>
			/// Handles the CollectionChanged event of the Evaluations control.
			/// </summary>
			/// <param name="sender">The source of the event (ObservableCollection&lt;Evaluation&gt;).</param>
			/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
			private void Evaluations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				//synchronize evaluations collection (sender) with the global collection of evaluations
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					//collection has changed significantly, so we may have now evaluations in the global
					//evaluations collection with Category = null => just remove them                    
					RemoveEvaluations((
						from x in this.Evaluations
						where x.Category == null
						select x).ToList()
					);
					AddEvaluations(sender as ObservableCollection<Evaluation>);
					
					return;
				}

				if (e.Action == NotifyCollectionChangedAction.Replace ||
					e.Action == NotifyCollectionChangedAction.Remove)
				{
					//remove evaluations in the categories to remove (replace)
					RemoveEvaluations(e.OldItems);
				}

				if (e.Action == NotifyCollectionChangedAction.Replace ||
					e.Action == NotifyCollectionChangedAction.Add)
				{
					//add evaluations in the categories to add (replace)
					AddEvaluations(e.NewItems);
				}
			}
		}

		protected class StudentsCollection : ObservableCollection<Student>
		{
			/// <summary>
			/// Gets the collection keeping all evaluations for all items in this collection.
			/// </summary>
			/// <value>
			/// The evaluation collection associated with this collection.
			/// </value>
			public ObservableCollection<Evaluation> Evaluations { get; private set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="EvaluationCollectionWithParent" /> class.
			/// </summary>
			/// <param name="evaluations">The collection that should contain evaluations of every parent in this collection.</param>
			public StudentsCollection(ObservableCollection<Evaluation> evaluations)
				: base()
			{
				Contract.Requires<ArgumentNullException>(evaluations != null);

				this.Evaluations = evaluations;
			}

			/// <summary>
			/// Inserts the item into the collection.
			/// </summary>
			/// <param name="index">The index at which the item is to insert.</param>
			/// <param name="item">The item to insert.</param>
			protected override void InsertItem(int index, Student item)
			{
				if (item == null)
					throw new ArgumentNullException();
				if (this.Contains(item))
					throw new ArgumentException("The item is already present in the collection");
				
				base.InsertItem(index, item);
				AddStudentEvaluations(item);
			}

			/// <summary>
			/// Sets the new item at the specific index.
			/// </summary>
			/// <param name="index">The index at which to modify the item.</param>
			/// <param name="item">The new item.</param>
			protected override void SetItem(int index, Student item)
			{
				if (item == null)
					throw new ArgumentNullException();
				if (this.Contains(item))
					throw new ArgumentException("The item is already present in the collection");

				//remove evaluations of the old item and add evaluation of the newitem
				Student oldItem = base[index];
				base.SetItem(index, item);

				RemoveStudentEvaluations(oldItem);
				AddStudentEvaluations(item);
			}

			/// <summary>
			/// Removes the item at the specific index.
			/// </summary>
			/// <param name="index">The index at which to remove the item.</param>
			protected override void RemoveItem(int index)
			{
				Student oldItem = base[index];
				base.RemoveItem(index);
				RemoveStudentEvaluations(oldItem);				
			}
			/// <summary>
			/// Clears the items.
			/// </summary>
			protected override void ClearItems()
			{
				foreach (var item in this.Items)
				{
					RemoveStudentEvaluations(item);
				}
				base.ClearItems();
			}

			/// <summary>
			/// Adds the evaluations of the specified parent into the global collection and enables their synchronizations.
			/// </summary>
			/// <param name="item">The item.</param>
			protected void AddStudentEvaluations(Student item)
			{
				Contract.Requires(item != null);

				//adds evaluations not already present in this.Evaluations
				AddEvaluations(item.Evaluations.ToList());

				//register ourselves to detect every change of the collection (enable synchronization)
				item.Evaluations.CollectionChanged += Evaluations_CollectionChanged;
			}

			/// <summary>
			/// Removes the evaluations of the specified parent from the global collection and disables their synchronizations.
			/// </summary>
			/// <param name="oldItem">The old item.</param>
			protected void RemoveStudentEvaluations(Student oldItem)
			{
				Contract.Requires(oldItem != null);

				//remove evaluations of the oldItem
				RemoveEvaluations(oldItem.Evaluations.ToList());

				//remove detection of every change of the collection (disable synchronization)
				oldItem.Evaluations.CollectionChanged -= Evaluations_CollectionChanged;
			}

			/// <summary>
			/// Adds the evaluations into the global collection.
			/// </summary>
			/// <remarks>If an evaluation already exists in the global collection, it is skipped.</remarks>
			/// <param name="toRemove">items to remove.</param>
			private void AddEvaluations(IList toAdd)
			{
				Contract.Requires(toAdd != null);

				foreach (Evaluation item in toAdd)
				{
					if (!this.Evaluations.Contains(item))
						this.Evaluations.Add(item);
				}
			}

			/// <summary>
			/// Removes the evaluations from the global collection.
			/// </summary>
			/// <param name="toRemove">items to remove.</param>
			private void RemoveEvaluations(IList toRemove)
			{
				Contract.Requires(toRemove != null);
				
				foreach (Evaluation item in toRemove)
				{
					if (item.Category == null)
						this.Evaluations.Remove(item);
				}
			}

			/// <summary>
			/// Handles the CollectionChanged event of the Evaluations control.
			/// </summary>
			/// <param name="sender">The source of the event (ObservableCollection&lt;Evaluation&gt;).</param>
			/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
			private void Evaluations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				//synchronize evaluations collection (sender) with the global collection of evaluations
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					//collection has changed significantly, so we may have now evaluations in the global
					//evaluations collection with Student = null => just remove them                    
					RemoveEvaluations((
						from x in this.Evaluations
						where x.Student == null
						select x).ToList()
					);
					AddEvaluations(sender as ObservableCollection<Evaluation>);

					return;
				}

				if (e.Action == NotifyCollectionChangedAction.Replace ||
					e.Action == NotifyCollectionChangedAction.Remove)
				{
					//remove evaluations in the categories to remove (replace)
					RemoveEvaluations(e.OldItems);
				}

				if (e.Action == NotifyCollectionChangedAction.Replace ||
					e.Action == NotifyCollectionChangedAction.Add)
				{
					//add evaluations in the categories to add (replace)
					AddEvaluations(e.NewItems);
				}
			}
		}
		
		protected class EvaluationsCollection : ObservableCollection<Evaluation>
		{
			public CategoriesCollection Categories { get; private set; }
			public StudentsCollection Students { get; private set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="EvaluationsCollection" /> class.
			/// </summary>
			public EvaluationsCollection()
			{
				this.Categories = new CategoriesCollection(this);
				this.Students = new StudentsCollection(this);
			}

			/// <summary>
			/// Inserts the item into the collection.
			/// </summary>
			/// <param name="index">The index at which the item is to insert.</param>
			/// <param name="item">The item to insert.</param>
			protected override void InsertItem(int index, Evaluation item)
			{
				if (item == null)
					throw new ArgumentNullException();
				if (this.Contains(item))
					throw new ArgumentException("The item is already present in the collection");

				base.InsertItem(index, item);

				if (item.Category != null && !this.Categories.Contains(item.Category))
					this.Categories.Add(item.Category);

				if (item.Student != null && !this.Students.Contains(item.Student))
					this.Students.Add(item.Student);

				item.PropertyChanged += StudentCategoryPropertyChanged;
			}

			/// <summary>
			/// Handles the change of the Student or Category property of an evaluation.
			/// </summary>
			/// <remarks>If the new reference is not included in the global collection of students (or categories) it is included.</remarks>
			/// <param name="sender">The sender.</param>
			/// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>            
			void StudentCategoryPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				Evaluation eval = (Evaluation)sender;

				switch (e.PropertyName)
				{
					case "Student":
						if (eval.Student != null && !this.Students.Contains(eval.Student))
						{
							this.Students.Add(((Evaluation)sender).Student);
						}
						break;

					case "Category": 
						if (eval.Category != null && !this.Categories.Contains(eval.Category))
						{
							this.Categories.Add(((Evaluation)sender).Category);
						}
						break;
				}
			}

			/// <summary>
			/// Sets the new item at the specific index.
			/// </summary>
			/// <param name="index">The index at which to modify the item.</param>
			/// <param name="item">The new item.</param>
			protected override void SetItem(int index, Evaluation item)
			{
				if (item == null)
					throw new ArgumentNullException();
				if (this.Contains(item))
					throw new ArgumentException("The item is already present in the collection");

				//remove evaluations of the old item and add evaluation of the newitem
				Evaluation oldItem = base[index];                              
				base.SetItem(index, item);

				if (item.Category != null && !this.Categories.Contains(item.Category))
					this.Categories.Add(item.Category);

				if (item.Student != null && !this.Students.Contains(item.Student))
					this.Students.Add(item.Student);

				item.PropertyChanged += StudentCategoryPropertyChanged;                
				oldItem.PropertyChanged -= StudentCategoryPropertyChanged;

				oldItem.Category = null;
				oldItem.Student = null;
			}

			/// <summary>
			/// Removes the item at the specific index.
			/// </summary>
			/// <param name="index">The index at which to remove the item.</param>
			protected override void RemoveItem(int index)
			{
				this.CheckReentrancy(); //reentrancy is not allowed (see MSDN)

				using (var rp = new RecursionPoint(this))
				{
					if (rp.IsRecursive())
						return; //avoid recursion

					Evaluation item = base[index];
					item.PropertyChanged -= StudentCategoryPropertyChanged; //no longer need to detect a change of Student or Category

					item.Category = null;   //this will cause a change of item.Category.Evaluations, which will be handled by CategoryCollection, which will call us recursively
					item.Student = null;    //and this is similar
					base.RemoveItem(index);
				}				
			}

			/// <summary>
			/// Clears the items.
			/// </summary>
			protected override void ClearItems()
			{
				var list = this.Items.ToArray();
				base.ClearItems();

				foreach (var item in list)
				{
					item.PropertyChanged -= StudentCategoryPropertyChanged;
					item.Category = null;
					item.Student = null;
				}				
			}
		}

		/// <summary>
		/// Gets the list of evaluation categories.
		/// </summary>
		/// <value>
		/// The list of categories to be contained.
		/// </value>
		public ObservableCollection<Category> Categories { get; private set; }

		/// <summary>
		/// Gets the list of students.
		/// </summary>
		/// <value>
		/// The list of students to be contained.
		/// </value>
		public ObservableCollection<Student> Students { get; private set; }
		
		/// <summary>
		/// Gets the list of evaluations.
		/// </summary>
		/// <value>
		/// The list of evaluations to be contained.
		/// </value>
		public ObservableCollection<Evaluation> Evaluations { get; private set; }        
		

		public TestRepository()
		{
			var evalCol = new EvaluationsCollection();

			this.Evaluations = evalCol;
			this.Categories = evalCol.Categories;
			this.Students = evalCol.Students;


			this.Categories.Add(new Category() { Name = "Design", MinPoints = 2m });
			this.Categories.Add(new Category() { Name = "Implementation", MinPoints = 5m, MaxPoints=10, });
			this.Categories.Add(new Category() { Name = "CodeCulture" });
			this.Categories.Add(new Category() { Name = "Documentation", MaxPoints = 2 });
			
			this.Students.Add(new Student() {PersonalNumber = "A12B0001P", FirstName="Anna", Surname="Aysle", });
			this.Students.Add(new Student() {PersonalNumber = "A12B0002P", FirstName="Barbora", Surname="Bílá", });
			this.Students.Add(new Student() {PersonalNumber = "A12B0003P", FirstName="Cyril", Surname="Cejn", });
						
						
			for (int i = 0, idx = 0; i < this.Students.Count; i++)
			{
				for (int j = 0; j < this.Categories.Count; j++, idx++)
				{
					var eval = new Evaluation()
					{
						Category = this.Categories[j],
						Student = this.Students[i],
					};

					//this.Evaluations.Add(eval);	
				}
			}            
		}
#if _POKUS


		/// <summary>
		/// Called after some students has been added internally into this.Students
		/// </summary>
		/// <remarks>
		/// Add (this.Students)
		/// => add student into this.Students [internal]
		/// => add its evaluations not already in this.Evaluations into that collection
		/// => register handler of student.Evaluations collection change
		/// </remarks>
		/// <param name="list">The list of students that has been added.</param>
		virtual protected void OnAddStudents(IList list)
		{
			foreach (Student st in list)
			{
				foreach (Evaluation eval in st.Evaluations)
				{
					if (!this.Evaluations.Contains(eval))
						this.Evaluations.Add(eval);
				}

				//st.Evaluations.CollectionChanged += //TODO: nema!
			}
		}

		/// <summary>
		/// Handles the CollectionChanged event of the Categories control.
		/// </summary>
		/// <remarks>When a new category is added</remarks>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
		void Categories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{          
			//if some items have been removed or replaced
			if (e.Action == NotifyCollectionChangedAction.Replace ||
				e.Action == NotifyCollectionChangedAction.Remove)
			{
				//Remove their evaluations
				foreach (Category item in e.OldItems)
				{
					RemoveEvaluations(item.Evaluations);
				}
			}

			//if some items have been added or replaced
			if (e.Action == NotifyCollectionChangedAction.Replace ||
				e.Action == NotifyCollectionChangedAction.Add)
			{
				//Add their evaluations
				foreach (Category item in e.NewItems)
				{
					AddEvaluations(item.Evaluations);
				}
			}

			//collection has been changed significantly
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				//remove any evaluation with the category not included in the collection
				var oldItems = new ObservableCollection<Evaluation>();
				var newItems = new ObservableCollection<Evaluation>();
				foreach (var item in this.Evaluations)
				{
					if (item.Category != null)
					{
						if (this.Categories.Contains(item.Category))
							newItems.Add(item); //this is new item
						else
							oldItems.Add(item);  //old items
					}
				}

				AddEvaluations(new ReadOnlyObservableCollection<Evaluation>(newItems));
				RemoveEvaluations(new ReadOnlyObservableCollection<Evaluation>(oldItems));
			}
		}

		/// <summary>
		/// Adds the evaluations into the repository
		/// </summary>
		/// <remarks>Only evaluations not already present in the repository are added. </remarks>
		/// <param name="readOnlyObservableCollection">The read only observable collection.</param>
		private void AddEvaluations(ReadOnlyObservableCollection<Evaluation> readOnlyObservableCollection)
		{
			foreach (var item in readOnlyObservableCollection)
			{
				if (item != null && this.Evaluations.Contains(item) == false)
				{                    
					this.Evaluations.Add(item); //will trigger AddStudent and AddCategory
				}
			}
		}

		/// <summary>
		/// Removes the evaluations from the repository.
		/// </summary>
		/// <param name="readOnlyObservableCollection">The read only observable collection.</param>
		private void RemoveEvaluations(ReadOnlyObservableCollection<Evaluation> readOnlyObservableCollection)
		{
			foreach (var item in readOnlyObservableCollection)
			{
				if (item != null)
				{
					this.Evaluations.Remove(item);  //remove evaluations
				}
			}
		}

		/// <summary>
		/// Handles the CollectionChanged event of the Evaluations collection.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
		void Evaluations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//if some items have been removed or replaced
			if (e.Action == NotifyCollectionChangedAction.Replace ||
				e.Action == NotifyCollectionChangedAction.Remove)
			{
				//Remove their evaluations
				foreach (Evaluation item in e.OldItems)
				{
					//Evaluation is being removed from the repository ->
					//1) unregister us from the evaluation
					item.PropertyChanged -= Evaluation_PropertyChanged;

					//2) Remove it from its Student.Evaluations and Categorory.Evaluations
					if (item.Student != null)
						item.Student.RemoveEvaluation(item);

					if (item.Category != null)
						item.Category.RemoveEvaluation(item);                    
				}
			}

			//if some items have been added or replaced
			if (e.Action == NotifyCollectionChangedAction.Replace ||
				e.Action == NotifyCollectionChangedAction.Add)
			{
				//Add their evaluations
				foreach (Evaluation item in e.NewItems)
				{                    
					AddStudent(item.Student);
					AddCategory(item.Category);

					//register to be able to react to a change of Student and Category of this evaluation
					item.PropertyChanged += Evaluation_PropertyChanged;
				}
			}

			//collection has been changed significantly
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				
				var newItems = new ObservableCollection<Evaluation>();
				foreach (var item in this.Evaluations)
				{
					AddStudent(item.Student);
					AddCategory(item.Category);
				}
			}
		}

		void Evaluation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

	   

		void Students_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//if some items have been removed or replaced
			if (e.Action == NotifyCollectionChangedAction.Replace ||
				e.Action == NotifyCollectionChangedAction.Remove)
			{
				//Remove their evaluations
				foreach (Student item in e.OldItems)
				{
					RemoveEvaluations(item.Evaluations);
				}
			}

			//if some items have been added or replaced
			if (e.Action == NotifyCollectionChangedAction.Replace ||
				e.Action == NotifyCollectionChangedAction.Add)
			{
				//Add their evaluations
				foreach (Student item in e.NewItems)
				{
					AddEvaluations(item.Evaluations);
				}
			}

			//collection has been changed significantly
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				//remove any evaluation with the category not included in the collection
				var oldItems = new ObservableCollection<Evaluation>();
				var newItems = new ObservableCollection<Evaluation>();
				foreach (var item in this.Evaluations)
				{
					if (item.Student != null)
					{
						if (this.Students.Contains(item.Student))
							newItems.Add(item); //this is new item
						else
							oldItems.Add(item);  //old items
					}
				}

				AddEvaluations(new ReadOnlyObservableCollection<Evaluation>(newItems));
				RemoveEvaluations(new ReadOnlyObservableCollection<Evaluation>(oldItems));
			}
		}

		

		private void AddStudent(Student student)
		{
			if (student != null && this.Students.Contains(student) == false)
			{
				this.Students.Add(student); //will trigger AddCategoryEvaluations
			}
		}

		private void AddCategory(Category category)
		{
			if (category != null && this.Categories.Contains(category) == false)
			{
				this.Categories.Add(category); //will trigger AddCategoryEvaluations
			}
		}

		

		


		
#endif

	}
}
