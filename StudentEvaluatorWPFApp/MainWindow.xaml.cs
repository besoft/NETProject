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
        }
        
        /// <summary>
        /// Initializes the Interface / Window.
        /// </summary>        
        private void InitializeIOC()
        {
            DialogService.DialogService.Default.RegisterSingleton<INotificationView, NotificationView>(this.NtfView, DialogService.DialogConstants.NotificationView);  
        } 
    }
}
