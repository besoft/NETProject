﻿using System.Collections.ObjectModel;
using Zcu.StudentEvaluator.Core.Collection;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Domain.Test
{
	/// <summary>
	/// Repository of Students, Categories and Evaluations that is automatically synchronized.
	/// </summary>
	/// <remarks>
	/// It is expected that the caller Adds valid items to these collections, i.e., these are not neither null nor already existing!
	/// Only one collection may change at the same time, which allows synchronization of the data in the repository with the data 
	/// changed outside (by manipulating with Student, Category, Evaluation members or directly with Students, Categories, Evaluations.
		/// </remarks>
	public class TestRepository
	{
		/// <summary>
		/// Gets the list of evaluation categories.
		/// </summary>
		/// <value>
		/// The list of categories to be contained.
		/// </value>
		public ObservableCollection<Category> Categories { get; private set; }

		/// <summary>
		/// Gets the list of students.
		/// </summary>
		/// <value>
		/// The list of students to be contained.
		/// </value>
		public ObservableCollection<Student> Students { get; private set; }
		
		/// <summary>
		/// Gets the list of evaluations.
		/// </summary>
		/// <value>
		/// The list of evaluations to be contained.
		/// </value>
		public ObservableCollection<Evaluation> Evaluations { get; private set; }        
		

		public TestRepository()
		{
			var evCol = new EvaluationObservableCollectionWithParentSync();					

			this.Categories = new CategoryObservableCollectionWithContentSync(evCol);
			this.Students = new StudentObservableCollectionWithContentSync(evCol);

			evCol.GlobalCategoryCollection = this.Categories;
			evCol.GlobalStudentCollection = this.Students;
			this.Evaluations = evCol;
			
			
			


			this.Categories.Add(new Category() { Name = "Design", MinPoints = 2m });
			this.Categories.Add(new Category() { Name = "Implementation", MinPoints = 5m, MaxPoints=10, });
			this.Categories.Add(new Category() { Name = "CodeCulture" });
			this.Categories.Add(new Category() { Name = "Documentation", MaxPoints = 2 });
			
			this.Students.Add(new Student() {PersonalNumber = "A12B0001P", FirstName="Anna", Surname="Aysle", });
			this.Students.Add(new Student() {PersonalNumber = "A12B0002P", FirstName="Barbora", Surname="Bílá", });
			this.Students.Add(new Student() {PersonalNumber = "A12B0003P", FirstName="Cyril", Surname="Cejn", });
						
						
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
