using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.View;

namespace Zcu.StudentEvaluator.ViewModel
{
	public class StudentViewModel : IStudentViewModel
	{		
		protected IStudentView _mainView;
		protected IConfirmationView _confirmView;
		protected INotificationView _notifyView;

		protected IStudentEvaluationUnitOfWork _unitOfWork;


		/// <summary>
		/// The student that is currently being edited.
		/// </summary>
		protected Student _currentStudent;

		/// <summary>
		/// true, if the student that is currently being edited is completely new student.
		/// </summary>
		protected bool _currentStudentIsNew;


		/// <summary>
		/// Initializes a new instance of the <see cref="StudentViewModel" /> class.
		/// </summary>
		/// <param name="mainView">The main view.</param>
		/// <param name="confirmView">The view for confirmation requests.</param>
		/// <param name="notifyView">The view for notifications.</param>
		/// <param name="repository">The repository used by this ViewModel.</param>
		/// <remarks>Typical concrete implementations of IStudentView implements also both IConfirmationView and INotificationView</remarks>
		public StudentViewModel(IStudentView mainView, 
			IConfirmationView confirmView, INotificationView notifyView,
			IStudentEvaluationUnitOfWork unitOfWork)
		{
			this._mainView = mainView;
			this._confirmView = confirmView;
			this._notifyView = notifyView;
			this._unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Displays the list of students.
		/// </summary>		
		public void DisplayList()
		{
			this._mainView.Display(this._unitOfWork.Students.Get(includeProperties: new string[]{"Evaluations"}));
		}

		private Random _rndPersonalNumber = new Random(Environment.TickCount);

		/// <summary>
		/// Create a new student.
		/// </summary>		
		public void Create()
		{
			if (this._currentStudent != null)
				AcceptChanges();	//automatically accept changes of the current student
			
			this._currentStudent = new Student
			{
				PersonalNumber = "#rnd_" + _rndPersonalNumber.Next().ToString(),
				FirstName = "Enter first name",
				Surname = "Enter surname",
			};

			this._currentStudentIsNew = true;

			this._mainView.Display(this._currentStudent, true);
		}

		/// <summary>
		/// Display the detail of a student.
		/// </summary>
		/// <param name="personalNumber">The personal number of student.</param>
		public void DisplayDetail(string personalNumber)
		{
			if (this._currentStudent != null)
				AcceptChanges();	//automatically accept changes of the current student

			var st = this._unitOfWork.Students.Get(x => x.PersonalNumber == personalNumber).SingleOrDefault(); 
			if (st == null)
				this._notifyView.DisplayNotification(NotificationType.Error, "Student not found",
					"Student with personal number '" + personalNumber + "' could not be found in the repository.");
			else
				this._mainView.Display(st, false);			
		}

		/// <summary>
		/// Edits the detail.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		public void EditDetail(string personalNumber)
		{
			if (this._currentStudent != null)
				AcceptChanges();	//automatically accept changes of the current student

			this._currentStudent = this._unitOfWork.Students.Get(x => x.PersonalNumber == personalNumber).SingleOrDefault();
			if (this._currentStudent == null)
				this._notifyView.DisplayNotification(NotificationType.Error, "Student not found",
					"Student with personal number '" + personalNumber + "' could not be found in the repository.");
			else
				this._mainView.Display(this._currentStudent, true);
		}

		/// <summary>
		/// Deletes the specified personal number.
		/// </summary>
		/// <param name="personalNumber">The personal number.</param>
		public void Delete(string personalNumber)
		{
			var st = this._unitOfWork.Students.Get(x => x.PersonalNumber == personalNumber).SingleOrDefault();
			if (st == null)
				this._notifyView.DisplayNotification(NotificationType.Error, "Student not found",
					"Student with personal number '" + personalNumber + "' could not be found in the repository.");
			else
			{
				if (this._confirmView.ConfirmAction(ConfirmationOptions.YesNo, "Student is to be deleted",
					"Do you really want to remove the student with personal number '"
					+ st.PersonalNumber + "' from the repository?") == ConfirmationResult.Yes)
				{
					this._unitOfWork.Students.Delete(st);
					this._notifyView.DisplayNotification(NotificationType.Message, "Student deleted",
						"Student with personal number '" + personalNumber + "' has been removed from the repository.");
				}
			}
		}

		/// <summary>
		/// Accepts the changes done to the currently edited student (must be called after Create or EditDetail).
		/// </summary>
		public void AcceptChanges()
		{
			//TODO: handle exceptions and concurrency problems

			if (this._currentStudentIsNew)
				this._unitOfWork.Students.Insert(this._currentStudent);
			else
				this._unitOfWork.Students.Update(this._currentStudent);

			if (this._currentStudentIsNew)
				this._notifyView.DisplayNotification(NotificationType.Message,
					"Student created", "A new student with personal number '" + this._currentStudent.PersonalNumber + "' has been added into the repository.");
			else
				this._notifyView.DisplayNotification(NotificationType.Message,
					"Student updated", "Student with personal number '" + this._currentStudent.PersonalNumber + "' has been updated.");

			this._unitOfWork.Save();	//save all data

			this._currentStudent = null;
			this._currentStudentIsNew = false;
		}

		/// <summary>
		/// Cancel the changes done to the currently edited student (must be called after Create or EditDetail).
		/// </summary>
		public void CancelChanges()
		{			
			if (!this._currentStudentIsNew)
			{
				try
				{
					//we must reset the state
					this._unitOfWork.Students.Reset(this._currentStudent);
					this._unitOfWork.Save();
				}
				catch (Exception e)
				{
					this._notifyView.DisplayNotification(NotificationType.Warning, "CancelChanges failed.",
						"Resetting the current student into the original state finished with an exception", e);
				}
			}

			this._currentStudent = null;
			this._currentStudentIsNew = false;
		}

		/// <summary>
		/// Exit the application.
		/// </summary>		
		public void Exit()
		{
			if (this._currentStudent != null)
				AcceptChanges();	//automatically accept changes

			this._mainView.Close();			
		}		
	}
}
