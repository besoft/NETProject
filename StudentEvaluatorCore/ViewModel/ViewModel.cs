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
	public abstract class ViewModel<M> : ViewModelBase, IViewModel, IDataErrorInfo
		where M : class, IEntity, new()
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelBase{T}" /> class with a new model.
		/// </summary>
		/// <param name="modelRepository">The model repository.</param>		
		public ViewModel(IRepository<M> modelRepository = null)
		{
			this.Model = new M();
			this.ModelState = this._originalModelState = ModelStates.Added;
			this.ModelRepository = modelRepository;
			
			this.IsReadOnly = false;	//default is View Only
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
		/// </summary>
		/// <param name="model">The model to be wrapped.</param>
		/// <param name="modelRepository">The model repository.</param>
		/// <remarks>Model state is unchanged.</remarks>
		/// <exception cref="System.ArgumentNullException">model cannot be null</exception>
		public ViewModel(M model, IRepository<M> modelRepository = null)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			this.Model = model;
			this.ModelState = this._originalModelState = ModelStates.Unchanged;
			this.ModelRepository = modelRepository;

			this.IsReadOnly = true;	//default is View Only
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelBase{T}"/> class.
		/// </summary>
		/// <param name="model">The model to be wrapped.</param>
		/// <param name="modelRepository">The model repository.</param>
		/// <param name="modelState">State of the model.</param>
		/// <exception cref="System.ArgumentNullException">model cannot be null</exception>
		protected ViewModel(M model, ModelStates modelState, IRepository<M> modelRepository = null)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			this.Model = model;
			this.ModelState = this._originalModelState = modelState;
			this.ModelRepository = modelRepository;

			this.IsReadOnly = modelState.HasFlag(ModelStates.Deleted) || modelState == ModelStates.Unchanged;	//default is View Only
		}
		#endregion // Constructor

		#region Model Base
		[System.Flags]
		public enum ModelStates
		{
			Unchanged = 0,		//there has been no change, so far, i.e., buttons such as "Save" may be disabled

			Added	= 1,		//a completely new model that must be either saved or discarded => "Save" button should be enabled
			Updated = 2,		//some property of the model has been changed => "Save" button should be enabled
			Deleted = 4,		//the object has been deleted and, therefore, any other operation with this object is ignored			
		}

		private ModelStates _originalModelState;			//original model state
		private ModelStates _modelState;					//current model state

		/// <summary>
		/// Gets the underlying model.
		/// </summary>		
		protected M Model { get; set; }

		/// <summary>
		/// Gets the  repository of the model.
		/// </summary>		
		protected IRepository<M> ModelRepository { get; set; }

		/// <summary>
		/// Gets or sets the state of the model.
		/// </summary>
		/// <value>
		/// The new state of the model.
		/// </value>
		protected ModelStates ModelState 
		{
			get { return _modelState;  }
			set
			{
				if (_modelState == value)
					return;

				_modelState = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged(() => this.IsModelNew);
				NotifyPropertyChanged(() => this.IsModelDirty);				
				NotifyPropertyChanged(() => this.IsModelDeleted);
			} 
		}

		/// <summary>
		/// Gets a value indicating whether any property of the underlying model has changed since the last save operation.
		/// </summary>
		/// <value>
		///   <c>true</c> if any model property has changed; otherwise, <c>false</c>.
		/// </value>
		public bool IsModelDirty 
		{
			get 
			{ 
				return !_modelState.HasFlag(ModelStates.Deleted) &&
						(_modelState.HasFlag(ModelStates.Added) || _modelState.HasFlag(ModelStates.Updated)); 
			}
		}

		/// <summary>
		/// Gets a value indicating whether the underlying model is new.
		/// </summary>
		/// <remarks>A new model is such an object that has never been stored into the repository.</remarks>
		/// <value>
		///   <c>true</c> if the underlying model is new; otherwise, <c>false</c>.
		/// </value>
		public bool IsModelNew
		{
			get	
			{
				return !_modelState.HasFlag(ModelStates.Deleted) && 
					_modelState.HasFlag(ModelStates.Added);	
			}
		}

		/// <summary>
		/// Gets a value indicating whether this object is marked for deletion.
		/// </summary>
		/// <remarks>Any operation with the ViewModel in this state is ignored.</remarks>
		/// <value>
		///   <c>true</c> if the object is marked to be deleted; otherwise, <c>false</c>.
		/// </value>
		public bool IsModelDeleted
		{
			get
			{
				return _modelState.HasFlag(ModelStates.Deleted);
			}
		}
		#endregion

		#region Model Properties
		/// <summary>
		/// Gets the unique identifier. 
		/// </summary>		
		public int Id
		{
			//It is read only attribute
			get { return this.Model.Id; }
		}
		#endregion

		#region Model Properties Caching
		private Dictionary<string, object> _modelPropertiesCache = new Dictionary<string, object>();	//cache for Model Properties
		private bool _modelPropertiesCacheEnabled = true;	//when set to false,  _modelPropertiesCache is not used		

		/// <summary>
		/// Gets the model property value.
		/// </summary>
		/// <param name="propertyName">Name of the property (in Model).</param>
		/// <returns>The value</returns>
		protected T GetModelPropertyValue<T>([CallerMemberName] string propertyName = null)
		{
			//use reflection to get the property value from the underlying model
			return (T)this.Model.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, this.Model, null);
			//return (T)TypeDescriptor.GetProperties(this.Model)[propertyName].GetValue(this.Model); --- looks nicer but this is at least 4x slower
		}

		/// <summary>
		/// Sets the model property value.
		/// </summary>
		/// <typeparam name="T">type of the value</typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="value">The new value.</param>
		/// <remarks>The method is supposed to be called from setters of the Model Properties of ViewModel. 
		/// The call is successful only if the new value is not equal the current value. Upon the first successful call,
		/// the current value is stored in order to support undo (unless _modelPropertiesCacheEnabled is false). Whenever the value is changed,
		/// automatic notification is raised. Caller property is supposed to raise then notification of a change of any derived value. </remarks>
		/// <returns>true, if value was successfully set; false otherwise (e.g. the new value is the same as the old one)</returns>
		protected bool SetModelPropertyValue<T>([CallerMemberName] string propertyName = null, T value = default(T))
		{
			if (this.IsReadOnly)
				throw new InvalidOperationException("object is read-only");

			if (ModelState.HasFlag(ModelStates.Deleted))
				return false;	//operation is ignored

			T originalValue = GetModelPropertyValue<T>(propertyName);			//get the original value
			if (EqualityComparer<T>.Default.Equals(originalValue, value))		//check if the value is new
				return false;	//no change

			if (_modelPropertiesCacheEnabled)
			{
				if (_modelPropertiesCache.Count == 0)
					_originalModelState = this.ModelState;

				//store the original value into the cache
				if (!_modelPropertiesCache.ContainsKey(propertyName))
					_modelPropertiesCache.Add(propertyName, originalValue);
			}

			this.ModelState |= ModelStates.Updated;

			//store the value into underlying Model using reflection
			this.Model.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, 
				null, this.Model, new object[] {value});

			NotifyPropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// Cancels the changes done to the model property selected by the given selector.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void UndoModelChanges(string propertyName)
		{
			object value;
			if (_modelPropertiesCache.TryGetValue(propertyName, out value))
			{
				bool oldState = _modelPropertiesCacheEnabled;
				_modelPropertiesCacheEnabled = false;			//prevent adding the original value into the cache

				//invoke calling Setter of this object
				this.GetType().InvokeMember(propertyName, BindingFlags.SetProperty,
					null, this, new object[] { value });

				_modelPropertiesCache.Remove(propertyName);		//removes the entry
				_modelPropertiesCacheEnabled = oldState;		//restore the original caching policy
			}
		}

		/// <summary>
		/// Cancels the changes done to the model property selected by the given selector.
		/// </summary>		
		/// <param name="propertyNameSelector">The selector of the property in the form of lambda expression: ()=>PropertyName.</param>
		protected void UndoModelChanges<T>(Expression<Func<T>> propertyNameSelector)
		{			
			UndoModelChanges(GetPropertyName(propertyNameSelector));
		}
		
		/// <summary>
		/// Cancels all the changes done.
		/// </summary>
		/// <returns>true, if the changes have been successfully reversed, false, otherwise</returns>
		virtual internal bool CancelModelChanges()
		{
			ConfirmationResult result = ConfirmationResult.Yes;
			return CancelModelChanges(ConfirmationOptions.YesNoCancel, null, ref result);
		}

		/// <summary>
		/// Cancels all the changes done.
		/// </summary>
		/// <param name="confOptions">The options for confirmation dialog. Yes, No, YesNoCancel, ... </param>
		/// <param name="confQuestion">The confirmation question to be displayed to the user.</param>
		/// <param name="confResult">The input/output result from confirmation.</param>
		/// <returns>true, if the model changes has been successfully cancelled, false otherwise</returns>
		/// <remarks>Confirmation dialog is initiated only if confResult [in] is set to Ask, otherwise the result passed in confResult
		/// is used as confirmation result. This method allows non-silent undoing of model changes.</remarks>
		/// <returns>true, if the changes have been successfully reversed, false, otherwise</returns>
		virtual internal bool CancelModelChanges(ConfirmationOptions confOptions, string confQuestion, ref ConfirmationResult confResult)
		{
			if (_modelPropertiesCache.Count == 0)
				return true;	//nothing to cancel

			if (confResult == ConfirmationResult.Ask)
			{
				if (!ConfirmAction(confOptions, "Cancel changes",
					confQuestion ?? "Do you really want to continue without saving changes?", ref confResult)
				)
				{
					confResult = ConfirmationResult.NoToAll;
					return false;
				}
			}

			if (confResult != ConfirmationResult.Yes && confResult != ConfirmationResult.YesToAll)
				return false;	//item is not to be deleted

			foreach (var item in _modelPropertiesCache.Keys.ToList())	//we need .ToList because we are going to modify the collection
			{
				UndoModelChanges(item);
			}

			this.ModelState = _originalModelState;	//return to the previous state			
			return true;
		}

		/// <summary>
		/// Saves all the changes done into the repository.
		/// </summary>
		/// <returns>true, if the changes have been successfully reversed, false, otherwise</returns>
		virtual internal bool SaveModelChanges()
		{
			ConfirmationResult result = ConfirmationResult.Yes;
			return SaveModelChanges(ConfirmationOptions.YesNoCancel, null, ref result);
		}

		/// <summary>
		/// Saves all the changes done into the repository.
		/// </summary>
		/// <param name="confOptions">The options for confirmation dialog. Yes, No, YesNoCancel, ... </param>
		/// <param name="confQuestion">The confirmation question to be displayed to the user.</param>
		/// <param name="confResult">The input/output result from confirmation.</param>
		/// <returns>true, if the model has been saved successfully, false otherwise</returns>
		/// <remarks>Confirmation dialog is initiated only if confResult [in] is set to Ask, otherwise the result passed in confResult
		/// is used as confirmation result. This method allows non-silent saving of model changes.</remarks>
		/// <returns>true, if the changes have been successfully reversed, false, otherwise</returns>
		virtual internal bool SaveModelChanges(ConfirmationOptions confOptions, string confQuestion, ref ConfirmationResult confResult)
		{
			//if the model has not been marked as deleted
			if (this.IsModelDeleted)
				return true;			//this is ignored

			if (confResult == ConfirmationResult.Ask)
			{				
				if (!ConfirmAction(confOptions, "Save changes",
					confQuestion ?? "Do you really want to save all changes?", ref confResult)
				)
				{
					confResult = ConfirmationResult.NoToAll;
					return false;
				}
			}

			if (confResult != ConfirmationResult.Yes && confResult != ConfirmationResult.YesToAll)
				return false;	//item is not to be saved

			if (this.ModelRepository != null)
			{
				//TODO: handle concurrency problems
				try
				{
					if (this.IsModelNew)
						this.ModelRepository.Insert(this.Model);
					else
						this.ModelRepository.Update(this.Model);

					this.ModelRepository.Save();	//save all data
				}
				catch (Exception e)
				{
					var sb = new StringBuilder("Saving the changes has failed because of the following reason(s):");
					sb.AppendLine();

					//validation error
					DbEntityValidationException ev = e as DbEntityValidationException;
					if (ev != null)
					{
						foreach (var entry in ev.EntityValidationErrors)
						{
							foreach (var it in entry.ValidationErrors)
							{
								sb.AppendFormat("{0}.{1} = {2}: {3}\n",
									entry.Entry.Entity.GetType().Name,
									it.PropertyName,
									entry.Entry.CurrentValues[it.PropertyName],
									it.ErrorMessage
									);
							}
						}
					}

					//notify the user about the error										
					DisplayNotification(NotificationType.Error, "Saving failed", sb.ToString(), e);
					return false;
				}				
			}

			//remove every entry from the _modelPropertiesCache
			_modelPropertiesCache.Clear();
			
			//and set the model state to Unchanged			
			this.ModelState = ModelStates.Unchanged;
			return true;
		}

		/// <summary>
		/// Delete the model object from the repository
		/// </summary>
		/// <param name="confOptions">The options for confirmation dialog.</param>
		/// <param name="confQuestion">The confirmation question to be displayed to the user.</param>
		/// <returns>true, if the model has been deleted successfully, false otherwise</returns>
		virtual internal bool DeleteModel(ConfirmationOptions confOptions = ConfirmationOptions.YesNo, string confQuestion = null)
		{
			ConfirmationResult result = ConfirmationResult.Ask;
			return DeleteModel(confOptions, confQuestion, ref result);
		}

		/// <summary>
		/// Delete the model object from the repository
		/// </summary>
		/// <param name="confOptions">The options for confirmation dialog. Yes, No, YesNoCancel, ... </param>
		/// <param name="confQuestion">The confirmation question to be displayed to the user.</param>
		/// <param name="confResult">The input/output result from confirmation.</param>
		/// <returns>true, if the model has been deleted successfully, false otherwise</returns>
		/// <remarks>Confirmation dialog is initiated only if confResult [in] is set to Ask, otherwise the result passed in confResult
		/// is used as confirmation result. This allows silent deletion, or YesToAll deletion for collections of ViewModels.</remarks>
		virtual internal bool DeleteModel(ConfirmationOptions confOptions, string confQuestion, ref ConfirmationResult confResult)
		{
			//if the model is already deleted
			if (this.IsModelDeleted)
				return true;	//ignore this request

			if (confResult == ConfirmationResult.Ask)
			{
				if (!ConfirmAction(confOptions, "Delete item",
					confQuestion ?? "Do you really want to delete (permanently) the item?", ref confResult)
					)
				{
					confResult = ConfirmationResult.NoToAll;
					return false;
				}
			}

			if (confResult != ConfirmationResult.Yes && confResult != ConfirmationResult.YesToAll)
				return false;	//item is not to be deleted			

			//if the model is new, it exists only locally wrapped in this ViewModel
			//and all that is needed is to change its state
			if (!this.IsModelNew && this.ModelRepository != null)
			{
				//the model is already in the persistent repository and must be deleted from it
				try
				{					
					this.ModelRepository.Delete(this.Model);
					this.ModelRepository.Save();	//save all data
				}
				catch (Exception e)
				{					
					DisplayNotification(NotificationType.Error, "Deletion failed", "Unable to delete the item.", e);
					return false;
				}
			}

			this.ModelState = ModelStates.Deleted;
			return true;
		}		
		#endregion

		#region Model Validation
		/// <summary>
		/// Gets a value indicating whether the model is valid.
		/// </summary>
		/// <value>
		///   <c>true</c> if the model is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsModelValid { 
			get
			{
				return ValidateModel() == String.Empty;
			}
		}

		/// <summary>
		/// Validates the model.
		/// </summary>
		/// <returns>An empty string "", if the model is valid, error string otherwise.</returns>	
		internal virtual string ValidateModel(bool allProperties = true)
		{
			//validation based on IValidatableObject interface implementation of the model class and 
			//ValidationAttribute instances attached to both the model class type and all its properties 
			var valRes = new List<ValidationResult>();
			if (Validator.TryValidateObject(this.Model,
				new ValidationContext(this.Model, null, null), //default validation context
				valRes, allProperties)	//validate also properties
				)
				return String.Empty;	//no error

			//there is some error, let us return the first one
			return valRes.First().ErrorMessage;
		}

		/// <summary>
		/// Validates the given model property.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// An empty string "", if the property value is valid, error string otherwise.
		/// </returns>
		internal virtual string ValidateModelProperty(string propertyName)
		{
			//first, validate the property itself
			var value = GetModelPropertyValue<object>(propertyName);
			var results = new List<ValidationResult>(1);
			var result = Validator.TryValidateProperty(value,
				new ValidationContext(this.Model)
				{
					
					MemberName = propertyName
				},
				results);

			string error = String.Empty;
			if (!result)
			{
				//property value is invalid
				var validationResult = results.First();
				error = validationResult.ErrorMessage;
			}			

			return error;

		}

		/// <summary>
		/// Validates the model property identified by the given lambda expression selector.
		/// </summary>		
		/// <param name="propertyNameSelector">The property name selector, e.g. "() => PropertyName".</param>
		/// <returns>An empty string "", if the property value is valid, error string otherwise.</returns>	
		internal string ValidateModelProperty<T>(Expression<Func<T>> propertyNameSelector)
		{
			return ValidateModelProperty(GetPropertyName(propertyNameSelector));
		}

		#endregion		
		
		#region IDataErrorInfo Members
		/// <summary>
		/// Gets an error message indicating what is wrong with this object.
		/// </summary>
		/// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>	
		string IDataErrorInfo.Error
		{
			get { return ValidateModel(false); }
		}

		/// <summary>
		/// Gets the error message for the property with the given name.
		/// </summary>
		/// <param name="columnName">Name of the column (property).</param>
		/// <returns>An empty string "", if the property value is valid, error string otherwise.</returns>		
		string IDataErrorInfo.this[string columnName]
		{
			get { return ValidateModelProperty(columnName); }
		} 
		#endregion		
	}
}
