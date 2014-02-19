using System.Diagnostics.Contracts;
namespace Zcu.StudentEvaluator.View
{
    /// <summary>
    /// Contains values that identify the way in which the confirmation dialog box was closed.
    /// </summary>
	[System.Flags]
	public enum ConfirmationResult
	{
        /// <summary>
        /// Unknown - must ask user differently (error)
        /// </summary>
		Ask = 0,

        /// <summary>
        /// The dialog box return value is OK (usually sent from a button labeled OK). 
        /// </summary>
        OK = 1,

        /// <summary>
        /// The dialog box return value is Cancel (usually sent from a button labeled Cancel). 
        /// </summary>
		Cancel = 2,

        /// <summary>
        /// The dialog box return value is Abort (usually sent from a button labeled Abort). 
        /// </summary>
		Abort = 4,

        /// <summary>
        /// The dialog box return value is Retry (usually sent from a button labeled Retry). 
        /// </summary>
		Retry = 8,

        /// <summary>
        /// The dialog box return value is Ignore (usually sent from a button labeled Ignore). 
        /// </summary>
		Ignore = 16,

        /// <summary>
        /// The dialog box return value is Yes (usually sent from a button labeled Yes). 
        /// </summary>
		Yes = 32,

        /// <summary>
        /// The dialog box return value is No (usually sent from a button labeled No). 
        /// </summary>
		No = 64,

        /// <summary>
        /// The dialog box return value is Yes (usually sent from a button labeled Yes To All). 
        /// "Yes" should be returned automatically to all other successive confirmation requests of the same kind.
        /// </summary>
		YesToAll = 128,

        /// <summary>
        /// The dialog box return value is No (usually sent from a button labeled No To All). 
        /// "No" should be returned automatically to all other successive confirmation requests of the same kind.
        /// </summary>
		NoToAll = 256,
	}

    /// <summary>
    /// Contains options that may be used in confirmation requests <see cref="IConfirmationView" />
    /// </summary>
	public enum ConfirmationOptions
	{

        /// <summary>
        /// The message box contains an OK button. 
        /// </summary>
        OK = ConfirmationResult.OK,

        /// <summary>
        /// The ok cancel
        /// </summary>
		OKCancel = ConfirmationResult.OK | ConfirmationResult.Cancel,		//The message box contains OK and Cancel buttons. 

        /// <summary>
        /// The message box contains Abort, Retry, and Ignore buttons. 
        /// </summary>
        AbortRetryIgnore = ConfirmationResult.Abort | ConfirmationResult.Retry | ConfirmationResult.Ignore,	

        /// <summary>
        /// The message box contains Yes, No, and Cancel buttons. 
        /// </summary>
		YesNoCancel = ConfirmationResult.Yes | ConfirmationResult.No | ConfirmationResult.Cancel,		

        /// <summary>
        /// The message box contains Yes and No buttons. 
        /// </summary>
		YesNo = ConfirmationResult.Yes | ConfirmationResult.No,
        
        /// <summary>
        /// The message box contains Retry and Cancel buttons.
        /// </summary>
		RetryCancel = ConfirmationResult.Retry | ConfirmationResult.Cancel,
        
        /// <summary>
        /// The message box contains Yes, No, Yes To All and No To All buttons. 
        /// </summary>
		YesYesoAllNoTNoToAll = YesNo | ConfirmationResult.YesToAll | ConfirmationResult.NoToAll,

        /// <summary>
        /// The message box contains Yes, No, Yes To All, No To All, and Cancel buttons. 
        /// </summary>
		YesYesoAllNoTNoToAllCancel = YesNoCancel | ConfirmationResult.YesToAll | ConfirmationResult.NoToAll,
	}	

	/// <summary>
	/// This represents dialog with the user to confirm some action, e.g., closing the document without saving.
	/// </summary>
	[ContractClass(typeof(ContractClassForIConfirmationView))]
    public interface IConfirmationView
	{
		/// <summary>
		/// Confirms the action to be done.
		/// </summary>
		/// <param name="options">Options available during the confirmation.</param>
		/// <param name="caption">The caption, i.e., a short summary of what is needed to be confirmed.</param>
		/// <param name="message">The detailed explanation of what is to be confirmed.</param>
		/// <returns>User decision.</returns>
		ConfirmationResult ConfirmAction(ConfirmationOptions options, string caption, string message);
	}

    [ContractClassFor(typeof(IConfirmationView))]
    abstract class ContractClassForIConfirmationView : IConfirmationView
    {
        public ConfirmationResult ConfirmAction(ConfirmationOptions options, string caption, string message)
        {
            Contract.Requires(caption != null);
            Contract.Requires(message != null);

            throw new System.NotImplementedException();
        }
    }

}
