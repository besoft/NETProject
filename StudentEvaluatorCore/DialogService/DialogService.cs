using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Zcu.StudentEvaluator.DialogService
{
	/// <summary>
	/// Provides support for dialog between ViewModels and Views.
	/// </summary>
	public sealed class DialogService : IDialogService
	{
		private static IDialogService _defaultService = null;

		/// <summary>
		/// Gets or sets the default.
		/// </summary>
		/// <value>
		/// The default.
		/// </value>
		public static IDialogService Default
		{
			get
			{
				if (_defaultService == null)
					_defaultService = new DialogService();

				return _defaultService;
			}
		}
	
		private DialogService ()
		{
			//private ctor to prevent construction
		}

		
		private class RegistryEntryBase
		{
			public DialogConstants viewId;
		}

		private class RegistryEntry<T> : RegistryEntryBase
		{			
			public Func<T> instanceCreator;
		}

		private Dictionary<Type, List<RegistryEntryBase>> _registry = new Dictionary<Type, List<RegistryEntryBase>>();

		/// <summary>
		/// Registers the view.
		/// </summary>
		/// <typeparam name="TviewInterface">The type of the view interface, e.g., IMainView.</typeparam>
		/// <param name="instanceCreator">The delegate to a function that creates a new instance of the concrete implementation of TviewInterface, 
		/// e.g., MainView, usually specified as a lambda expression: () => new MainView(param1, param2)</param>
		/// <param name="viewId">The requested view unique identifier.</param>
		/// <remarks>If an interface is implemented by one concrete class only, viewId can be ignored, otherwise it is important.</remarks>
		/// <returns>The assigned view unique identifier (that may be passed to Get method)</returns>        
		public DialogConstants Register<TviewInterface>(Func<TviewInterface> instanceCreator, DialogConstants viewId = DialogConstants.AutoResolve) where TviewInterface : class
		{
			var type = typeof(TviewInterface);

			List<RegistryEntryBase> list;
			if (!_registry.TryGetValue(type, out list))
				_registry.Add(type, list = new List<RegistryEntryBase>());

            Contract.Assume(list != null);
			if (viewId != DialogConstants.AutoResolve)
			{
				if (list.Find(x => x.viewId == viewId) != null)
					throw new ArgumentException("Interface is already registered for view with id=" + viewId);
			}
			else
			{
				//automatically assign a new Constant                
                Contract.Assume(Contract.ForAll<RegistryEntryBase>(list, x => x != null));

				int maxId = Math.Max(Enum.GetValues(typeof(DialogConstants)).Cast<int>().Max(), list.Max(x => (int)x.viewId));
				viewId = (DialogConstants)(maxId + 1);
			}

			var entry = new RegistryEntry<TviewInterface>()
			{
				viewId = viewId,
				instanceCreator = instanceCreator,
			};

			list.Add(entry);
			return viewId;
		}

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
		public DialogConstants Register<TviewInterface, TviewImplementation>(DialogConstants viewId = DialogConstants.AutoResolve)
			where TviewInterface : class
			where TviewImplementation : TviewInterface, new()
		{
			return Register<TviewInterface>(() => new TviewImplementation(), viewId);
		}

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
		public DialogConstants RegisterSingleton<TviewInterface, TviewImplementation>(TviewImplementation instance, DialogConstants viewId = DialogConstants.AutoResolve)
			where TviewInterface : class
			where TviewImplementation : TviewInterface
		{
			return Register<TviewInterface>(() => instance, viewId);
		}


		/// <summary>
		/// Gets the view identified by its type and optionally by its unique identifier.
		/// </summary>
		/// <typeparam name="TviewInterface">The type of the view interface.</typeparam>
		/// <param name="viewId">The view unique identifier from Register methods.</param>
		/// <returns>null, if  the view does not exist, otherwise instance of the registered view of the specified type.</returns>
		public TviewInterface Get<TviewInterface>(DialogConstants viewId = DialogConstants.AutoResolve)
			where TviewInterface : class
		{			
			List<RegistryEntryBase> list;
			if (!_registry.TryGetValue(typeof(TviewInterface), out list))
				return null;

			RegistryEntry<TviewInterface> entry = null;
			if (viewId != DialogConstants.AutoResolve)
				entry = (RegistryEntry < TviewInterface >) list.Where(x => x.viewId == viewId).SingleOrDefault();
			else if (list.Count > 1)
				throw new ArgumentException("Cannot automatically resolve the concrete implementation for " + typeof(TviewInterface).Name);
			else if (list.Count > 0)
				entry = (RegistryEntry < TviewInterface >) list[0];
						
			return entry != null ? entry.instanceCreator() : null;
		}
	}
}
