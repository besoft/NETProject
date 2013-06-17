using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Zcu.StudentEvaluator.Core.Data.Schema;
using System.Collections.Specialized;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// Groups evaluation of individual items (and its schema)
    /// </summary>
    public class CourseEvaluation : IEvaluation
    {   
        /// <summary>
        /// Gets the collection containing evaluations (definitions + values).
        /// </summary>
        /// <remarks>The structure of the collection is derived automatically from the definition specified in 
        /// <see cref="CourseEvaluation(ObservableCollection<EvaluationDefinition>)"/> and 
        /// is automatically updated whenever the evaluation definitions change. The collection is read-only 
        /// to prevent the caller from altering its structure. Nevertheless, both the individual evaluation definition
        /// and evaluation values can be modified. </remarks>
        /// <value>
        /// The collection of evaluations.
        /// </value>
        public ReadOnlyObservableCollection<Evaluation> Evaluations { get; private set; }

        /// <summary>
        /// Gets the evaluation definitions.
        /// </summary>
        /// <value>
        /// The evaluation definitions.
        /// </value>
        public EvaluationDefinitionCollection EvaluationDefinitions { get; private set; }

        /// <summary>
        /// Gets the collection for evaluations.
        /// </summary>
        /// <remarks>This collection is created according to definitions and supports modifications unlike <see cref="Evaluations"/> .</remarks>
        protected ObservableCollection<Evaluation> _Evaluations;        

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseEvaluation" /> class with empty definition.
        /// </summary>
        public CourseEvaluation()
            : this(new EvaluationDefinitionCollection())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseEvaluation" /> class with initial definition.
        /// </summary>
        /// <param name="evalDefinitions">The collection with the definition of evaluation parameters. May not be NULL.</param>
        /// <exception cref="ArgumentNullException">If evalDefinitions is null.</exception>
        public CourseEvaluation(EvaluationDefinitionCollection evalDefinitions)
        {
            if (evalDefinitions == null)
            {
                throw new ArgumentNullException("evalDefinitions");
            }
            
            //create an empty collection of evaluation
            this._Evaluations = new ObservableCollection<Evaluation>();
            this.Evaluations = new ReadOnlyObservableCollection<Evaluation>(_Evaluations);            

            //register ourselves to definitions, so we immediately knows, when something changes
            this.EvaluationDefinitions = evalDefinitions;
            this.EvaluationDefinitions.CollectionChanged += OnEvaluationDefinitionsChanged;

            OnEvaluationDefinitionsChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Called when the evaluation definitions has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnEvaluationDefinitionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                //some new items have been added into the definitions
                case NotifyCollectionChangedAction.Add:
                    {
                        int index = 0;
                        foreach (var item in e.NewItems)
                        {
                            this._Evaluations.Insert(index + e.NewStartingIndex,
                                new Evaluation()
                                {
                                    Definition = (EvaluationDefinition)item,
                                    Value = new EvaluationValue()
                                });
                            index++;
                        }
                    }
                    break;

                //some items in the definition has moved
                case NotifyCollectionChangedAction.Move:
                    this._Evaluations.Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;

                //some items have been removed from the definitions
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        this._Evaluations.RemoveAt(e.OldStartingIndex);
                    }
                    break;

                //some items have been replaced
                case NotifyCollectionChangedAction.Replace:
                    {
                        int index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            this._Evaluations[index] = new Evaluation()
                            {
                                Definition = (EvaluationDefinition)item,
                                Value = this.Evaluations[index].Value,
                            };
                            index++;
                        }
                    }
                    break;

                //collection has been changed significantly
                case NotifyCollectionChangedAction.Reset:
                    {
                        this._Evaluations.Clear();
                        foreach (var item in this.EvaluationDefinitions)
                        {
                            this._Evaluations.Add(
                                new Evaluation()
                                {
                                    Definition = (EvaluationDefinition)item,
                                    Value = new EvaluationValue()
                                });
                        }
                        break;
                    }
            }
        }

        #region ICourseEvaluation
        /// <summary>
        /// Gets all evaluations available.
        /// </summary>
        /// <returns>
        /// Collection of evaluations.
        /// </returns>
        public IEnumerable<Evaluation> GetAllEvaluations()
        {
            return this.Evaluations;
        }

        /// <summary>
        /// Gets only the valid evaluations, i.e., evaluations with specified number of points.
        /// </summary>
        /// <returns>
        /// Collection of evaluations satisfying the required criterion.
        /// </returns>
        public IEnumerable<Evaluation> GetValidEvaluations()
        {
            return this.Evaluations.Where(x => x.Points != null);
        }

        /// <summary>
        /// Gets the missing evaluations, i.e., evaluations with not specified number of points.
        /// </summary>
        /// <returns>
        /// Collection of evaluations  satisfying the required criterion.
        /// </returns>
        public IEnumerable<Evaluation> GetMissingEvaluations()
        {
            return this.Evaluations.Where(x => x.Points == null);
        }

        /// <summary>
        /// Gets the valid evaluations that with the status "passed".
        /// </summary>
        /// <returns>
        /// Collection of evaluations  satisfying the required criterion.
        /// </returns>
        /// <remarks>
        /// A valid evaluation has the status "passed", if the number of points received is greater than or equal the minimum required.
        /// </remarks>
        public IEnumerable<Evaluation> GetHasPassedEvaluations()
        {
            return this.Evaluations.Where(x => x.HasResultPassed);
        }

        /// <summary>
        /// Gets the valid evaluations that with the status "failed".
        /// </summary>
        /// <returns>
        /// Collection of evaluations  satisfying the required criterion.
        /// </returns>
        /// <remarks>
        /// A valid evaluation has the status "failed", if the number of points received is less than the minimum required.
        /// </remarks>
        public IEnumerable<Evaluation> GetHasFailedEvaluations()
        {
            return this.Evaluations.Where(x => x.HasResultFailed);
        }

        /// <summary>
        /// Gets the total counted points.
        /// </summary>
        /// <returns>
        /// The overall number of the points (which were counted).
        /// </returns>
        public decimal? GetTotalPoints()
        {            
            decimal? ret = null;
            foreach (var item in this.Evaluations)
            {
                decimal? points = item.ValidPoints;
                if (points == null)
                    continue;

                //N.B. null + value is always null by the default but we need this to be different
                if (ret == null)
                    ret = points;
                else
                    ret += points;  //N.B. this won't be marked as Covered Block by Unit Tests because it expects 
                                    //us to test also situation with points == null, which won't happen, as such a case is impossible
            }

            return ret;
        }

        /// <summary>
        /// Gets the reason for the total points given
        /// </summary>
        /// <returns>
        /// The explanation of the evaluation given.
        /// </returns>
        public string GetTotalPointsReason()
        {            
            var sb = new StringBuilder();
            foreach (var item in this.Evaluations)
            {
                if (item.ValidPoints != null)
                    sb.Append(item.ToString()).Append(", ");
            }

            string s = sb.ToString().TrimEnd(' ', ',');
            return s.Length != 0 ? s : null;
        }
        #endregion

        #region IEvaluation
        /// <summary>
        /// Gets the name of the evaluation.
        /// </summary>
        /// <value>
        /// The name of the evaluation, e.g. "Comments in code".
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the minimal number of points required to pass.
        /// </summary>
        /// <value>
        /// The number of points to pass.
        /// </value>
        public decimal? MinPoints { get; set; }

        /// <summary>
        /// Gets the maximal number of points that will count (will be valid).
        /// </summary>
        /// <value>
        /// The maximal number of points that counts
        /// </value>
        public decimal? MaxPoints { get; set; }

        /// <summary>
        /// Gets the number of points.
        /// </summary>
        /// <value>
        /// The number of points.
        /// </value>
        public decimal? Points
        {
            get { 
                //TODO: cache it
                return GetTotalPoints(); 
            }
        }

        /// <summary>
        /// Gets the number of points that can be counted.
        /// </summary>
        /// <value>
        /// Number of points that counts, i.e., Points truncated by the available Max
        /// </value>
        public decimal? ValidPoints
        {
            get 
            {
                //TODO: cache it
                if (this.Points == null || this.MaxPoints == null)
                    return this.Points;
                else
                    return Math.Min(this.Points.Value, this.MaxPoints.Value);
            }
        }

        /// <summary>
        /// Gets the reason for the points given.
        /// </summary>
        /// <value>
        /// The reason for the points give, e.g. "the solution lacks OO design".
        /// </value>        
        public string Reason
        {
            get {
                //TODO: cache it
                return GetTotalPointsReason();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the result of this course evaluation is "Pass".
        /// </summary>
        /// <value>
        /// <c>true</c> if the evaluation result is "passed"; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Course evaluation result is "pass" if, and only if, the result of all evaluations is "pass" and the total number of points
        /// is greater than or equal the minimum requested (or, if the minimum requested is not specified).
        /// 
        /// The result of a single evaluation is "pass", if the number of received points is greater than or equal the minimum
        /// requested for that evaluation. 
        /// </remarks>
        public bool HasResultPassed
        {
            get 
            {
                //check, if individual evaluations have been passed
                foreach (var item in this.Evaluations)
                {
                    if (!item.HasResultPassed)
                        return false;
                }

                //check the overall pass
                if (this.MinPoints == null)
                    return true;
                else if (this.Points == null)
                    return false;
                else
                    return this.Points.Value >= this.MinPoints.Value;                
            }
        }

        /// <summary>
        /// Gets a value indicating whether the result of this evaluation is "Fail".
        /// </summary>
        /// <value>
        /// <c>true</c> if the evaluation result is "Fail"; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Course evaluation result is "Fail" if, and only if, the result of any evaluations is "Fail" or the total number of points
        /// is less than the minimum requested (if the minimum requested is specified).
        /// 
        /// Evaluation result is "Fail", if the number of received points is less than the minimum requested.
        /// Note that the result is also "Fail", if the number of received points is not specified whilst the minimum requested is
        /// but the result is NOT "Fail", if the minimum requested is not specified.
        /// </remarks>
        public bool HasResultFailed
        {
            get 
            {
                foreach (var item in this.Evaluations)
                {
                    if (item.HasResultFailed)
                        return true;
                }

                if (this.MinPoints == null)
                    return false;
                else if (this.Points == null)
                    return true;
                else
                    return this.Points.Value < this.MinPoints.Value;
            }
        }  
        #endregion

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            decimal? points = this.GetTotalPoints();
            if (!points.HasValue)
            {
                return "?b";
            }
            else
            {                
                return String.Format("{0}b = {1}", points.Value, 
                    this.GetTotalPointsReason());
            }
        }
    }
}
