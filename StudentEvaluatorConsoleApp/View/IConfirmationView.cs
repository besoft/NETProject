namespace Zcu.StudentEvaluator.View
{
	[System.Flags]
	public enum ConfirmationResult
	{		
		OK = 1,			//The dialog box return value is OK (usually sent from a button labeled OK). 
		Cancel = 2,		//The dialog box return value is Cancel (usually sent from a button labeled Cancel). 
		Abort = 4,		//The dialog box return value is Abort (usually sent from a button labeled Abort). 
		Retry = 8,		//The dialog box return value is Retry (usually sent from a button labeled Retry). 
		Ignore = 16,	//The dialog box return value is Ignore (usually sent from a button labeled Ignore). 
		Yes = 32,		//The dialog box return value is Yes (usually sent from a button labeled Yes). 
		No = 64,		//The dialog box return value is No (usually sent from a button labeled No). 
	}

	public enum ConfirmationOptions
	{
		OK = ConfirmationResult.OK,											// The message box contains an OK button. 
		OKCancel = ConfirmationResult.OK | ConfirmationResult.Cancel,		//The message box contains OK and Cancel buttons. 
		AbortRetryIgnore = ConfirmationResult.Abort | ConfirmationResult.Retry | ConfirmationResult.Ignore,	
				//The message box contains Abort, Retry, and Ignore buttons. 
		YesNoCancel = ConfirmationResult.Yes | ConfirmationResult.No | ConfirmationResult.Cancel,		
				//The message box contains Yes, No, and Cancel buttons. 
		YesNo = ConfirmationResult.Yes | ConfirmationResult.No,				//The message box contains Yes and No buttons. 
		RetryCancel = ConfirmationResult.Retry | ConfirmationResult.Cancel	//The message box contains Retry and Cancel buttons.
	}	

	/// <summary>
	/// This represents dialog with the user to confirm some action, e.g., closing the document without saving.
	/// </summary>
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
}
