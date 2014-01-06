using System;

namespace Zcu.StudentEvaluator.View
{
	public class NotificationView : INotificationView
	{
		#region INotificationView
		/// <summary>
		/// Displays the notification message to the user.
		/// </summary>
		/// <param name="type">The type of the notification.</param>
		/// <param name="caption">The caption of the message, i.e., this is a short summary of what has happened.</param>
		/// <param name="message">The message to be displayed containing the detailed explanation of what has happened.</param>
		/// <param name="exc">The exception containing all the details (may be null).</param>
		public void DisplayNotification(NotificationType type, string caption, string message, Exception exc = null)
		{
			var oldColor = Console.ForegroundColor;
			switch (type)
			{
				case NotificationType.Message:
					Console.ForegroundColor = Properties.ColorSettings.Default.MessageColor; break;
				case NotificationType.Warning:
					Console.ForegroundColor = Properties.ColorSettings.Default.WarningColor; break;
				default:
					Console.ForegroundColor = Properties.ColorSettings.Default.ErrorColor; break;
			}

			Console.WriteLine("{0} : {1}\n\n{2}", type.ToString().ToUpper(), caption, message);

			if (exc != null)
			{
				Console.WriteLine("\nException:");
				Console.WriteLine(exc.ToString());
			}

			Console.WriteLine();
			Console.ForegroundColor = oldColor;
		}
		#endregion
	}
}
