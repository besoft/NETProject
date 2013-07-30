using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// This structure keeps the number of points given and the reason for it.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Gets or sets the personal number of the student.
        /// </summary>
        /// <value>
        /// The personal number, e.g. A12B0012P.
        /// </value>
        public string PersonalNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name, e.g., "Josef".
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname, e.g., "Kohout".
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets the full name of the student.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName {
            get
            {
                return Surname.ToUpper() + " " + FirstName;
            }
        }

        /// <summary>
        /// Gets or sets the individual student evaluation.
        /// </summary>
        /// <remarks>This collection is private to each student.</remarks>
        /// <value>
        /// The evaluation.
        /// </value>
        public Evaluation[] Evaluations { get; set; }

        
        /// <summary>
        /// Gets the total points the student obtained.
        /// </summary>
        /// <value>
        /// The total points.
        /// </value>
        public decimal? TotalPoints
        {
            get
            {
                decimal? sum = null;
                foreach (var item in this.Evaluations)
                {
                    if (sum == null)
                        sum = item.ValidPoints;
                    else
                        sum += item.ValidPoints ?? 0;
                }

                return sum;
            }
        }

        /// <summary>
        /// Gets the reason for the total points given
        /// </summary>
        /// <value>
        /// The evaluation details.
        /// </value>
        public string TotalPointsReason { 
            get
            {
                var sb = new StringBuilder();
                foreach (var item in this.Evaluations)
                {
                    sb.AppendFormat("{0}, ", item);
                }

                return sb.ToString();
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
            return string.Format("{0}\t{1}\t{2}\n\t{3}",
                this.PersonalNumber, this.FullName, this.TotalPoints, this.TotalPointsReason);
        }
    }
}
