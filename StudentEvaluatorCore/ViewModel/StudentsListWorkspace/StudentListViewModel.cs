using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.View;
using System.Windows.Input;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Represents an observable collection of StudentListItemViewModels with associated commands
	/// </summary>
	public class StudentListViewModel : ViewModelBase, IStudentListViewModel				
	{
		#region Fields
		private ObservableCollection<StudentListItemViewModel> _items;	//All students
		
		private ICommand _createCommand;	//definition of the command to create new student
		private ICommand _editCommand;		//definition of the command to edit the currently focused student personal data
		private ICommand _deleteCommand;	//definition of the command to delete the currently selected students
		private ICommand _refreshCommand;		//definition of the command to refresh the list
		#endregion

		#region IListViewModel
		/// <summary>
		/// Gets the items in the collection (list).
		/// </summary>
		public ObservableCollection<StudentListItemViewModel> Items
		{
			get
			{
				return _items;
			}
			protected set
			{
				if (_items == value)
					return;		//no change

				//unregister from the original list
				if (_items != null)
				{
					foreach (var item in _items.ToList())
					{
						item.PropertyChanged -= Items_PropertyChanged;
					}

					_items.CollectionChanged -= Items_CollectionChanged;
				}

				_items = value;

				//register to the new list
				if (_items != null)
				{
					_items.CollectionChanged += Items_CollectionChanged;

					foreach (var item in _items.ToList())
					{
						item.PropertyChanged += Items_PropertyChanged;
					}
				}

				NotifyPropertyChanged();				
				NotifyPropertyChanged(() => FocusedItem);
				NotifyPropertyChanged(() => SelectedItems);

				NotifyPropertyChanged(() => AllStudentsCount);
				NotifyPropertyChanged(() => HasPassedStudentsCount);
			}
		}
		#endregion

		#region ISelectableListViewModel
		/// <summary>
		/// Gets the selected items in the collection (list).
		/// </summary>
		public ObservableCollection<StudentListItemViewModel> SelectedItems
		{
			get
			{
				return GetModelDerivedPropertyValue(
					defaultSelector:
						() => new ObservableCollection<StudentListItemViewModel>
							(this.Items.Where(x => x.IsSelected))
					);
			}
		}

		/// <summary>
		/// Gets the currently focused item in the collection (list).
		/// </summary>
		public StudentListItemViewModel FocusedItem
		{
			get
			{
				return GetModelDerivedPropertyValue(
					defaultSelector:
						() => this.Items.FirstOrDefault(x => x.IsFocused)
						);
			}
		} 
		#endregion

		#region IStudentListViewModel
		/// <summary>
		/// Gets the number of students in the list.
		/// </summary>
		public int AllStudentsCount
		{
			get { return this.Items.Count; }
		}

		/// <summary>
		/// Gets the number of students in the list with HasPassed.
		/// </summary>		
		public int HasPassedStudentsCount
		{
			get { 
				return GetModelDerivedPropertyValue(
					defaultSelector:
					() => this.Items.Count(x => x.HasPassed)
				); 
			}
		}
		#endregion

		/// <summary>
		/// Gets the unit of work (with all repositories).
		/// </summary>
		protected IStudentEvaluationUnitOfWork UnitOfWork { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentListViewModel"/> class.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		public StudentListViewModel(IStudentEvaluationUnitOfWork unitOfWork = null)
		{
			this.UnitOfWork = unitOfWork;
			base.DisplayName = "Students";
						
			//populate the list of students
			this.PopulateList();
		}		

		#region Commands
		/// <summary>
		/// Gets the Create command.
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		/// <remarks>
		/// Supports creating a new model. The model is set to be the current one and marked as being edited.
		/// </remarks>
		public ICommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
					_createCommand = new RelayCommand(
						execute: param => CreateNewStudent(),
						canExecute: param => CanCreateNewStudent()
						);

				return _createCommand;
			}

		}

		/// <summary>
		/// Gets the edit command.
		/// </summary>		
		/// <remarks>
		/// Supports editing the current model. The changes are typically not stored until SaveCommand is executed.
		/// </remarks>
		public ICommand EditCommand
		{
			get
			{
				if (_editCommand == null)
					_editCommand = new RelayCommand(
						execute: param => EditFocusedStudent(),
						canExecute: param => CanEditFocusedStudent()
						);

				return _editCommand;
			}
		}

		/// <summary>
		/// Gets the save command.
		/// </summary>
		/// <exception cref="System.NotImplementedException">Not available</exception>
		/// <remarks>
		/// Supports saving the changes done to the current model into the repository.
		/// </remarks>
		public ICommand SaveCommand
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the cancel command.
		/// </summary>
		/// <exception cref="System.NotImplementedException">Not available</exception>
		/// <remarks>
		/// Supports cancelling the changes done to the current model.
		/// </remarks>
		public ICommand CancelCommand
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the delete command.
		/// </summary>		
		/// <remarks>
		/// Supports deleting the current model from the repository.
		/// </remarks>
		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(
						execute: param => DeleteSelectedStudents(),
						canExecute: param => CanDeleteSelectedStudents()
						);

				return _deleteCommand;
			}
		}

		/// <summary>
		/// Gets the refresh list command that can be used to refresh the content of the list.
		/// </summary>
		public ICommand RefreshListCommand 
		{
			get
			{
				if (_refreshCommand == null)
					_refreshCommand = new RelayCommand(
						execute: param => RefreshList()						
						);

				return _refreshCommand;
			}
		}
		#endregion

		#region Commands Business Logic
		/// <summary>
		/// Determines whether a new student can be created in the current context.
		/// </summary>
		/// <returns>true, if a new student can be added, false otherwise</returns>		
		virtual protected bool CanCreateNewStudent()
		{
			//Students can be added only if the list is not ReadOnly
			return !this.IsReadOnly;
		}

		/// <summary>
		/// Determines whether the student can be edited in the current context.
		/// </summary>
		/// <returns>true, if the currently focused student can be deleted, false otherwise</returns>		
		virtual protected bool CanEditFocusedStudent()
		{
			//Students can be edited only if the list is not ReadOnly and there is some Focused student
			return !this.IsReadOnly && this.FocusedItem != null;
		}

		/// <summary>
		/// Determines whether the student can be deleted in the current context.
		/// </summary>
		/// <returns>true, if the selected students can be deleted, false otherwise</returns>		
		virtual protected bool CanDeleteSelectedStudents()
		{
			//Students can be deleted only if the list is not ReadOnly and there are some selected students
			return !this.IsReadOnly && this.SelectedItems.Count != 0;
		}

		/// <summary>
		/// Create a new student
		/// </summary>		
		virtual protected void CreateNewStudent()
		{
			var window = DialogService.DialogService.Default.Get<IWindowView>(DialogService.DialogConstants.EditStudentView);
			if (window == null)
			{
				Debug.Fail("Unable to retrieve EditStudentView");
				return;
			}

			var viewModel = new StudentListItemViewModel(this.UnitOfWork);			
			window.DataContext = viewModel;

			if (window.ShowDialog() == true)
			{
				//the new item has been accepted, so it is time to add the new item into the list
				if (!viewModel.IsModelDeleted)	//unless it was deleted
				{
					this.Items.Add(viewModel);

					viewModel.IsFocused = true;
					this.Items.ToList().ForEach(x => x.IsSelected = false);

					viewModel.IsSelected = true;
				}
			}
		}

		/// <summary>
		/// Edit the currently focused student
		/// </summary>		
		virtual protected void EditFocusedStudent()
		{
			var window = DialogService.DialogService.Default.Get<IWindowView>(DialogService.DialogConstants.EditStudentView);
			if (window == null)
			{
				Debug.Fail("Unable to retrieve EditStudentView");
				return;
			}
			
			var viewModel = this.FocusedItem;
			window.DataContext = viewModel;

			if (window.ShowDialog() == true)
			{
				//if the item has been  deleted, remove it from the list
				if (viewModel.IsModelDeleted)
					this.Items.Remove(viewModel);
			}
		}

		/// <summary>
		///Delete the currently selected students
		/// </summary>		
		virtual protected void DeleteSelectedStudents()
		{			
			ConfirmationResult confResult = ConfirmationResult.Ask;

			var listToDelete = this.SelectedItems.ToList();
			foreach (var item in listToDelete)
			{
				if (!item.DeleteModel(ConfirmationOptions.YesYesoAllNoTNoToAll, 
                    String.Format(Resources.Strings.DeleteSelectedStudents_Confirmation, item.DisplayName), 
					ref confResult) || confResult == ConfirmationResult.NoToAll
					)
				break;	//fatal error or terminated
				
				if (item.IsModelDeleted)	//if the item has been successfully deleted, remove it from the list
					this.Items.Remove(item);
			}			

			//list is automatically refreshed
		}

		#endregion

		/// <summary>
		/// Populates the list with the new data.
		/// </summary>
		protected void PopulateList()
		{
			if (this.UnitOfWork != null)
			{
				this.Items = new ObservableCollection<StudentListItemViewModel>(
				 this.UnitOfWork.Students.Get(includeProperties: new string[] { "Evaluations" })
					.Select(x => new StudentListItemViewModel(x, this.UnitOfWork)));	//wrap Student into the appropriate ViewModel
			}
		}

		/// <summary>
		/// Populates the list with the new data.
		/// </summary>
		protected void RefreshList()
		{
			if (this.UnitOfWork != null)
			{
				this.Items = new ObservableCollection<StudentListItemViewModel>(
				 this.UnitOfWork.Students.Get(includeProperties: new string[] { "Evaluations" }).AsParallel()
					.Select(x => 
						{
							var existing = this.Items.SingleOrDefault(y => y.Id == x.Id);
							return existing ?? new StudentListItemViewModel(x, this.UnitOfWork);
						}
						));	//wrap Student into the appropriate ViewModel
			}
		}

		#region On Items Collection Changed
		/// <summary>
		/// Handles the CollectionChanged event of the Items control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>		
		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				//register ourself as observers of ViewModels of items
				foreach (StudentListItemViewModel custVM in e.NewItems)
					custVM.PropertyChanged += this.Items_PropertyChanged;
			}

			if (e.OldItems != null)
			{
				foreach (StudentListItemViewModel custVM in e.OldItems)
					custVM.PropertyChanged -= this.Items_PropertyChanged;
			}

			NotifyPropertyChanged(() => FocusedItem);
			NotifyPropertyChanged(() => SelectedItems);

			NotifyPropertyChanged(() => AllStudentsCount);
			NotifyPropertyChanged(() => HasPassedStudentsCount);
			
		}

		/// <summary>
		/// Handles the PropertyChanged event of the Items control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void Items_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var it = sender as StudentListItemViewModel;

			// Make sure that the property name we're referencing is valid.
			// This is a debugging technique, and does not execute in a Release build.
			it.VerifyPropertyName(e.PropertyName);

			// When ValidPoints has changed, we need to invalidate TotalPoints
			// so that it will be queried again for a new value.			
			if (e.PropertyName == GetPropertyName(() => it.IsSelected))
				this.RemoveModelDerivedPropertyCacheEntry(() => this.SelectedItems);
			else if (e.PropertyName == GetPropertyName(() => it.IsFocused))
			{
				if (this.FocusedItem != null)
					this.FocusedItem.IsFocused = false;	//only one item can have focus

				this.RemoveModelDerivedPropertyCacheEntry(() => this.FocusedItem);
			}
			else if (e.PropertyName == GetPropertyName(() => it.HasPassed))
				this.RemoveModelDerivedPropertyCacheEntry(() => this.HasPassedStudentsCount);
			else if (e.PropertyName == GetPropertyName(() => it.IsModelDeleted))
			{
				if (it.IsModelDeleted)
					this.Items.Remove(it);	//remove the item from the list
			}
		}
		#endregion				
	}	
}
