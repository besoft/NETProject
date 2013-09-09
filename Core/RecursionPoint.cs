using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Zcu.StudentEvaluator.Core
{
    /// <summary>
    /// This class is designed to prevent undesirable recursion in the custom code.
    /// </summary>
    /// <remarks>
    /// Typical use is:
    /// using (var rp = new RecursionPoint(this))
    /// {
    ///     if (rp.IsRecursive())
    ///         return; //we are in recursion, the code bellow is not to be run
    ///         
    ///     //some code calling methods that may call this method (or property) recursively
    /// }
    /// 
    /// N.B. the class is THREAD SAFE.
    /// </remarks>
    public class RecursionPoint : IDisposable
    {
        /// <summary>
        /// The static dictionary of all registered recursions
        /// </summary>
        protected static Dictionary<string, int> _recursiveCalls = new Dictionary<string,int>();

        /// <summary>
        /// The name of this recursion object
        /// </summary>
        protected string _recursiveMethodName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionPoint" /> class.
        /// </summary>
        /// <param name="caller">The instance of the caller.</param>
        /// <param name="methodName">Automatically provided by compiler.</param>
        public RecursionPoint(object caller, [CallerMemberName] string methodName = "")
        {
            Contract.Requires<ArgumentNullException>(caller != null);

            _recursiveMethodName = caller.GetType().FullName + "." + methodName;

            Enter();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionPoint" /> class.
        /// </summary>
        /// <remarks>This method is supposed to be used from static methods (with typeof(MyClass)).</remarks>
        /// <param name="typeCaller">The type instance of the caller.</param>
        /// <param name="methodName">Automatically provided by compiler.</param>
        public RecursionPoint(Type typeCaller, [CallerMemberName] string methodName = "")
        {
            Contract.Requires<ArgumentNullException>(typeCaller != null);

            _recursiveMethodName = typeCaller.FullName + "." + methodName;

            Enter();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Leave();           
        }

        /// <summary>
        /// Determines whether this instance is recursive.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is recursive; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRecursive()
        {
            return RecursiveDepth() > 0;
        }

        /// <summary>
        /// Gets the depth of the recursion.
        /// </summary>
        /// <returns>0, if there is no recursion, otherwise larger value</returns>
        public int RecursiveDepth()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            int ret = 0;

            lock (_recursiveCalls)
            {
                if (_recursiveCalls.ContainsKey(_recursiveMethodName))
                    ret = _recursiveCalls[_recursiveMethodName];
            }

            return ret;
        }

        /// <summary>
        /// Increases the depth of the recursion.
        /// </summary>
        private void Enter()
        {
            lock (_recursiveCalls)
            {
                if (_recursiveCalls.ContainsKey(_recursiveMethodName))
                    ++_recursiveCalls[_recursiveMethodName];
                else
                    _recursiveCalls[_recursiveMethodName] = 0;
            }
        }

        /// <summary>
        /// Decreases the depth of the recursion.
        /// </summary>
        private void Leave()
        {
            lock (_recursiveCalls)
            {
                if (--_recursiveCalls[_recursiveMethodName] < 0)
                    _recursiveCalls.Remove(_recursiveMethodName);
            }
        }

    }
}
