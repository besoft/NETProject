using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using Zcu.StudentEvaluator.View;

namespace Zcu.StudentEvaluator.ViewModel
{
	/// <summary>
	/// Base class for all ViewModel classes in the application.
	/// It provides support for property change notifications and has a DisplayName property.  This class is abstract.
	/// </summary>	
	/// <remarks>
	/// ViewModel is a wrapper of Model and provides four kinds of properties:
	/// a) Model Property = R/W property that provides direct R/W access to the same named property of the underlying model (e.g., FirstName)
	/// b) Model Derived Property = R property that provides some product of values in model properties, e.g., FullName, DisplayName
	/// c) Presentation Property = R/W property that stores some hints about how the ViewModel should be visualised, e.g., IsSelected
	/// d) Another Property = special property, typically, private or protected 
	/// 
	/// All properties in a), b), c) groups notifies the listeners about changes done (via INotifyPropertyChanged). Properties in b) group
	/// are being cached so that their product calculation is not necessary to repeat all the time. The cached value is released when 
	/// a change of such a property is notified (typically from some property in group a)). The cached is also released when 
	/// ResetModelCache is called (causes notification of changes, so that listeners may retrieve new values). Properties in a) group
	/// are "cached" upon its first modification (as originalValues). When CancelModelChanges is called originalValues are copied to properties
	/// (causes notification of changes). When SaveModelChanges is called, this "cache" is released, i.e., originalValues are released.
	/// 
	///INotifyPropertyChanged = standard interface for notifying listener that some property has changed. When WPF binding is being
	///established, the WPF core register itself as a listener of the source (an object implementing INotifyPropertyChanged) so as
	///whenever the source property changes, the underlying code passes the new value to the target property (using appropriate convertor)
	///IDataErrorInfo = standard interface for notifying caller about validation errors. When WPF binding is established 
	///(with  ValidatesOnDataErrors=True), after the value is passed from target property to the source property,
	///source object's IDataErrorInfo is used to get validation error.
	/// </remarks>
	public abstract class ViewModelBase : IViewModelBase, INotifyPropertyChanged, IDisposable		
	{			
		#region Model Derived Properties Caching
		private Dictionary<string, object> _modelDerivedPropertiesCache = new Dictionary<string, object>();	//cache for Model Derived Properties

		/// <summary>
		/// Gets the model derived property value.
		/// </summary>
		/// <typeparam name="T">return type</typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="defaultSelector">Delegate that gives the original (default value).</param>
		/// <remarks>The method returns the cached value of the property, or, if the value is not in the cache, it uses defaultSelector to 
		/// get the default value of the property, which is then stored into the cached and returned to the caller. </remarks>
		/// <returns>
		/// The value of the property
		/// </returns>
		protected T GetModelDerivedPropertyValue<T>([CallerMemberName] string propertyName = null, Func<T> defaultSelector = null)
		{
			object value;
			if (!_modelDerivedPropertiesCache.TryGetValue(propertyName, out value))
			{
				value = (defaultSelector == null ? default(T) : defaultSelector());
				_modelDerivedPropertiesCache.Add(propertyName, value);
			}

			return (T)value;
		}

		/// <summary>
		/// Gets the model derived property value.
		/// </summary>
		/// <typeparam name="T">return type</typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value of the property</returns>
		protected T GetModelDerivedPropertyValue<T>([CallerMemberName] string propertyName = null, T defaultValue = default(T))
		{
			object value;
			if (!_modelDerivedPropertiesCache.TryGetValue(propertyName, out value))			
				_modelDerivedPropertiesCache.Add(propertyName, value = defaultValue);
			
			return (T)value;
		}

		/// <summary>
		/// Gets the value of the property selected by lambda expression.
		/// </summary>
		/// <remarks>propertyNameSelector is supposed to be a lambda expression of style: () => PropertyName</remarks>
		/// <typeparam name="T">return type</typeparam>
		/// <param name="propertyNameSelector">The property name selector.</param>
		/// <param name="defaultSelector">The delegate to a function to be called when the cached value cannot be retrieved.</param>		
		/// <returns>The value of the property</returns>
		protected T GetModelDerivedPropertyValue<T>(Expression<Func<T>> propertyNameSelector, Func<T> defaultSelector = null)
		{
			return GetModelDerivedPropertyValue(GetPropertyName(propertyNameSelector), defaultSelector);
		}

