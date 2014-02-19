using System;
using System.Runtime.CompilerServices;
namespace Zcu.StudentEvaluator.View
{
    /// <summary>
    /// This namespace contains classes/interfaces implementing Views.
    /// </summary>       
    /// <remarks>A View represents the current state of the ViewModel associated with this View
    /// using various visual controls, i.e., it actually represents GUI, e.g., a dialog or form. 
    /// </remarks>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
        //Trick to document a namespace
    }

	/// <summary>
	/// Enumeration  of possible notifications.
	/// </summary>
	public enum NotificationType
	{
        /// <summary>
        /// The notification has only informational character (message).
        /// </summary>
		Message,

        /// <summary>
        /// The notification is a warning.
        /// </summary>
		Warning,

        /// <summary>
        /// The notification is an error. Something has gone wrong.
        /// </summary>
		Error,

        /// <summary>
        /// The notification is a severe error that might lead to application crash.
        /// </summary>
		FatalError,
	}

	/// <summary>
	/// This represents a notification system for notifying the user of any change in the application, e.g., "The requested item does not exist".
	/// </summary>
	public interface INotificationView
	{
		/// <summary>
		/// Displays the notification message to the user.
		/// </summary>
		/// <param name="type">The type of the notification.</param>
		/// <param name="caption">The caption of the message, i.e., this is a short summary of what has happened.</param>
		/// <param name="message">The message to be displayed containing the detailed explanation of what has happened.</param>
		/// <param name="exc">The exception containing all the details (may be null).</param>
		void DisplayNotification(NotificationType type, string caption,	string message, Exception exc = null);		
	}
}
