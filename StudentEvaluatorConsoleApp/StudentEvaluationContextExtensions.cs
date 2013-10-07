using System;
using System.Collections.Generic;
using System.IO;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.ConsoleApp
{
	/// <summary>
	/// Extension class
	/// </summary>
	/// <remarks>Alternative solution would be Partial Class</remarks>
	public static class StudentEvaluationContextExtensions
	{
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
		/// <param name="output">The output, e.g.., Console.Out.</param>
		public static void DumpData(this LocalStudentEvaluationContext context, string userMessage = null, TextWriter output = null)
		{			
			if (output == null)
				output = Console.Out;

			output.WriteLine("=================================");

			if (userMessage != null)
			{
				output.Write("====> ");
				output.Write(userMessage);
				output.WriteLine(" <====");
			}

			output.WriteLine("Number of students: " + context.Students.Count);
			output.WriteLine("---------------------------------");
			foreach (var st in context.Students)
			{
				output.WriteLine("{0}, {1} - evaluations: {2}",
					st.FullName, st.PersonalNumber, st.Evaluations.Count);
			}

			output.WriteLine("=================================");
			output.WriteLine();
		}
	}
}
