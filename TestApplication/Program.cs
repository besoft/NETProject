using System;
using Zcu.StudentEvaluator.Domain.Test;


namespace TestApplication
{	 
	class Program
	{
		static void Main(string[] args)
		{
			var repo = new TestRepository();

			Console.WriteLine("Number of students: " + repo.Students.Count);
			foreach (var st in repo.Students)
			{
				Console.WriteLine(st.ToString());
			}

			repo.Students[0].Evaluations[0].Points = 5.5m;
			repo.Students[0].Evaluations[0].Reason = "Bad design";

			repo.Students[0].Evaluations[1].Points = 4m;
			repo.Students[0].Evaluations[1].Reason = "Bad implementation";

			repo.Students[0].Evaluations[2].Points = 2m;

			repo.Students[1].Evaluations[0].Points = 15m;

			
			foreach (var st in repo.Students)
			{
				Console.WriteLine(st.ToString());
			}           

			Console.ReadLine();
		}
	}
}
