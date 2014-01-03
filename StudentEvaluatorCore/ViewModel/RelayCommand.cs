using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Zcu.StudentEvaluator.ViewModel
{
    /// <summary>
    /// Represents a simple tool to override the body of RelayCommand.CanExecuteChanged event 
    /// and related RaiseCanExecuteChanged method from applications
    /// </summary>
    public static class RelayCommandInjector
    {
        /// <summary>
        /// Gets or sets the delegate to an action to be executed from CanExecuteChanged.Add routine.
        /// </summary>
        /// <value>
        /// The action that is to be called have one parameter which is the delegate passed in: CanExecuteChanged += delegate.
        /// </value>
        public static Action<Delegate> CanExecuteChangedAddAction { get; set; }

        /// <summary>
        /// Gets or sets the delegate to an action to be executed from CanExecuteChanged.Remove routine.
        /// </summary>
        /// <value>
        /// The action that is to be called have one parameter which is the delegate passed in: CanExecuteChanged -= delegate.
        /// </value>
        public static Action<Delegate> CanExecuteChangedRemoveAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be called from RaiseCanExecuteChangedAction.
        /// </summary>
        /// <value>
        /// The action has one parameter which will be the reference to the caller.
        /// </value>
        public static Action<ICommand> RaiseCanExecuteChangedAction { get; set; }
    }

	internal class RelayCommand : RelayCommand<object>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		public RelayCommand(Action<object> execute) : base(execute)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
			: base(execute, canExecute)
		{

		}
	}

	internal class RelayCommand<T> : ICommand
	{
		#region Fields
		readonly Action<T> _execute;
		readonly Predicate<T> _canExecute;
		#endregion // Fields

		/// <summary>
		/// Creates a new command that can always execute.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		public RelayCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter != null ? (T)parameter : default(T));
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
                _execute(parameter != null ? (T)parameter : default(T));
		}

                
        /// <summary>
        /// Internal CanExecuteChanged back-end.
        /// </summary>
        /// <remarks>
        /// When an event has add and remove accessors specified, it cannot be raised directly (compiler error).
        /// For the details, see http://csharpindepth.com/Articles/Chapter2/Events.aspx.
        /// This internal event therefore is here to hack it.
        /// </remarks>
        private event EventHandler _CanExecuteChanged;

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (this)
                {                    
                    if (RelayCommandInjector.CanExecuteChangedAddAction != null)                    
                        RelayCommandInjector.CanExecuteChangedAddAction(value);
                    else
                        this._CanExecuteChanged += value;   //this is the default implementation
                }
            }
            remove
            {
                lock (this)
                {                    
                    if (RelayCommandInjector.CanExecuteChangedRemoveAction != null)
                        RelayCommandInjector.CanExecuteChangedRemoveAction(value);                    
                    else
                        this._CanExecuteChanged -= value;   //this is the default implementation
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>		
        public void RaiseCanExecuteChanged()
        {
            if (RelayCommandInjector.RaiseCanExecuteChangedAction != null)
                RelayCommandInjector.RaiseCanExecuteChangedAction(this);
            else
            {
                //default implementation
                if (this._CanExecuteChanged != null)
                {
                    this._CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
	}
}