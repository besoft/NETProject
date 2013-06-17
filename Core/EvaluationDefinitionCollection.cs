using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Core.Data.Schema;

namespace Zcu.StudentEvaluator.Core.Data.Schema
{
    /// <summary>
    /// Collection of single evaluation definitions.
    /// </summary>    
    public class EvaluationDefinitionCollection : ObservableCollection<EvaluationDefinition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class.
        /// </summary>
        public EvaluationDefinitionCollection()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class that contains 
        /// elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public EvaluationDefinitionCollection(IEnumerable<EvaluationDefinition> collection)
            : base(collection)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class that contains 
        /// elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public EvaluationDefinitionCollection(IList<EvaluationDefinition> collection)
            : base(collection)
        {

        }        
    }
}
