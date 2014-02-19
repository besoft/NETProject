using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zcu.StudentEvaluator.View
{
	/// <summary>
	/// Represents a generic View
	/// </summary>
	public interface IWindowView
	{
		/// <summary>
		/// Gets or sets the data context.
		/// </summary>
		/// <value>
		/// The data context implementing IViewModel and other interfaces.
		/// </value>
		object DataContext {get; set; }

		/// <summary>
		/// Opens a window and returns without waiting for the newly opened window to close.
		/// </summary>
		void Show();

		/// <summary>
		/// Manually closes a Window.
		/// </summary>
		void Close();

		/// <summary>
		/// Gets or sets the dialog result value, which is the value that is returned from the ShowDialog method.
		/// </summary>
		/// <value>
        /// A <see cref="Nullable{T}"/> value of type <see cref="Boolean"/>. The default is false.
		/// </value>
		bool? DialogResult { get; set; }

		/// <summary>
		/// Opens a window and returns only when the newly opened window is closed.
		/// </summary>
        /// <returns>A <see cref="Nullable{T}"/> value of type <see cref="Boolean"/> that specifies whether the activity was accepted (true) or cancelled (false). 
		/// The return value is the value of the DialogResult property before a window closes.</returns>
		bool? ShowDialog();
	}
}
