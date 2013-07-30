using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// Evaluation object describes one particular evaluation in one evaluation category (Category) for one student (Student)
    /// </summary>
    public class Evaluation
    {
        /// <summary>
        /// Gets the number of points.
        /// </summary>
        /// <value>
        /// The number of points.
        /// </value>
        public decimal? Points { get; set; }

        /// <summary>
        /// Gets the reason for the points given.
        /// </summary>
        /// <value>
        /// The reason for the points give, e.g. "the solution lacks OO design".
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the evaluation category.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the student to whom this evaluation belongs.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        public Student Student { get; set; }

        #region Category shortcuts
        /// <summary>
        /// Gets the name of the evaluation.
        /// </summary>
        /// <value>
        /// The name of the evaluation, e.g. "Comments in code".
        /// </value>
        public string Name { get { return this.Category != null ? this.Category.Name : null; } }

        /// <summary>
        /// Gets the minimal number of points required to pass.
        /// </summary>
        /// <value>
        /// The number of points to pass.
        /// </value>
        public decimal? MinPoints { get { return this.Category != null ? this.Category.MinPoints : null; } }

        /// <summary>
        /// Gets the maximal number of points that will count.
        /// </summary>
        /// <value>
        /// The maximal number of points that counts
        /// </value>
        public decimal? MaxPoints { get { return this.Category != null ? this.Category.MaxPoints : null; } }
        #endregion
        
        /// <summary>
        /// Gets the number of points that can be counted.
        /// </summary>
        /// <value>
        ///  Number of points that counts, i.e., Points truncated by the available Max
        /// </value>
        public decimal? ValidPoints {
            get
            {
                if (this.Points == null || this.MaxPoints == null)
                    return this.Points;
                else
                    return Math.Min(this.Points.Value, this.MaxPoints.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the result of this evaluation is "Pass".
        /// </summary>
        /// <remarks>Evaluation result is "pass", if the number of received points is greater than or equal the minimum requested.
        /// Note that the result is also "Pass", if the minimum requested is not specified. </remarks>
        /// <value>
        /// <c>true</c> if the evaluation result is "passed"; otherwise, <c>false</c>.
        /// </value>
        public bool HasResultPassed
        {
            get
            {
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
        /// <remarks>Evaluation result is "Fail", if the number of received points is less than the minimum requested. 
        /// Note that the result is also "Fail", if the number of received points is not specified whilst the minimum requested is
        /// but the result is NOT "Fail", if the minimum requested is not specified. </remarks>
        /// <value>
        /// <c>true</c> if the evaluation result is "Fail"; otherwise, <c>false</c>.
        /// </value>
        public bool HasResultFailed
        {
            get
            {
                if (this.MinPoints == null)
                    return false;
                else if (this.Points == null)
                    return true;
                else
                    return this.Points.Value < this.MinPoints.Value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string strValue;
            if (!this.Points.HasValue)
                strValue = "?b";
            else
            {
                strValue = this.Reason == null ? this.Points.Value + "b" :
                    this.Points.Value + "b (" + this.Reason + ")";
            }


            return (this.Category ?? new Category()).ToString() + ": " + strValue;
        }
    }
}
