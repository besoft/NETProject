using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// This ViewModel represents data of one particular student.
	/// </summary>	
	public class StudentListItemViewModel : StudentViewModel, IStudentListItemViewModel
	{		
		/// <summary>
		/// Gets the  repository of the model (student).
		/// </summary>		
		protected IStudentEvaluationUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentViewModel" /> class with a new model.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
		public StudentListItemViewModel(IStudentEvaluationUnitOfWork unitOfWork = null) : base()
		{			
			this.UnitOfWork = unitOfWork;
			if (this.UnitOfWork != null)
				this.ModelRepository = this.UnitOfWork.Students;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentListItemViewModel" /> class.
		/// </summary>
		/// <param name="model">The model to be wrapped.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <exception cref="System.ArgumentNullException">student cannot be null </exception>
		public StudentListItemViewModel(Student model, IStudentEvaluationUnitOfWork unitOfWork = null) 
			: base(model)
		{
			this.UnitOfWork = unitOfWork;
			if (this.UnitOfWork != null)
				this.ModelRepository = this.UnitOfWork.Students;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentViewModel" /> class.
        /// </summary>
        /// <param name="model">The model to be wrapped.</param>
        /// <param name="modelState">State of the model.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <exception cref="System.ArgumentNullException">model cannot be null</exception>
		protected StudentListItemViewModel(Student model, ModelStates modelState, IStudentEvaluationUnitOfWork unitOfWork = null) 
			: base(model, modelState)
		{
			this.UnitOfWork = unitOfWork;
			if (this.UnitOfWork != null)
				this.ModelRepository = this.UnitOfWork.Students;
		}		

		#region Derived Model Properties

		/// <summary>
		/// Gets the evaluations (ViewModels) of this Student View Model.
		/// </summary>
		public ObservableCollection<EvaluationViewModel> Evaluations 
		{
			get
			{
				return GetModelDerivedPropertyValue<ObservableCollection<EvaluationViewModel>>(
					defaultSelector: () => ConstructEvaluationCollection());				
			}
		}				

		/// <summary>
		/// Gets the total points the student obtained.
		/// </summary>
		public decimal? TotalPoints
		{
			get
			{
				//return the cached value whenever possible
				return GetModelDerivedPropertyValue<decimal?>(
					defaultSelector: () => this.Evaluations.Sum(x => x.ValidPoints));				
			}			
		}

		///<summary>
		/// Gets the reason for the total points given
		/// </summary>		
		public string TotalPointsReason
		{
			get
			{
				return GetModelDerivedPropertyValue<string>(
					defaultSelector: () =>
				{
					var sb = new StringBuilder();
					foreach (var item in this.Evaluations)
					{
						//TODO: zakomentovat tento test a ukazat moznosti Diggeru
						if (item == null)
							continue;

						sb.AppendFormat("{0}\n", item.ValidPointsReason);
					}

					return sb.ToString();
				});					
			}			
		}

		/// <summary>
		/// Gets a value indicating whether the student has passed
		/// </summary>
		public bool HasPassed 
		{
			get
			{
				return GetModelDerivedPropertyValue<bool>(
					defaultSelector: () => this.Evaluations.All(x => x.HasPassed));
			}
		}
		
		#region Evaluation Collections Changes
		/// <summary>
		/// Constructs the ViewModel evaluation collection for Model evaluation collection.
		/// </summary>
		/// <remarks>Upon construction, the object registers itself to listen notify changes of evaluations</remarks>
		/// <returns>The constructed collection</returns>
		protected virtual ObservableCollection<EvaluationViewModel> ConstructEvaluationCollection()
		{
			var evaluations = new ObservableCollection<EvaluationViewModel>();
			
			foreach (var item in this.Model.Evaluations)	//lazy loading
			{
				//create a wrapper
				var evaluation = new EvaluationViewModel(item, this.UnitOfWork != null ? this.UnitOfWork.Evaluations : null);
				evaluation.PropertyChanged += OnEvaluationPropertyChanged;	//register us to Notify
				evaluations.Add(evaluation);
			}

			evaluations.CollectionChanged += OnEvaluationCollectionChange;
			return evaluations;
		}

		/// <summary>
		/// Called when the evaluation collection change.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		private void OnEvaluationCollectionChange(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//reset aggregation values
			this.RemoveModelDerivedPropertyCacheEntry(() => this.HasPassed);
			this.RemoveModelDerivedPropertyCacheEntry(() => this.TotalPoints);
			this.RemoveModelDerivedPropertyCacheEntry(() => this.TotalPointsReason);

			if (e.OldItems != null)
			{
				foreach (EvaluationViewModel item in e.OldItems)
				{
					item.PropertyChanged -= OnEvaluationPropertyChanged;
				}
			}

			if (e.NewItems != null)
			{
				foreach (EvaluationViewModel item in e.NewItems)
				{
					item.PropertyChanged += OnEvaluationPropertyChanged;
				}
			}
		}

		/// <summary>
		/// Called when some property of evaluation ViewModel changed.
		/// </summary>
		/// <param name="sender">The sender (evaluation ViewModel).</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>		
		private void OnEvaluationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var ev = sender as EvaluationViewModel;

			// Make sure that the property name we're referencing is valid.
			// This is a debugging technique, and does not execute in a Release build.
			ev.VerifyPropertyName(e.PropertyName);

			// When ValidPoints has changed, we need to invalidate TotalPoints
			// so that it will be queried again for a new value.			
			if (e.PropertyName == GetPropertyName(() => ev.ValidPoints))
				this.RemoveModelDerivedPropertyCacheEntry(() => this.TotalPoints);
			else if (e.PropertyName == GetPropertyName(() => ev.ValidPointsReason))
				this.RemoveModelDerivedPropertyCacheEntry(() => this.TotalPointsReason);
			else if (e.PropertyName == GetPropertyName(() => ev.HasPassed))
				this.RemoveModelDerivedPropertyCacheEntry(() => this.HasPassed);
		}
		#endregion
		#endregion		
	}
}
