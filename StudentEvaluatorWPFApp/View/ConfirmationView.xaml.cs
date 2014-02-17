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
using System.Windows.Shapes;

namespace Zcu.StudentEvaluator.View
{
    /// <summary>
    /// Interaction logic for ConfirmationView.xaml
    /// </summary>
    public partial class ConfirmationView : Window, IConfirmationView
    {
        public ConfirmationView()
        {
            InitializeComponent();
        }

                
        #region IConfirmationView Members
        private ConfirmationResult _result = ConfirmationResult.Ask;

        /// <summary>
        /// Confirms the action to be done.
        /// </summary>
        /// <param name="options">Options available during the confirmation.</param>
        /// <param name="caption">The caption, i.e., a short summary of what is needed to be confirmed.</param>
        /// <param name="message">The detailed explanation of what is to be confirmed.</param>
        /// <returns>
        /// User decision.
        /// </returns>        
        public ConfirmationResult ConfirmAction(ConfirmationOptions options, string caption, string message)
        {
            this.Title = caption;
            this.MessageCtrl.Text = message;
            this._result = ConfirmationResult.Ask;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.Abort))
                this.AbortBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.Retry))
                this.RetryBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.Ignore))
                this.IgnoreBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.OK))
                this.OKBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.Yes))
                this.YesBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.YesToAll))
                this.YesToAllBttn.Visibility = System.Windows.Visibility.Collapsed;	//Abort is not used with YesToAll

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.No))
                this.NoBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.NoToAll))
                this.NoToAllBttn.Visibility = System.Windows.Visibility.Collapsed;

            if (!options.HasFlag((ConfirmationOptions)ConfirmationResult.Cancel))
                this.CancelBttn.Visibility = System.Windows.Visibility.Collapsed;

            this.ShowDialog();
            return _result;
        }

        #endregion

        private void AbortBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.Abort;
            this.DialogResult = true;            
        }

        private void RetryBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.Retry;
            this.DialogResult = true;   
        }

        private void IgnoreBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.Ignore;
            this.DialogResult = true;   
        }

        private void OKBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.OK;
            this.DialogResult = true;   
        }

        private void YesBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.Yes;
            this.DialogResult = true;   
        }

        private void YesToAllBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.YesToAll;
            this.DialogResult = true;   
        }

        private void NoBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.No;
            this.DialogResult = true;   
        }

        private void NoToAllBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.NoToAll;
            this.DialogResult = true;   
        }

        private void CancelBttn_Click(object sender, RoutedEventArgs e)
        {
            this._result = ConfirmationResult.Cancel;
            this.DialogResult = false;
        }
    }
}