		/// <summary>
		/// Gets the value of the property selected by lambda expression.
		/// </summary>
		/// <typeparam name="T">return type</typeparam>
		/// <param name="propertyNameSelector">The property name selector.</param>
		/// <param name="defaultValue">The default value used when the cached value cannot be retrieved.</param>
		/// <returns>
		/// The value of the property
		/// </returns>
		/// <remarks>
		/// propertyNameSelector is supposed to be a lambda expression of style: () =&gt; PropertyName
		/// </remarks>
		protected T GetModelDerivedPropertyValue<T>(Expression<Func<T>> propertyNameSelector, T defaultValue)
		{
			return GetModelDerivedPropertyValue(GetPropertyName(propertyNameSelector), defaultValue);
		}

		/// <summary>
		/// Clears all entries in the model derived properties cache.
		/// </summary>
		/// <remarks>Raises change notification for every cached value.</remarks>
		protected void ClearModelDerivedPropertiesCache()
		{
			foreach (var item in _modelDerivedPropertiesCache.Keys.ToList())	//we need .ToList because we are going to modify the collection
			{
				RemoveModelDerivedPropertyCacheEntry(item);
			}
		}

        /// <summary>
        /// Clears the entry in the model derived properties cache for the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <remarks>
        /// Raises change notification for every cached value.
        /// </remarks>
		protected void RemoveModelDerivedPropertyCacheEntry(string propertyName)
		{
			//make sure that listener that have the cached values are notified so that they may get fresh (new) values if necessary
			if (_modelDerivedPropertiesCache.Remove(propertyName))
				NotifyPropertyChanged(propertyName);
		}

		/// <summary>
		/// Clears the entry in the model derived properties cache for the given property.
		/// </summary>				
		/// <param name="propertyNameSelector">The selector of the property. It is assumed that this selector is lambda expression 
		/// such as: "() => PropertyName".</param>
		/// <remarks>Raises change notification for every cached value.</remarks>
		protected void RemoveModelDerivedPropertyCacheEntry<T>(Expression<Func<T>> propertyNameSelector)
		{
			RemoveModelDerivedPropertyCacheEntry(GetPropertyName(propertyNameSelector));
		}
		#endregion
		
		#region Representation Properties
		/// <summary>
		/// Returns the user-friendly name of this object.
		/// Child classes can set this property to a new value,
		/// or override it to determine the value on-demand.
		/// </summary>
		public virtual string DisplayName { get; protected set; }

		/// <summary>
		/// Gets or sets a value indicating whether the ViewModel is read only.
		/// </summary>
		/// <value>
		///   <c>true</c> if the ViewModel is read only; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>When IsReadOnly is set to true, setting any model property ends with an exception. </remarks>
		public bool IsReadOnly { get; protected set; }
		#endregion // DisplayName

		#region DialogService Helpers
		/// <summary>
		/// Confirms the action to be done.
		/// </summary>
		/// <param name="options">Options available during the confirmation.</param>
		/// <param name="caption">The caption, i.e., a short summary of what is needed to be confirmed.</param>
		/// <param name="message">The detailed explanation of what is to be confirmed.</param>
		/// <param name="confResult">The confirmation result.</param>
		/// <returns>true, if the confirmation dialog has been successfully done, false otherwise</returns>
		protected bool ConfirmAction(ConfirmationOptions options, string caption, string message, ref ConfirmationResult confResult)
		{
			var confirmView = DialogService.DialogService.Default.Get<IConfirmationView>();
			if (confirmView == null)
			{
				Debug.Fail("IConfirmationView could not be resolved.");
				return false;
			}

			confResult = confirmView.ConfirmAction(options, caption, message);
			return true;
		}

