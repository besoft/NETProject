using System.Windows.Input;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.View;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// This ViewModel represents data of one particular student.
	/// </summary>	
	public class StudentViewModel : ViewModel<Student>,
		IEditableViewModel, ISelectableViewModel, IStudentViewModel
	{
		#region Fields
		private bool _isFocused = false;	//currently not focused
		private bool _isSelected = false;	//currently not selected

		private ICommand _editCommand;		//definition of the command to edit the student personal data
		private ICommand _saveCommand;		//definition of the command to accept changes
		private ICommand _cancelCommand;	//definition of the command to cancel changes
		private ICommand _deleteCommand;	//definition of the command to delete the student (from model)		
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="StudentViewModel" /> class with a new model.
		/// </summary>
		/// <param name="modelRepository">The model repository.</param>
		public StudentViewModel(IRepository<Student> modelRepository = null)
			: base(modelRepository)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentViewModel"/> class.
		/// </summary>
		/// <param name="model">The model to be wrapped.</param>
		/// <param name="modelRepository">The model repository.</param>
		/// <remarks>Model state is supposed to be Unchanged.</remarks>
		/// <exception cref="System.ArgumentNullException">model cannot be null</exception>
		public StudentViewModel(Student model, IRepository<Student> modelRepository = null)
			: base(model, modelRepository)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentViewModel"/> class.
		/// </summary>
		/// <param name="model">The model to be wrapped.</param>
		/// <param name="modelRepository">The model repository.</param>
		/// <param name="modelState">State of the model.</param>
		/// <exception cref="System.ArgumentNullException">model cannot be null</exception>
		protected StudentViewModel(Student model, ModelStates modelState, IRepository<Student> modelRepository = null)
			: base(model, modelState, modelRepository)
		{

		}
		#endregion // Constructor

		#region Model Properties
		/// <summary>
		/// Gets or sets the personal number of the student.
		/// </summary>
		/// <value>
		/// The personal number, e.g. A12B0012P.
		/// </value>			
		public string PersonalNumber
		{
			get
			{
				return GetModelPropertyValue<string>();
			}

			set
			{
				if (SetModelPropertyValue(value: value))
					NotifyPropertyChanged(() => this.DisplayName);
			}
		}

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name, e.g., "Josef".
		/// </value>		
		public string FirstName
		{
			get
			{
				return GetModelPropertyValue<string>();
			}
			set
			{
				if (SetModelPropertyValue(value: value))
				{
					NotifyPropertyChanged(() => this.DisplayName);
					NotifyPropertyChanged(() => this.FullName);
				}
			}
		}

		/// <summary>
		/// Gets or sets the surname.
		/// </summary>
		/// <value>
		/// The surname, e.g., "Kohout".
		/// </value>		
		public string Surname
		{
			get
			{
				return GetModelPropertyValue<string>();
			}
			set
			{
				if (SetModelPropertyValue(value: value))
				{
					NotifyPropertyChanged(() => this.DisplayName);
					NotifyPropertyChanged(() => this.FullName);
				}
			}
		}
		#endregion

		#region Derived Model Properties
		/// <summary>
		/// Gets the full name of the student.
		/// </summary>
		/// <value>
		/// The full name.
		/// </value>
		public string FullName
		{
			get
			{
				//TODO: odkomentovat puvodni kod a ukazat moznosti Diggeru
				//return Surname.ToUpper() + " " + FirstName;
				if (this.Surname != null)
				{
					return (this.FirstName != null) ? Surname.ToUpper() + " " + FirstName : Surname.ToUpper();
				}
				else if (this.FirstName != null)
				{
					return FirstName;
				}
				else
					return null;
			}
		}
		#endregion

		#region Presentation Properties
		/// <summary>
		/// Returns the user-friendly name of this object.
		/// Child classes can set this property to a new value,
		/// or override it to determine the value on-demand.
		/// </summary>
		public override string DisplayName
		{
			get
			{
				return (this.PersonalNumber == null) ? this.FullName :
					this.FullName + " (" + this.PersonalNumber + ")";
			}
		}

		/// <summary>
		/// Gets/sets whether this customer is selected in the UI.
		/// </summary>
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (value == _isSelected)
					return;

				_isSelected = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets/sets whether this customer is focused in the UI.
		/// </summary>
		public bool IsFocused
		{
			get { return _isFocused; }
			set
			{
				if (value == _isFocused)
					return;

				_isFocused = value;
				NotifyPropertyChanged();
			}
		}
		#endregion

		#region Commands
		/// <summary>
		/// Gets the Create command.
		/// </summary>
		/// <exception cref="System.NotImplementedException">Not implemented by this class.</exception>
		/// <remarks>
		/// Supports creating a new model. The model is set to be the current one and marked as being edited.
		/// </remarks>
		public ICommand CreateCommand
		{
			get { throw new System.NotImplementedException(); }
		}

		/// <summary>
		/// Gets the edit command.
		/// </summary>		
		/// <remarks>When executed, the ViewModel is set to EditMode, which allows editing properties of the model.
		/// View is notified about this change through the change of IsReadOnly property.</remarks>
		public ICommand EditCommand
		{
			get
			{
				if (_editCommand == null)
					_editCommand = new RelayCommand(
						execute: param => EditStudent(),
						canExecute: param => CanEditStudent()
						);

				return _editCommand;
			}
		}
		
		/// <summary>
		/// Gets the save command.
		/// </summary>		
		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
					_saveCommand = new RelayCommand(
						execute: param => SaveChanges(),
						canExecute: param => CanSaveChanges()
						);

				return _saveCommand;
			}
		}

		/// <summary>
		/// Gets the cancel command.
		/// </summary>		
		public ICommand CancelCommand
		{
			get
			{
				if (_cancelCommand == null)
					_cancelCommand = new RelayCommand(
						execute: param => CancelChanges(),
						canExecute: param => CanCancelChanges()
						);

				return _cancelCommand;
			}
		}

		/// <summary>
		/// Gets the delete command.
		/// </summary>		
		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(
						execute: param => DeleteStudent(),
						canExecute: param => CanDeleteStudent()
						);

				return _deleteCommand;
			}
		}
		#endregion

		#region BusinessLogic
		/// <summary>
		/// Determines whether the student can be deleted in the current context.
		/// </summary>
		/// <returns>true, if the student can be delete, false otherwise</returns>		
		virtual protected bool CanEditStudent()
		{
			//Student can be deleted only if it is not currently being edited and has not been already deleted
			return this.IsReadOnly && !this.IsModelDeleted;
		}

		/// <summary>
		/// Determines whether changes can be saved.
		/// </summary>
		/// <returns>true, if SaveChanges can be executed, false otherwise</returns>
		virtual protected bool CanSaveChanges()
		{
			//Changes can be saved only if a) object is in edit mode, 
			//b) there are some changes and c) the object changes are valid
			return !this.IsReadOnly  && this.IsModelDirty && this.IsModelValid;
		}

		/// <summary>
		/// Determines whether changes can be cancelled in the current context.
		/// </summary>
		/// <returns>true, if CancelChanges can be executed, false otherwise</returns>
		virtual protected bool CanCancelChanges()
		{
			return !this.IsReadOnly && this.IsModelDirty;
		}

		/// <summary>
		/// Determines whether the student can be deleted in the current context.
		/// </summary>
		/// <returns>true, if the student can be delete, false otherwise</returns>		
		virtual protected bool CanDeleteStudent()
		{
			//Student can be deleted only if it is not currently being edited, i.e., if the student is valid and exists in the repository
			return this.IsReadOnly && !this.IsModelDeleted;
		}

		/// <summary>
		/// Switch to edit mode
		/// </summary>		
		virtual protected void EditStudent()
		{
			this.IsReadOnly = false;
		}

		/// <summary>
		/// Deletes the student.
		/// </summary>		
		virtual protected void DeleteStudent()
		{			
			var personalNumber = this.PersonalNumber ?? "<null>";

			if (DeleteModel(confQuestion:
				"Do you really want to remove the student with personal number '"
					+ personalNumber + "' from the repository?")
				)
			{
				DisplayNotification(NotificationType.Message, "Student deleted",
						"Student with personal number '" + personalNumber + "' has been removed from the repository.");
			}
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>		
		virtual protected void SaveChanges()
		{		
			bool isNew = this.IsModelNew;
			if (SaveModelChanges())
			{
				if (isNew)
					DisplayNotification(NotificationType.Message,
						"Student created", "A new student with personal number '" + this.PersonalNumber + "' has been added into the repository.");
				else
					DisplayNotification(NotificationType.Message,
						"Student updated", "Student with personal number '" + this.PersonalNumber + "' has been updated.");

				this.IsReadOnly = true;
			}
		}

		/// <summary>
		/// Cancel the changes done to the currently edited student (must be called after EditStudent).
		/// </summary>
		virtual protected void CancelChanges()
		{
			if (CancelModelChanges())
				this.IsReadOnly = true;			
		}

		
		#endregion			
	}
}
