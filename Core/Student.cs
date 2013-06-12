using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Core.Data.Schema;

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
                return this.Surname.ToUpper() + " " + this.FirstName;
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
            return string.Format("{0}\t{1}",this.PersonalNumber, this.FullName);  
        }
    }
}
