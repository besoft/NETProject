using System;
using System.Collections.ObjectModel;
namespace Zcu.StudentEvaluator.ViewModel
{
    /// <summary>
    /// This interface represents information of a student to be presented in lists of students.
    /// </summary>
	public interface IStudentListItemViewModel : IStudentViewModel, ISelectableViewModel
	{
		/// <summary>
		/// Gets the evaluations (ViewModels) of this Student View Model.
		/// </summary>
		ObservableCollection<EvaluationViewModel> Evaluations { get; }
		
		/// <summary>
		/// Gets the total points the student obtained.
		/// </summary>
		decimal? TotalPoints { get; }

		///<summary>
		/// Gets the reason for the total points given
		/// </summary>		
		string TotalPointsReason { get; }

		/// <summary>
		/// Gets a value indicating whether the student has passed
		/// </summary>
		bool HasPassed { get; }
	}
}
