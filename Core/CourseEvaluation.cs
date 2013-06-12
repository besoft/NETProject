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
    public class CourseEvaluation : ICourseEvaluation
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
        public ObservableCollection<EvaluationDefinition> EvaluationDefinitions { get; private set; }

        /// <summary>
        /// Gets the collection for evaluations.
        /// </summary>
        /// <remarks>This collection is created according to definitions and supports modifications unlike <see cref="Evaluations"/> .</remarks>
        protected ObservableCollection<Evaluation> _Evaluations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseEvaluation" /> class.
        /// </summary>
        /// <param name="evalDefinitions">The collection with the definition of evaluation parameters. May not be NULL.</param>
        /// <exception cref="ArgumentNullException">If evalDefinitions is null.</exception>
        public CourseEvaluation(ObservableCollection<EvaluationDefinition> evalDefinitions)
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
                ret = (ret == null) ? points : ret + points;
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

            return sb.ToString().TrimEnd(' ', ',');
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
                string reason = this.GetTotalPointsReason();
                return reason.Length == 0 ? points.Value + "b" :
                    String.Format("{0}b = {1}", points.Value, reason);
            }
        }       
    }
}
