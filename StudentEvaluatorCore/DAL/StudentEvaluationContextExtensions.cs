using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;
using System.Linq;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Extension class
	/// </summary>
	/// <remarks>Alternative solution would be Partial Class</remarks>
	public static class StudentEvaluationContextExtensions
	{
        /// <summary>
        /// Populates the given work of unit with test data.
        /// </summary>
        /// <param name="workOfUnit">The work of unit.</param>
		public static void PopulateWithData(this IStudentEvaluationUnitOfWork workOfUnit)
		{
			var categories = new List<Category>
			{
				new Category() { Name = "Design", MinPoints = 2m },
				new Category() { Name = "Implementation", MinPoints = 5m, MaxPoints = 10, },
				new Category() { Name = "CodeCulture" },
				new Category() { Name = "Documentation", MaxPoints = 2 },
			};

			var students = new List<Student>
			{						
				new Student() { PersonalNumber = "A12B0001P", FirstName = "Anna", Surname = "Aysle", },
				new Student() { PersonalNumber = "A12B0002P", FirstName = "Barbora", Surname = "Bílá", },
				new Student() { PersonalNumber = "A12B0003P", FirstName = "Cyril", Surname = "Cejn", },
			};
			
			var evals = new List<Evaluation>();
			for (int i = 0, idx = 0; i < students.Count; i++)
			{
				for (int j = 0; j < categories.Count; j++, idx++)
				{
					var eval = new Evaluation()
					{
						Category = categories[j],
						Student = students[i],
					};

					evals.Add(eval);
				}
			}

			evals[0].Points = 5m;
			evals[1].Points = 9m;

			evals.ForEach(x =>
			{
				if (x.Category.Evaluations == null)
					x.Category.Evaluations = new List<Evaluation>();
				x.Category.Evaluations.Add(x);

				if (x.Student.Evaluations == null)
					x.Student.Evaluations = new List<Evaluation>();

				x.Student.Evaluations.Add(x);
			});
			
			students.ForEach(x => workOfUnit.Students.Insert(x));
			categories.ForEach(x => workOfUnit.Categories.Insert(x));
			evals.ForEach(x => workOfUnit.Evaluations.Insert(x));

			workOfUnit.Save();
		}

        /// <summary>
        /// Dumps the data in the repository using the specified output.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="output">The output, e.g.., Console.Out.</param>
        public static void DumpData(this IStudentEvaluationUnitOfWork context, string userMessage = null, TextWriter output = null)
		{
            Contract.Requires(context != null);

			if (output == null)
				output = Console.Out;

			output.WriteLine("=================================");

			if (userMessage != null)
			{
				output.Write("====> ");
				output.Write(userMessage);
				output.WriteLine(" <====");
			}

            var students = context.Students.Get();

			output.WriteLine("Number of students: " + students.Count());
			output.WriteLine("---------------------------------");
			foreach (var st in students)
			{
				output.WriteLine("{0} {1}, {2} - evaluations: {3}",
					st.Surname, st.FirstName, st.PersonalNumber, st.Evaluations.Count);
			}

			output.WriteLine("=================================");
			output.WriteLine();
		}
	}
}
