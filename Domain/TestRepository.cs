using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Domain;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Domain.Test
{
	public class TestRepository
	{        
		/// <summary>
		/// Gets the list of students.
		/// </summary>
		/// <value>
		/// The list of students to be contained.
		/// </value>
		public List<Student> Students { get; private set; }

		/// <summary>
		/// Gets the list of evaluation categories.
		/// </summary>
		/// <value>
		/// The list of categories to be contained.
		/// </value>
		public List<Category> Categories { get; private set; }

		/// <summary>
		/// Gets the list of evaluations.
		/// </summary>
		/// <value>
		/// The list of evaluations to be contained.
		/// </value>
		public List<Evaluation> Evaluations { get; private set; }        
		

		public TestRepository()
		{
			this.Categories = new List<Category>();
			this.Categories.Add(new Category() { Name = "Design", MinPoints = 2m });
			this.Categories.Add(new Category() { Name = "Implementation", MinPoints = 5m, MaxPoints=10, });
			this.Categories.Add(new Category() { Name = "CodeCulture" });
			this.Categories.Add(new Category() { Name = "Documentation", MaxPoints = 2 });			
			
			this.Students = new List<Student>();
			this.Students.Add(new Student() {PersonalNumber = "A12B0001P", FirstName="Anna", Surname="Aysle", });
			this.Students.Add(new Student() {PersonalNumber = "A12B0002P", FirstName="Barbora", Surname="Bílá", });
			this.Students.Add(new Student() {PersonalNumber = "A12B0003P", FirstName="Cyril", Surname="Cejn", });
			
			for (int i = 0; i < this.Categories.Count; i++)
			{
				this.Categories[i].Evaluations = new List<Evaluation>();
			}

			for (int i = 0; i < this.Students.Count; i++)
			{
				this.Students[i].Evaluations = new List<Evaluation>();
			}
			
			this.Evaluations = new List<Evaluation>();
			for (int i = 0, idx = 0; i < this.Students.Count; i++)
			{
				for (int j = 0; j < this.Categories.Count; j++, idx++)
				{
					var eval = new Evaluation()
					{
						Category = this.Categories[j],
						Student = this.Students[i],
					};

					this.Evaluations.Add(eval);
					this.Categories[j].Evaluations.Add(eval);
					this.Students[i].Evaluations.Add(eval);
				}
			}
		}
	}
}