		/// <summary>
		/// Displays the notification message to the user.
		/// </summary>
		/// <param name="type">The type of the notification.</param>
		/// <param name="caption">The caption of the message, i.e., this is a short summary of what has happened.</param>
		/// <param name="message">The message to be displayed containing the detailed explanation of what has happened.</param>
		/// <param name="exc">The exception containing all the details (may be null).</param>
		/// <returns>true, if the notification has been successfully displayed, false otherwise</returns>
		protected bool DisplayNotification(NotificationType type, string caption, string message, Exception exc = null)
		{
			var notifyView = DialogService.DialogService.Default.Get<INotificationView>();
			if (notifyView == null)
			{
				Debug.Fail("INotificationView could not be resolved.");
				return false;
			}

			notifyView.DisplayNotification(type, caption, message, exc);
			return true;
		}
		#endregion

		#region Debugging Aides
		/// <summary>
		/// Warns the developer if this object does not have
		/// a public property with the specified name. This 
		/// method does not exist in a Release build.
		/// </summary>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			// Verify that the property name matches a real,  
			// public, instance property on this object.
			if (TypeDescriptor.GetProperties(this)[propertyName] == null)
			{
				var propInfo = this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
				if (propInfo == null || propInfo.GetAccessors(true)[0].IsPrivate)
				{
					string msg = "Invalid property name: " + propertyName;

					if (this.ThrowOnInvalidPropertyName)
						throw new Exception(msg);
					else
						Debug.Fail(msg);
				}
			}
		}		

		/// <summary>
		/// Returns whether an exception is thrown, or if a Debug.Fail() is used
		/// when an invalid property name is passed to the VerifyPropertyName method.
		/// The default value is false, but subclasses used by unit tests might 
		/// override this property's getter to return true.
		/// </summary>
		protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

		#endregion // Debugging Aides

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Raised when a property on this object has a new value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The property that has a new value.</param>
		///<remarks>The CallerMemberName attribute that is applied to the optional propertyName  (from .NET 4.5)
		/// parameter causes the property name of the caller to be substituted as an argument.  </remarks>
		protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
		{
			this.VerifyPropertyName(propertyName);

			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">Data type of the property that has changed</typeparam>
        /// <param name="propertyNameSelector">The selector of property name.</param>
        /// <remarks>
        /// This version has similar behaviour as [CallerMemberName] with the only reason that
        /// the caller is not the property itself but some other method. This method is then called using lambda expression
        /// as '() =&gt; Property'
        /// </remarks>
		protected virtual void NotifyPropertyChanged<T>(Expression<Func<T>> propertyNameSelector)
		{			
			NotifyPropertyChanged(GetPropertyName(propertyNameSelector));		
		}

        /// <summary>
        /// Gets the name of the property passed as a lambda expression.
        /// </summary>
        /// <typeparam name="T">Data type of the property that has changed</typeparam>
        /// <param name="propertyNameSelector">The property name selector.</param>
        /// <returns>
        /// The name of the property or null, if propertyNameSelector does not selects a property
        /// </returns>
		protected string GetPropertyName<T>(Expression<Func<T>> propertyNameSelector)
		{	
			if (propertyNameSelector == null)
				return null;

			var unary = propertyNameSelector.Body as UnaryExpression;			//for value types, there is UnaryExpression (boxing)
			var member = unary != null ? unary.Operand as MemberExpression :	//for reference types, the member is already the property
				propertyNameSelector.Body as MemberExpression;			
		    
			return member != null ? member.Member.Name : null;			
		}

		#endregion // INotifyPropertyChanged Members

		#region IDisposable Members

		/// <summary>
		/// Invoked when this object is being removed from the application
		/// and will be subject to garbage collection.
		/// </summary>
		public void Dispose()
		{
			this.OnDispose();
		}

		/// <summary>
		/// Child classes can override this method to perform 
		/// clean-up logic, such as removing event handlers.
		/// </summary>
		protected virtual void OnDispose()
		{
		}

#if DEBUG
		/// <summary>
		/// Useful for ensuring that ViewModel objects are properly garbage collected.
		/// </summary>
		~ViewModelBase()
		{
			string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
			System.Diagnostics.Debug.WriteLine(msg);
		}
#endif

		#endregion // IDisposable Members			
	}
}
