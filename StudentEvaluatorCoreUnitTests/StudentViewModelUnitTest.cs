using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.DialogService;
using Zcu.StudentEvaluator.ViewModel;
using Zcu.StudentEvaluator.Model;
using System.Diagnostics.CodeAnalysis;
using Zcu.StudentEvaluator.View;

namespace StudentEvaluatorCoreUnitTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class StudentViewModelUnitTest : BaseUnitTest
	{
		public const string StudentPersonalNumber = "A12B34P";
		public const string StudentFirstName = "Anna";
		public const string StudentSurname = "Nova";

		public static Student CreateStudentTestInstance()
		{
			return new Student()
			{
				PersonalNumber = StudentPersonalNumber,
				FirstName = StudentFirstName,
				Surname = StudentSurname,
			};
		}

		[TestMethod]
		public void TestStudentCtor()
		{
			var st = new Student();

			Assert.IsNull(st.FirstName);
			Assert.IsNull(st.PersonalNumber);
			Assert.IsNull(st.Surname);			

			Assert.IsNotNull(st.Evaluations);

			st = CreateStudentTestInstance();			

			Assert.AreEqual(StudentPersonalNumber, st.PersonalNumber);
			Assert.AreEqual(StudentFirstName, st.FirstName);
			Assert.AreEqual(StudentSurname, st.Surname);			
		}


		[TestMethod]
		public void TestConstructorWithNewModel()
		{
			var vm = new StudentViewModel();
			Assert.IsTrue(vm.IsModelNew);
			Assert.IsFalse(vm.IsReadOnly, "New ViewModel cannot be in ReadOnly");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestConstructorWithNullModel()
		{
			var vm = new StudentViewModel((Student) null);
			Assert.Fail("StudentViewModel should throw an exception when Model is null");
		}


		[TestMethod]		
		public void TestConstructorWithExistingModel()
		{
			var vm = new StudentViewModel(CreateStudentTestInstance());
			Assert.IsFalse(vm.IsModelNew);
			Assert.IsTrue(vm.IsModelValid);
			Assert.IsFalse(vm.IsModelDirty);
		}

		[TestMethod]
		public void TestGetSetNotify()
		{
            int notificationsToSend = 0;
			int notificationsSent = 0;
			string propertyName = "FirstName";

			var vm = new StudentViewModel();
			vm.PropertyChanged += (sender, e) => 
				{
					if (e.PropertyName == propertyName)
						notificationsSent++;
				};

			vm.FirstName = propertyName;
            vm.FirstName = propertyName;    //should be ignored since it is the same as previously
			Assert.AreEqual(++notificationsToSend, notificationsSent, "PropertyChangedNotification not sent for " + propertyName);
			Assert.AreEqual(vm.FirstName, propertyName);
			Assert.IsFalse(vm.IsModelValid);
			Assert.IsTrue(vm.IsModelDirty);
						
			propertyName = "Surname";
			vm.Surname = propertyName;
            vm.Surname = propertyName;    //should be ignored since it is the same as previously
			Assert.AreEqual(++notificationsToSend, notificationsSent, "PropertyChangedNotification not sent for " + propertyName);
			Assert.AreEqual(vm.Surname, propertyName);
			Assert.IsFalse(vm.IsModelValid);
			
			propertyName = "PersonalNumber";
            vm.PersonalNumber = propertyName;
			vm.PersonalNumber = propertyName;//should be ignored since it is the same as previously
			Assert.AreEqual(++notificationsToSend, notificationsSent, "PropertyChangedNotification not sent for " + propertyName);
			Assert.AreEqual(vm.PersonalNumber, propertyName);
			Assert.IsFalse(vm.IsModelValid);

            ++notificationsToSend;
			vm.PersonalNumber = "A12B02P";
			Assert.IsTrue(vm.IsModelValid);
			Assert.IsTrue(vm.IsModelDirty);            


			propertyName = "IsSelected";
			Assert.IsFalse(vm.IsSelected);
			vm.IsSelected = true;
            vm.IsSelected = true; //should be ignored since it is the same as previously
            Assert.AreEqual(++notificationsToSend, notificationsSent, "PropertyChangedNotification not sent for " + propertyName);			
			Assert.IsTrue(vm.IsSelected);
			
			propertyName = "IsFocused";
			Assert.IsFalse(vm.IsFocused);
            vm.IsFocused = true; 
            vm.IsFocused = true; //should be ignored since it is the same as previously
            Assert.AreEqual(++notificationsToSend, notificationsSent, "PropertyChangedNotification not sent for " + propertyName);		
			Assert.IsTrue(vm.IsFocused);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestReadOnlyProtection()
		{
			var vm = new StudentViewModel(CreateStudentTestInstance());
			Assert.IsTrue(vm.IsReadOnly);
			vm.Surname = "A";
			Assert.Fail();			
		}

		[TestMethod]		
		public void TestEditCommand()
		{
			var vm = new StudentViewModel(CreateStudentTestInstance());
			Assert.IsTrue(vm.IsReadOnly);
			Assert.IsNotNull(vm.EditCommand);
			Assert.IsTrue(vm.EditCommand.CanExecute(null));

			vm.EditCommand.Execute(null);
			Assert.IsFalse(vm.IsReadOnly);
			vm.Surname = "A";
		}

		[TestMethod]
		public void TestGetDerivedProperties()
		{
			var vm = new StudentViewModel(CreateStudentTestInstance());
			Assert.AreEqual("NOVA Anna", vm.FullName);

			vm.EditCommand.Execute(null);

			int notificationsToSend = 0;
			int notificationsSent = 0;
			string propertyName = "FullName";
			vm.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == propertyName)
					notificationsSent++;
			};

			vm.FirstName = null;            
			Assert.AreEqual("NOVA", vm.FullName);
			Assert.AreEqual(++notificationsToSend, notificationsSent, 
				"PropertyChangedNotification not sent for " + propertyName);

			vm.Surname = null;            
			Assert.AreEqual(null, vm.FullName);
			Assert.AreEqual(++notificationsToSend, notificationsSent,
				"PropertyChangedNotification not sent for " + propertyName);

			vm.FirstName = StudentFirstName;
			Assert.AreEqual(StudentFirstName, vm.FullName);
			Assert.AreEqual(++notificationsToSend, notificationsSent,
				"PropertyChangedNotification not sent for " + propertyName);
		}

		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void TestCreateCommand()
		{
			var vm = new StudentViewModel();
			var command = vm.CreateCommand;
			Assert.Fail();
		}

        class StudentViewModelTest : StudentViewModel
        {
            public StudentViewModelTest(Student model, ModelStates modelState)
                : base(model, modelState)
            {

            }
        }

        [TestMethod]
        public void TestProtectedCtor()
        {
            var vm = new StudentViewModelTest(CreateStudentTestInstance(), ViewModel<Student>.ModelStates.Updated);
            Assert.IsTrue(vm.IsModelDirty);
            Assert.IsFalse(vm.IsReadOnly);

            vm = new StudentViewModelTest(CreateStudentTestInstance(), ViewModel<Student>.ModelStates.Added);
            Assert.IsTrue(vm.IsModelDirty);
            Assert.IsFalse(vm.IsReadOnly);

            vm = new StudentViewModelTest(CreateStudentTestInstance(), ViewModel<Student>.ModelStates.Unchanged);
            Assert.IsFalse(vm.IsModelDirty);
            Assert.IsTrue(vm.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestProtectedCtorWithNullModel()
        {
            var vm = new StudentViewModelTest(null, ViewModel<Student>.ModelStates.Updated);
            Assert.Fail();            
        }

		[TestMethod]		
		public void TestSaveCancelCommand()
		{
			var vm = new StudentViewModel(CreateStudentTestInstance());
			Assert.IsTrue(vm.IsReadOnly);
			Assert.IsFalse(vm.SaveCommand.CanExecute(null));
			Assert.IsFalse(vm.CancelCommand.CanExecute(null));

			vm.EditCommand.Execute(null);
            Assert.IsFalse(vm.EditCommand.CanExecute(null));    //already in edit
            vm.EditCommand.Execute(null);                       //this should have change nothing

			Assert.IsFalse(vm.SaveCommand.CanExecute(null));
			Assert.IsFalse(vm.CancelCommand.CanExecute(null));
			Assert.IsFalse(vm.IsModelDirty);

            vm.SaveCommand.Execute(null);     //this should be ignored since CanExecute is false
            vm.CancelCommand.Execute(null);   //this should be ignored since CanExecute is false
            Assert.IsFalse(vm.IsReadOnly);

			vm.FirstName = "K";

			Assert.IsTrue(vm.IsModelDirty);
			Assert.IsTrue(vm.SaveCommand.CanExecute(null));
			Assert.IsTrue(vm.CancelCommand.CanExecute(null));

			vm.CancelCommand.Execute(null);

			Assert.IsFalse(vm.IsModelDirty);
			Assert.IsTrue(vm.IsReadOnly);
			Assert.IsFalse(vm.SaveCommand.CanExecute(null));
			Assert.IsFalse(vm.CancelCommand.CanExecute(null));
			Assert.AreEqual(vm.FirstName, StudentFirstName);

			vm.EditCommand.Execute(null);
			vm.FirstName = "K";
			vm.SaveCommand.Execute(null);

			Assert.IsFalse(vm.IsModelDirty);
			Assert.IsTrue(vm.IsReadOnly);
			Assert.IsFalse(vm.SaveCommand.CanExecute(null));
			Assert.IsFalse(vm.CancelCommand.CanExecute(null));
			Assert.AreEqual(vm.FirstName, "K");

            vm = new StudentViewModelTest(CreateStudentTestInstance(), ViewModel<Student>.ModelStates.Added);
            vm.SaveCommand.Execute(null);
            Assert.IsTrue(vm.IsReadOnly);   //saved
		}

		[TestMethod]
		public void TestDeleteCommand()
		{
			var vm = new StudentViewModel(CreateStudentTestInstance());
			Assert.IsFalse(vm.IsModelDeleted);

			ConfirmView.RequestedConfirmation = ConfirmationResult.No;

			Assert.IsTrue(vm.DeleteCommand.CanExecute(null));
			vm.DeleteCommand.Execute(null);
			Assert.IsFalse(vm.IsModelDeleted);

			ConfirmView.RequestedConfirmation = ConfirmationResult.Yes;
			vm.DeleteCommand.Execute(null);
			Assert.IsTrue(vm.IsModelDeleted);

            Assert.IsFalse(vm.DeleteCommand.CanExecute(null));
            Assert.IsFalse(vm.EditCommand.CanExecute(null));
            Assert.IsFalse(vm.SaveCommand.CanExecute(null));
            Assert.IsFalse(vm.CancelCommand.CanExecute(null));

            vm.DeleteCommand.Execute(null); //should do nothing

            vm = new StudentViewModel();
            vm.DeleteCommand.Execute(null);
            Assert.IsFalse(vm.IsModelDeleted);

            vm = new StudentViewModelTest(new Student(), ViewModel<Student>.ModelStates.Unchanged);
            vm.DeleteCommand.Execute(null); //PersonalNumber is null
            Assert.IsTrue(vm.IsModelDeleted);
		}               
	}
}
