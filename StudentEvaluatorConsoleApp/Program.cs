using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.DAL;

namespace Zcu.StudentEvaluator.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var context = new StudentEvaluationContext();
			context.PopulateWithData();

			context.DumpData("Initial Repository");

			Console.WriteLine("Press ENTER to continue.");
			Console.ReadLine();
		}
	}
}
