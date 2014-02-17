using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zcu.StudentEvaluator.View;

namespace Zcu.StudentEvaluator.DesktopApp
{                  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeIOC();

            //try it
            var ntf = DialogService.DialogService.Default.Get<INotificationView>();
            ntf.DisplayNotification(NotificationType.Error, null, "Testovaci zprava", new InvalidOperationException());
        }

        /// <summary>
        /// Initializes the Interface / Window.
        /// </summary>        
        private void InitializeIOC()
        {
            DialogService.DialogService.Default.RegisterSingleton<INotificationView, NotificationView>(this.NtfView, DialogService.DialogConstants.NotificationView);
            DialogService.DialogService.Default.Register<IConfirmationView, ConfirmationView>(DialogService.DialogConstants.ConfirmationView);
            //DialogService.DialogService.Default.Register<IWindowView, StudentView>(DialogService.DialogConstants.EditStudentView);

            //DialogService.DialogService.Default.Register<IWindowView, StudentListView>();	//main View
        }
    }
}
