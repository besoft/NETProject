using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.View;
using Zcu.StudentEvaluator.ViewModel;

namespace Zcu.StudentEvaluator.DesktopApp
{    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {   
        private DbStudentEvaluationUnitOfWork _unitOfWork = null;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("cs-CZ");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            //Register Views
            DialogService.DialogService.Default.Register<IConfirmationView, ConfirmationView>(DialogService.DialogConstants.ConfirmationView);
            //DialogService.DialogService.Default.Register<IWindowView, StudentView>(DialogService.DialogConstants.EditStudentView);

            //DialogService.DialogService.Default.Register<IWindowView, StudentListView>();	//main View

         
            _unitOfWork = 
				//new LocalStudentEvaluationUnitOfWork();
				new DbStudentEvaluationUnitOfWork();
			if (_unitOfWork.Categories.Get().FirstOrDefault() == null)
			{
				try
				{
					_unitOfWork.PopulateWithData();
				}
				catch (DbEntityValidationException excValidation)
				{
                    var sb = new StringBuilder();

					foreach (var item in excValidation.EntityValidationErrors)
					{                        
						sb.AppendFormat("Validation of '{0}' failed with these errors:\n", item.Entry.Entity.GetType().Name);
						foreach (var err in item.ValidationErrors)
						{
                            sb.AppendFormat("For '{0}' : {1}\n", err.PropertyName, err.ErrorMessage);
						}
					}

                    MessageBox.Show(sb.ToString());                    
				}
			}


            base.OnStartup(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Activated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnActivated(EventArgs e)
        {
            if (this.MainWindow != null)
            {
                this.MainWindow.DataContext = new StudentListViewModel(this._unitOfWork);
            }

            base.OnActivated(e);
        }        
    }
}
