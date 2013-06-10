using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zcu.StudentEvaluator.Core.Data.Schema
{
    /// <summary>
    /// This class contains settings for an evaluation
    /// </summary>
    public struct EvaluationItemSchema
    {
        /// <summary>
        /// Gets or sets the name of the evaluation.
        /// </summary>
        /// <value>
        /// The name of the evaluation, e.g. "Comments in code".
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the minimal number of points required to pass.
        /// </summary>
        /// <value>
        /// The number of points to pass.
        /// </value>
        public decimal? MinPoints { get; set; }

        /// <summary>
        /// Gets or sets the maximal number of points that will count.
        /// </summary>
        /// <value>
        /// The maximal number of points that counts
        /// </value>
        public decimal? MaxPoints { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder(this.Name ?? "<?>");
                           
            if (this.MinPoints.HasValue && this.MaxPoints.HasValue)
                sb.AppendFormat(" [{0}-{1}]", this.MinPoints, this.MaxPoints);
            else if (this.MinPoints.HasValue)
                sb.AppendFormat(" [min {0}b]", this.MinPoints);
            else if (this.MaxPoints.HasValue)
                sb.AppendFormat(" [max {0}b]", this.MinPoints);
                        
            return sb.ToString();
        }
    }
}
