using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Zcu.StudentEvaluator.Core.Collection;
using System.Collections.ObjectModel;
using Zcu.StudentEvaluator.Core.Data;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Data.Entity;

namespace Zcu.StudentEvaluator.Domain
{
	/// <summary>
	/// Represents a repository of students and their evaluations stored in the database.
	/// </summary>
	public class DbStudentEvaluationRepository : DbContext, IStudentEvaluationRepository
	{
		/// <summary>
		/// Gets the collection of students.
		/// </summary>		
		public DbSet<Category> Students { get; private set; }

		/// <summary>
		/// Gets the collection of categories.
		/// </summary>		
		public DbSet<Category> Categories { get; private set; }

		/// <summary>
		/// Gets the collection of evaluations.
		/// </summary>
		/// <value>		
		public DbSet<Category> Evaluations { get; private set; }		
	}
}
