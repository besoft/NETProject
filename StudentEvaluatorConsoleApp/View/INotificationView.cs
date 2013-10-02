using System;
namespace Zcu.StudentEvaluator.View
{
	/// <summary>
	/// Enumeration  of possible notifications.
	/// </summary>
	public enum NotificationType
	{
		Message,
		Warning,
		Error,
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
