using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.View;

namespace StudentEvaluatorCoreUnitTests
{
	[ExcludeFromCodeCoverage]
	public class NotificationView : INotificationView
	{
		public void DisplayNotification(NotificationType type, string caption, string message, Exception exc = null)
		{
			Debug.WriteLine("NOTIFICATION REQUEST: {0}-{1} ({2})", Enum.GetName(type.GetType(), type), caption, message);
			if (exc != null)
			{
				Debug.WriteLine(exc);
			}
		}
	}
}
