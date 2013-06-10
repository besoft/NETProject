using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Domain
{
	public interface IRepository
	{
        /// <summary>
        /// Gets the list of students.
        /// </summary>
        /// <value>
        /// The list of students to be contained.
        /// </value>
		Student[] Students { get; set; }
	}    
}
