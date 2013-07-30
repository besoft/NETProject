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
		public Student[] Students { get; private set; }

		/// <summary>
		/// Gets the list of evaluation categories.
		/// </summary>
		/// <value>
		/// The list of categories to be contained.
		/// </value>
		public Category[] Categories { get; private set; }

		/// <summary>
		/// Gets the list of evaluations.
		/// </summary>
		/// <value>
		/// The list of evaluations to be contained.
		/// </value>
		public Evaluation[] Evaluations { get; private set; }        
		

		public TestRepository()
		{
			const int Ccnt = 4;
			this.Categories = new Category[Ccnt]{
				new Category() {Name="Design"},
				new Category() {Name="Implementation"},
				new Category() {Name="CodeCulture"},
				new Category() {Name="Documentation"},
			};

			const int Scnt = 3;
			this.Students = new Student[Scnt]{
				new Student() {PersonalNumber = "A12B0001P", FirstName="Anna", Surname="Aysle", },
				new Student() {PersonalNumber = "A12B0002P", FirstName="Barbora", Surname="Bílá", },
				new Student() {PersonalNumber = "A12B0003P", FirstName="Cyril", Surname="Cejn", },
			};

			for (int i = 0; i < Ccnt; i++)
			{
				this.Categories[i].Evaluations = new Evaluation[Scnt];
			}

			for (int i = 0; i < Scnt; i++)
			{
				this.Students[i].Evaluations = new Evaluation[Ccnt];
			}
			
			this.Evaluations = new Evaluation[Ccnt * Scnt];
			for (int i = 0, idx = 0; i < Scnt; i++)
			{                
				for (int j = 0; j < Ccnt; j++, idx++)
				{
					this.Evaluations[idx] = new Evaluation()
					{
						Category = this.Categories[j],
						Student = this.Students[i],
					};

					this.Categories[j].Evaluations[i] = this.Evaluations[idx];
					this.Students[i].Evaluations[j] = this.Evaluations[idx];
				}
			}
		}
	}
}
