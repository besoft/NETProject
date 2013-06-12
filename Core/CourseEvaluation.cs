using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Core.Data.Schema;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// Groups evaluation of individual items (and its schema)
    /// </summary>
    public class CourseEvaluation
    {
        /// <summary>
        /// Gets the definition for evaluation (names, min and max, ...) .
        /// </summary>
        /// <remarks>This collection is typically shared by multiple instances of this class.</remarks>
        /// <value>
        /// The collection with definitions.
        /// </value>
        public IList<EvaluationDefinition> EvaluationDefinitions { get; private set; }

        /// <summary>
        /// Gets the collection for evaluations.
        /// </summary>
        /// <remarks>This collection is created according to definitions.</remarks>
        /// <value>
        /// The collection of evaluations.
        /// </value>
        public IList<Evaluation> Evaluations { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseEvaluation" /> class.
        /// </summary>
        /// <param name="evalDefinitions">The collection with the definition of evaluation parameters. May not be NULL.</param>
        /// <exception cref="ArgumentNullException">If evalDefinitions is null.</exception>
        public CourseEvaluation(IList<EvaluationDefinition> evalDefinitions)
        {
            if (evalDefinitions == null)
            {
                throw new ArgumentNullException("evalDefinitions");
            }

            this.EvaluationDefinitions = evalDefinitions;
            this.Evaluations = new List<Evaluation>();

            //create initial "empty" evaluation
            for (int i = 0; i < this.EvaluationDefinitions.Count; i++)
            {
                this.Evaluations.Add(new Evaluation());
            }
        }

        /// <summary>
        /// Gets all evaluations.
        /// </summary>
        /// <returns>Dictionary containing definitions as keys and evaluations as values.</returns>
        public IEnumerable<KeyValuePair<EvaluationDefinition, Evaluation>> GetAllEvaluations()
        {
            var dict = new Dictionary<EvaluationDefinition, Evaluation>();
            for (int i = 0; i < this.EvaluationDefinitions.Count; i++)
            {
                dict[this.EvaluationDefinitions[i]] = this.Evaluations[i];
            }

            return dict;
        }

        /// <summary>
        /// Gets only the valid evaluations, i.e., evaluations with filled number of points.
        /// </summary>
        /// <returns>Dictionary containing definitions as keys and evaluations as values.</returns>
        public IEnumerable<KeyValuePair<EvaluationDefinition, Evaluation>> GetValidEvaluations()
        {
            var source = GetAllEvaluations();
            return source.Where(x => x.Value.Points.HasValue);
        }

        /// <summary>
        /// Gets only the blank evaluations, i.e., evaluations with not filled number of points.
        /// </summary>
        /// <returns>Dictionary containing definitions as keys and evaluations as values.</returns>
        public IEnumerable<KeyValuePair<EvaluationDefinition, Evaluation>> GetBlankEvaluations()
        {
            var source = GetAllEvaluations();
            return source.Where(x => !x.Value.Points.HasValue);
        }

        /// <summary>
        /// Checks, if the required minimal number of points has been achieved.
        /// </summary>
        /// <returns></returns>
        public bool AreMinRequirementsMet()
        {
            var source = GetAllEvaluations().Where(x => x.Key.MinPoints.HasValue);
            foreach (var item in source)
            {
                if (!item.Value.Points.HasValue ||
                    item.Value.Points.Value < item.Key.MinPoints.Value)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the total points obtained.
        /// </summary>
        /// <remarks>If points have not been entered, it returns Null value.</remarks>
        /// <value>
        /// The total points.
        /// </value>
        public decimal? GetTotalPoints()
        {
            var source = GetValidEvaluations();

            decimal? ret = null;            
            foreach (var item in source)
            {
                decimal points = Math.Min(item.Value.Points.Value, item.Key.MaxPoints ?? Decimal.MaxValue);

                //N.B. null + value is always null by the default but we need this to be different
                ret = (ret == null) ? points : ret + points;                
            }

            return ret;
        }

        /// <summary>
        /// Gets the reason for the total points given
        /// </summary>
        /// <value>
        /// The evaluation details.
        /// </value>
        public string GetTotalPointsReason()
        {
            var source = GetValidEvaluations();

            var sb = new StringBuilder();
            foreach (var item in source)
            {
                sb.AppendFormat("{0}: {1}, ", item.Key, item.Value);
            }

            return sb.ToString().TrimEnd(' ', ',');
        }

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
