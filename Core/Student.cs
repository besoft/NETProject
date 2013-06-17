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
        /// Gets the personal number of the student.
        /// </summary>
        /// <value>
        /// The personal number, e.g. A12B0012P.
        /// </value>        
        public string PersonalNumber { get; private set; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name, e.g., "Josef".
        /// </value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the surname.
        /// </summary>
        /// <value>
        /// The surname, e.g., "Kohout".
        /// </value>
        public string Surname { get; private set; }

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
        /// Initializes a new instance of the <see cref="Student" /> class.
        /// </summary>
        /// <param name="personalNumber">The personal number.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <exception cref="System.ArgumentNullException">if personalNumber, firstName or surname is null.</exception>
        public Student(string personalNumber, string firstName, string surname)
        {
            if (personalNumber == null)
                throw new ArgumentNullException("personalNumber");
            if (firstName == null)
                throw new ArgumentNullException("firstName");
            if (surname == null)
                throw new ArgumentNullException("surname");

            this.PersonalNumber = personalNumber;
            this.FirstName = firstName;
            this.Surname = surname;
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
