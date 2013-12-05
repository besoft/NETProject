using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.DialogService;
using Zcu.StudentEvaluator.View;
using System.Diagnostics.CodeAnalysis;

namespace StudentEvaluatorCoreUnitTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class BaseUnitTest
	{
		private static ConfirmationView _confirmView = new ConfirmationView();
		private static NotificationView _notifyView = new NotificationView();

		public ConfirmationView ConfirmView { get { return _confirmView; } }
		public NotificationView NotifyView { get { return _notifyView; } }
		

		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			DialogService.Default.RegisterSingleton<IConfirmationView, ConfirmationView>(_confirmView, DialogConstants.ConfirmationView);
			DialogService.Default.RegisterSingleton<INotificationView, NotificationView>(_notifyView, DialogConstants.NotificationView);
		}		
	}
}
