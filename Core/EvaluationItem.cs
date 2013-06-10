using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// This structure keeps the number of points given and the reason for it.
    /// </summary>
    public struct EvaluationItem
    {
        /// <summary>
        /// Gets or sets the number of points.
        /// </summary>
        /// <value>
        /// The number of points.
        /// </value>
        public decimal? Points { get; set; }

        /// <summary>
        /// Gets or sets the reason for the points given.
        /// </summary>
        /// <value>
        /// The reason for the points give, e.g. "the solution lacks OO design".
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (!this.Points.HasValue)
                return "?b";
            else
            {                
                return this.Reason == null ? this.Points.Value + "b" :
                    this.Points.Value + "b (" + this.Reason + ")";
            }                
        }
    }
}
