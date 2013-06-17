using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Core.Data.Schema;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// Represents a dynamic data collection of StudentCourseEvaluation that provides notifications when items are added, changed, removed.
    /// </summary>
    /// <remarks>Each item may have its own definition of the evaluation. When a new Student is added or inserted, 
    /// the default definition of the evaluation is used.
    /// </remarks>
    public class StudentCourseEvaluationCollection : ObservableCollection<StudentCourseEvaluation>
    {
        /// <summary>
        /// The default definition for evaluation 
        /// </summary>
        private EvaluationDefinitionCollection defaultEvaluationDefinition;

        /// <summary>
        /// Gets or sets the default evaluation definition.
        /// </summary>
        /// <remarks>This default evaluation is used when students (<see cref="Student"/>) are being added.</remarks>
        /// <value>
        /// The evaluation definition.
        /// </value>
        public EvaluationDefinitionCollection DefaultEvaluationDefinition
        {
            get { return defaultEvaluationDefinition; }
            set 
            {                 
                defaultEvaluationDefinition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultEvaluationDefinition"));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCourseEvaluationCollection" /> class.
        /// </summary>
        /// <param name="defaultEvaluationDefinition">The default definition of evaluation. This parameter may be null.</param>
        public StudentCourseEvaluationCollection(EvaluationDefinitionCollection defaultEvaluationDefinition = null)
        {
            this.DefaultEvaluationDefinition = defaultEvaluationDefinition ?? new EvaluationDefinitionCollection();            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class that contains
        /// elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The input collection.</param>
        /// <param name="defaultEvalutionDefinition">The default evaluation definition.</param>
        /// <exception cref="System.ArgumentNullException">If collection is null.</exception>        
        public StudentCourseEvaluationCollection(IEnumerable<StudentCourseEvaluation> collection,
            EvaluationDefinitionCollection defaultEvaluationDefinition = null) : base(collection)
        {
            InitializeDefaultEvaluationDefinitions(collection, defaultEvaluationDefinition);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class that contains
        /// elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="defaultEvaluationDefinition">The default evalutaion definition.</param>
        /// <exception cref="System.ArgumentNullException">If collection is null.</exception>
        public StudentCourseEvaluationCollection(IList<StudentCourseEvaluation> collection,
            EvaluationDefinitionCollection defaultEvaluationDefinition = null) : base(collection)
        {
            InitializeDefaultEvaluationDefinitions(collection, defaultEvaluationDefinition);
        }

        /// <summary>
        /// Initializes the default evaluation definitions.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="defaultEvaluationDefinition">The default evaluation definition.</param>
        /// <remarks>
        /// This method is called from constructors to initialize <see cref="DefaultEvaluationDefinition" />
        /// </remarks>
        private void InitializeDefaultEvaluationDefinitions(IEnumerable<StudentCourseEvaluation> collection,
            EvaluationDefinitionCollection defaultEvaluationDefinition)
        {
            //if the default collection has been specified, use it
            if (defaultEvaluationDefinition != null)
                this.DefaultEvaluationDefinition = defaultEvaluationDefinition;
            else
            {
                //otherwise extract the default evaluation definition from the collection
                //as Evaluation may not be null, this will be taken from the first item                
                var item = collection.FirstOrDefault();

                this.DefaultEvaluationDefinition = (item != null) ?
                    item.Evaluation.EvaluationDefinitions : new EvaluationDefinitionCollection();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class that contains
        /// elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The input collection.</param>
        /// <param name="defaultEvalutionDefinition">The default evaluation definition.</param>
        /// <exception cref="System.ArgumentNullException">If collection is null.</exception>        
        public StudentCourseEvaluationCollection(IEnumerable<Student> collection,
            EvaluationDefinitionCollection defaultEvaluationDefinition = null)
        {
            this.DefaultEvaluationDefinition = defaultEvaluationDefinition ?? new EvaluationDefinitionCollection();
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationDefinitionCollection" /> class that contains
        /// elements copied from the given collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="defaultEvaluationDefinition">The default evalutaion definition.</param>
        /// <exception cref="System.ArgumentNullException">If collection is null.</exception>
        public StudentCourseEvaluationCollection(IList<Student> collection,
            EvaluationDefinitionCollection defaultEvaluationDefinition = null)
            : this((IEnumerable<Student>)collection, defaultEvaluationDefinition)
        {
            
        }

        /// <summary>
        /// Adds the specified item into the collection using the default evaluation definition.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">If item is null</exception>
        public void Add(Student item)
        {
            if (item == null)
                throw new ArgumentNullException();

            Add(new StudentCourseEvaluation(item, this.DefaultEvaluationDefinition));
        }

        /// <summary>
        /// Adds the specified item into the collection using the default evaluation definition.
        /// </summary>
        /// <param name="studentPersonalNumber">The student personal number.</param>
        /// <param name="studentFirstName">First name of the student.</param>
        /// <param name="studentSurname">The student surname.</param>
        /// <exception cref="System.ArgumentNullException">if studentPersonalNumber, studentFirstName or studentSurname is null.</exception>
        public void Add(string studentPersonalNumber, string studentFirstName, string studentSurname)
        {           
            Add(new StudentCourseEvaluation(
                studentPersonalNumber, studentFirstName, studentSurname, this.DefaultEvaluationDefinition));
        }

        /// <summary>
        /// Inserts the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">If the item is null.</exception>
        public void Insert(int index, Student item)
        {
            if (item == null)
                throw new ArgumentNullException();

            Insert(index, new StudentCourseEvaluation(item, this.DefaultEvaluationDefinition));
        }

        /// <summary>
        /// Inserts the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException">if studentPersonalNumber, studentFirstName or studentSurname is null.</exception>
        public void Insert(int index, string studentPersonalNumber, string studentFirstName, string studentSurname)
        {            
            Insert(index, new StudentCourseEvaluation(
                studentPersonalNumber, studentFirstName, studentSurname, this.DefaultEvaluationDefinition));                
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire collection.
        /// </summary>
        /// <param name="item">The object to locate in the collection. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire collection, if found; otherwise, -1.</returns>
        public int IndexOf(Student item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Student == item)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if the collection contains the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Student item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>true, if the item has been removed, false, otherwise.</returns>
        public bool Remove(Student item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }        
    }
}
