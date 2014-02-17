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
using System.Windows.Threading;

namespace Zcu.StudentEvaluator.View
{
    /// <summary>
    /// Interaction logic for NotificationView.xaml
    /// </summary>
    public partial class NotificationView : UserControl, INotificationView
    {
        /// <summary>
        /// Gets or sets the title of the notification.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(NotificationView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the notification text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NotificationView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>
        /// The color of the text.
        /// </value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Color), typeof(NotificationView), new PropertyMetadata(Colors.Black));

        /// <summary>
        /// Gets or sets the time out for displaying the notification.
        /// </summary>
        /// <value>
        /// The time out interval in ms. Use -1 to indefinite display.
        /// </value>
        public int DisplayTimeOut
        {
            get { return (int)GetValue(DisplayTimeOutProperty); }
            set { SetValue(DisplayTimeOutProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayTimeOut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayTimeOutProperty =
            DependencyProperty.Register("DisplayTimeOut", typeof(int), typeof(NotificationView), new PropertyMetadata(5000));

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationView"/> class.
        /// </summary>
        public NotificationView()
        {
            InitializeComponent();
        }

        #region INotificationView Members

        /// <summary>
        /// The timer
        /// </summary>
        private DispatcherTimer _timer = new DispatcherTimer();
        private long _timerHideTime = 0;

        /// <summary>
        /// Displays the notification message to the user.
        /// </summary>
        /// <param name="type">The type of the notification.</param>
        /// <param name="caption">The caption of the message, i.e., this is a short summary of what has happened.</param>
        /// <param name="message">The message to be displayed containing the detailed explanation of what has happened.</param>
        /// <param name="exc">The exception containing all the details (may be null).</param>        
        public void DisplayNotification(NotificationType type, string caption, string message, Exception exc = null)
        {            
            switch (type)
            {
                case NotificationType.Message:
                    this.TextColor = Properties.ColorSettings.Default.MessageColor; break;
                case NotificationType.Warning:
                    this.TextColor = Properties.ColorSettings.Default.WarningColor; break;
                default:
                    this.TextColor = Properties.ColorSettings.Default.ErrorColor; break;
            }

            if (caption != null && caption.Length > 0)
                this.Title = type.ToString().ToUpper() + " : " + caption;
            else
                this.Title = type.ToString().ToUpper();

            if (exc == null)
                this.Text = message;
            else            
                this.Text = message + "\nException:" + exc.ToString();

            this.Visibility = System.Windows.Visibility.Visible;

            if (this.DisplayTimeOut > 0)
            {
                _timerHideTime = Environment.TickCount + this.DisplayTimeOut;
                if (!_timer.IsEnabled)  //if timer is not running
                {
                    _timer.Interval = new TimeSpan(2500); //each 250 ms
                    _timer.Tick += (sender, e) =>
                        {
                            if (Environment.TickCount > _timerHideTime)
                            {
                                _timer.Stop();
                                this.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        };
                    
                    _timer.Start();
                }                
            }
        }

        #endregion        
    }
}
