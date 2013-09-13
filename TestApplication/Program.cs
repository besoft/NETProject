using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Zcu.StudentEvaluator.Domain;
using Zcu.StudentEvaluator.Domain.Test;


namespace TestApplication
{
	/// <summary>
	/// Extension class for StudentEvaluationRepository
	/// </summary>
	public static class StudentEvaluationRepositoryExtension
	{
		/// <summary>
		/// Dumps the data in the repository using the specified output.
		/// </summary>
		/// <param name="repo">The repository.</param>
		/// <param name="output">The output, e.g.., Console.Out.</param>
		public static void DumpData(this StudentEvaluationRepository repo, string userMessage = null, TextWriter output = null)
		{
			Contract.Requires(repo != null);

			if (output == null)
				output = Console.Out;

			output.WriteLine("=================================");

			if (userMessage != null)
			{
				output.Write("====> ");
				output.Write(userMessage);
				output.WriteLine(" <====");
			}

			output.WriteLine("Number of students: " + repo.Students.Count);
			output.WriteLine("---------------------------------");
			foreach (var st in repo.Students)
			{
				output.WriteLine(st.ToString());
			}

			output.WriteLine("=================================");
			output.WriteLine();
		}
	}
	


	class Program
	{
		static void Main(string[] args)
		{
			var repo = new TestStudentEvaluationRepository();
			repo.DumpData("New Repo");
			

			repo.Students[0].Evaluations[0].Points = 5.5m;
			repo.Students[0].Evaluations[0].Reason = "Bad design";

			repo.Students[0].Evaluations[1].Points = 4m;
			repo.Students[0].Evaluations[1].Reason = "Bad implementation";

			repo.Students[0].Evaluations[2].Points = 2m;

			repo.Students[1].Evaluations[0].Points = 15m;


			repo.DumpData("Modified Repo");
			

			repo.Save();

			repo.InitNew();

			repo.DumpData("Reset Repo");
			

			repo.Load();

			repo.DumpData("Loaded Modified Repo");			

			Console.ReadLine();
		}
	}
}
