using System.Collections.ObjectModel;
using Zcu.StudentEvaluator.Core.Collection;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Domain.Test
{
	/// <summary>
	/// This is a test repository with initial data created.
	/// </summary>		
	public class TestStudentEvaluationRepository : DbStudentEvaluationRepository
	{		    		
		public TestStudentEvaluationRepository()
		{
			PopulateRepository();					
		}

		/// <summary>
		/// Called after the repository has been initialized.
		/// </summary>
		protected override void OnRepositoryInitialized()
		{
			PopulateRepository();
		}

		/// <summary>
		/// Populates the repository with the test data.
		/// </summary>
		private void PopulateRepository()
		{
			this.Categories.Add(new Category() { Name = "Design", MinPoints = 2m });
			this.Categories.Add(new Category() { Name = "Implementation", MinPoints = 5m, MaxPoints = 10, });
			this.Categories.Add(new Category() { Name = "CodeCulture" });
			this.Categories.Add(new Category() { Name = "Documentation", MaxPoints = 2 });

			this.Students.Add(new Student() { PersonalNumber = "A12B0001P", FirstName = "Anna", Surname = "Aysle", });
			this.Students.Add(new Student() { PersonalNumber = "A12B0002P", FirstName = "Barbora", Surname = "Bílá", });
			this.Students.Add(new Student() { PersonalNumber = "A12B0003P", FirstName = "Cyril", Surname = "Cejn", });


			for (int i = 0, idx = 0; i < this.Students.Count; i++)
			{
				for (int j = 0; j < this.Categories.Count; j++, idx++)
				{
					var eval = new Evaluation()
					{
						Category = this.Categories[j],
						Student = this.Students[i],
					};

					//this.Evaluations.Add(eval);	
				}
			}
		}		
	}	
}
