using System;
using System.Runtime.CompilerServices;

namespace Zcu.StudentEvaluator.DialogService
{
    /// <summary>
    /// This namespace contains classes and interfaces to support binding of concrete implementation of Views with unique View interfaces.
    /// </summary>       
    [CompilerGenerated]
    internal class NamespaceDoc
    {
        //Trick to document a namespace
    }

    /// <summary>
    /// Identifiers of well-known Views
    /// </summary>
	public enum DialogConstants : int
	{
        /// <summary>
        /// The view should be automatically resolved from the specified interface type
        /// </summary>
		AutoResolve,

        /// <summary>
        /// The view used to notify the user about errors, warnings, messages, ...
        /// </summary>
		NotificationView,

        /// <summary>
        /// The view allowing confirmation/rejection of some action by the user
        /// </summary>
        ConfirmationView,

        /// <summary>
        /// The view for editing the data of the given student viewmodel
        /// </summary>
		EditStudentView,
	}

	/// <summary>
	/// Provides support for dialog between ViewModels and Views.
	/// </summary>
	/// <remarks>Application uses the concrete class of  IDialogService to register concrete View implementations of various View interfaces.
	/// ViewModels access Views objects via their registered interfaces. This actually splits Views from ViewModels. 
	/// However, the project grows (due to a heavy use of interfaces)</remarks>
	public interface IDialogService
	{
		/// <summary>
		/// Registers the view.
		/// </summary>
		/// <typeparam name="TviewInterface">The type of the view interface, e.g., IMainView.</typeparam>
		/// <param name="instanceCreator">The delegate to a function that creates a new instance of the concrete implementation of TviewInterface, 
		/// e.g., MainView, usually specified as a lambda expression: () => new MainView(param1, param2)</param>
		/// <param name="viewId">The requested view unique identifier.</param>
		/// <remarks>If an interface is implemented by one concrete class only, viewId can be ignored, otherwise it is important.</remarks>
		/// <returns>The assigned view unique identifier (that may be passed to Get method)</returns>
		DialogConstants Register<TviewInterface>(Func<TviewInterface> instanceCreator, DialogConstants viewId = DialogConstants.AutoResolve)			
			where TviewInterface : class;

        /// <summary>
        /// Registers the view.
        /// </summary>
        /// <typeparam name="TviewInterface">The type of the view interface, e.g., IMainView.</typeparam>
        /// <typeparam name="TviewImplementation">The type of the view implementation, e.g., MainView (implements IMainView).</typeparam>
        /// <param name="viewId">The requested view unique identifier.</param>
        /// <returns>
        /// The assigned view unique identifier (that may be passed to Get method)
        /// </returns>
        /// <remarks>
        /// When the instance is required, non-parametric constructor is used.
        /// If an interface is implemented by one concrete class only, viewId can be ignored, otherwise it is important.
        /// </remarks>
		DialogConstants Register<TviewInterface, TviewImplementation>(DialogConstants viewId = DialogConstants.AutoResolve)			
			where TviewInterface : class
			where TviewImplementation : TviewInterface, new();

        /// <summary>
        /// Registers the view.
        /// </summary>
        /// <typeparam name="TviewInterface">The type of the view interface, e.g., IMainView.</typeparam>
        /// <typeparam name="TviewImplementation">The type of the view implementation, e.g., MainView (implements IMainView).</typeparam>
        /// <param name="instance">The existing instance to the sigleton.</param>
        /// <param name="viewId">The requested view unique identifier.</param>
        /// <returns>
        /// The assigned view unique identifier (that may be passed to Get method)
        /// </returns>
        /// <remarks>
        /// When the instance is required, the same instance (instance) is always returned, i.e., this method register
        /// singleton view (has shared instance). If an interface is implemented by one concrete class only, viewId can be ignored, otherwise it is important.
        /// </remarks>
		DialogConstants RegisterSingleton<TviewInterface, TviewImplementation>(TviewImplementation instance, DialogConstants viewId = DialogConstants.AutoResolve)			
			where TviewInterface : class
			where TviewImplementation : TviewInterface;

		/// <summary>
		/// Gets the view identified by its type and optionally by its unique identifier.
		/// </summary>
		/// <typeparam name="TviewInterface">The type of the view interface.</typeparam>
		/// <param name="viewId">The view unique identifier from Register methods.</param>
		/// <returns>null, if  the view does not exist, otherwise instance of the registered view of the specified type.</returns>
		TviewInterface Get<TviewInterface>(DialogConstants viewId = DialogConstants.AutoResolve)
			where TviewInterface : class;
	}
}
