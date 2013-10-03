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
			this._mainView.Display(this._unitOfWork.Students.Get());
		}

		private Random _rndPersonalNumber = new Random(Environment.TickCount);

		/// <summary>
		/// Create a new student.
		/// </summary>		
		public void Create()
		{
			Student st = new Student
			{
				PersonalNumber = "#rnd_" + _rndPersonalNumber.Next().ToString(),
				FirstName = "Enter first name",
				Surname = "Enter surname",
			};

			this._unitOfWork.Students.Insert(st);
			this._notifyView.DisplayNotification(NotificationType.Message,
				"Student created",
				"A new student with random personal number '" + st.PersonalNumber + "' has been added into the repository.");
		}

		/// <summary>
		/// Display the detail of a student.
		/// </summary>
		/// <param name="personalNumber">The personal number of student.</param>
		public void DisplayDetail(string personalNumber)
		{
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
			var st = this._unitOfWork.Students.Get(x => x.PersonalNumber == personalNumber).SingleOrDefault(); 
			if (st == null)
				this._notifyView.DisplayNotification(NotificationType.Error, "Student not found",
					"Student with personal number '" + personalNumber + "' could not be found in the repository.");
			else
				this._mainView.Display(st, true);
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
		/// Exit the application.
		/// </summary>		
		public void Exit()
		{
			var result = this._confirmView.ConfirmAction(
				ConfirmationOptions.YesNoCancel, "Application is to be closed", 
				"Do you want to save all changes?");

			if (result == ConfirmationResult.Yes)
			{
				this._unitOfWork.Save();
			}

			if (result != ConfirmationResult.Cancel)
			{
				this._mainView.Close();	
			}			
		}
	}
}
