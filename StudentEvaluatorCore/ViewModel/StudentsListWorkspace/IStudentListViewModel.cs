
using System.Windows.Input;
namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// ViewModel / Controller for manipulation with students.
	/// </summary>
	public interface IStudentListViewModel : ISelectableListViewModel<StudentListItemViewModel>, IEditableViewModel
	{
		/// <summary>
		/// Gets the refresh list command that can be used to refresh the content of the list.
		/// </summary>
		ICommand RefreshListCommand { get; }

		/// <summary>
		/// Gets the number of students in the list.
		/// </summary>
		int AllStudentsCount { get;  }

		/// <summary>
		/// Gets the number of students in the list with HasPassed.
		/// </summary>
		int HasPassedStudentsCount { get; }
	}
}
