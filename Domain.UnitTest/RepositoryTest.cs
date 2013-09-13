using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

using Zcu.StudentEvaluator.Domain;
using Zcu.StudentEvaluator.Domain.Test;
using Zcu.StudentEvaluator.Core.Data;

namespace Zcu.StudentEvaluator.Domain.UnitTest
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class RepositoryTest
	{
		[TestMethod]
		public void TestStudentsManipulation()
		{
			var repo = new TestStudentEvaluationRepository();
			//4 categories, 3 students, each student having one evaluation for every category
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);

			//simple add
			var st1 = new Student()
			{
				PersonalNumber = "8888",
				FirstName = "F1",
				Surname = "S1",
			};
			
			repo.Students.Add(st1);
			//+1 new student without evaluation =>
			//4 categories, 4 students (one without any evaluation), 12 evaluations
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);
			Assert.AreEqual(st1, repo.Students[3]);

			//complex add
			var st2 = new Student()
			{
				PersonalNumber = "9999",
				FirstName = "F2",
				Surname = "S2",                
			};

			st2.AddEvaluation(new Evaluation()
				{
					Category = new Category() { Name = "Category1"}
				}
				);

			repo.Students.Add(st2);
			//+1 new student with evaluation with assigned new category (+1 category) =>			
			//5 categories, 5 students (one without any evaluation), 13 evaluations
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			Assert.AreEqual(st1, repo.Students[3]);
			Assert.AreEqual(st2, repo.Students[4]);
			Assert.AreEqual(st2.Evaluations[0], repo.Evaluations[12]);
			Assert.AreEqual(st2, st2.Evaluations[0].Student);
			Assert.AreEqual(st2.Evaluations[0].Category, repo.Categories[4]);

			//replace simple
			var st3 = new Student()
			{
				PersonalNumber = "1145",
				FirstName = "F3",
				Surname = "S3",
			};

			repo.Students[3] = st3;
			//the student without any evaluation replaced by a new student without any evaluation =>			
			//5 categories, 5 students (one without any evaluation), 13 evaluations
			Assert.AreEqual(st3, repo.Students[3]);
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//replace complex
			var st4 = new Student()
			{
				PersonalNumber = "225",
				FirstName = "F4",
				Surname = "S4",
			};

			repo.Students[4] = st4;
			//the student with evaluation (with assigned category) replaced by a new student without any evaluation =>			
			//5 categories, 5 students (2 without any evaluation), 13 evaluations (one belonging to a category only)
			Assert.AreEqual(st4, repo.Students[4]);
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//remove simplex
			repo.Students.RemoveAt(4);
			//the student without any evaluation removed =>			
			//5 categories, 4 students (2 without any evaluation), 13 evaluations (one belonging to a category only)
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//remove complex
			repo.Students.RemoveAt(0);
			//the student with evaluations for every initial category removed =>			
			//5 categories, 3 students (2 without any evaluation), 13 evaluations (5 belonging to a category only)
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//complex add 2
			var st5 = new Student()
			{
				Surname = "StWithEvalWithoutCat"
			};

			st5.AddEvaluation(new Evaluation());

			repo.Students.Add(st5);
			//+1 student, +1 evaluation =>
			//5 categories, 4 students (2 without any evaluation), 14 evaluations (5 belonging to a category only, 1 without category)
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(14, repo.Evaluations.Count);

			//reset
			repo.Students.Clear();
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(0, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);
		}

		[TestMethod]
		public void TestCategoriesManipulation()
		{
			var repo = new TestStudentEvaluationRepository();
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);

			//4 categories, 3 students, each student has one evaluation for every category

			//simple add
			var cat1 = new Category()
			{
				Name = "Cat1"                
			};

			repo.Categories.Add(cat1);
			//4 categories, 3 students, each student has one evaluation for every category
			//+ 1 new category without any evaluation
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);
			Assert.AreEqual(cat1, repo.Categories[4]);

			//complex add
			var cat2 = new Category()
			{
				Name = "Category1"                
			};

			cat2.AddEvaluation(new Evaluation()
			{
				Student = new Student() {
					PersonalNumber = "9999",
					FirstName = "F2",
					Surname = "S2",
				}
			}
				);

			repo.Categories.Add(cat2);
			//4 categories, 3 students, each student has one evaluation for every category
			//+ 1 category without any evaluation
			//+1 new category with one evaluation for a new student =>
			//6 categories, 4 students, 13 evaluations
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);
			
			Assert.AreEqual(cat2, repo.Categories[5]);
			Assert.AreEqual(cat2.Evaluations[0], repo.Evaluations[12]);
			Assert.AreEqual(cat2.Evaluations[0].Student, repo.Students[3]);
			
			//replace simple
			var cat3 = new Category()
			{
				Name = "1145",                
			};

			repo.Categories[4] = cat3;
			//the category without any evaluation was replaced by a category without any evaluation
			//no significant change => 6 categories, 4 students, 13 evaluations
			Assert.AreEqual(cat3, repo.Categories[4]);
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//replace complex
			var cat4 = new Category()
			{
				Name = "225",                
			};

			repo.Categories[5] = cat4;
			//the category with one evaluation assigned to a student was replaced by a category without any evaluation
			//=> 6 categories, 4 students, 13 evaluations (one is no longer belonging to any category)
			Assert.AreEqual(cat4, repo.Categories[5]);
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//remove simplex
			repo.Categories.RemoveAt(5);
			//the category without any evaluation removed =>
			//=> 5 categories, 4 students, 13 evaluations (one is no longer belonging to any category)
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//remove complex
			repo.Categories.RemoveAt(0);
			//the category with evaluations for each of the initial 3 students is removed removed
			//but students are still valid in the repository =>
			//=> 4 categories, 4 students, 13 evaluations (4 is no longer belonging to any category)
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//complex add 2
			var cat5 = new Category()
			{
				Name = "CatWithEvalWithoutStudent"
			};

			cat5.AddEvaluation(new Evaluation());

			repo.Categories.Add(cat5);
			//+1 category, +1 evaluation =>
			//5 categories, 4 students, 14 evaluations (4 no longer belonging to any category, 1 no longer belonging to any student)
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(14, repo.Evaluations.Count);
			
			//reset
			repo.Categories.Clear();
			//every category is removed, 13 evaluations belonging to 4 students
			Assert.AreEqual(0, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);
		}

		[TestMethod]
		public void TestEvaluationsManipulation()
		{
			var repo = new TestStudentEvaluationRepository();
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);

			//simple add
			var cat1 = new Evaluation();

			repo.Evaluations.Add(cat1);
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);
			Assert.AreEqual(cat1, repo.Evaluations[12]);

			//complex add
			var cat2 = new Evaluation()
			{
				Student = new Student(),
				Category = new Category()
				{
					Name = "Category1"
				}
			};

		   
			repo.Evaluations.Add(cat2);
			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual(4, repo.Students.Count);
			Assert.AreEqual(14, repo.Evaluations.Count);           
		  
			//replace complex
			var cat4 = new Evaluation()
			{
				Student = new Student(),
				Category = new Category()
				{
					Name = "Category2"
				}
			};

			repo.Evaluations[13] = cat4;
			Assert.AreEqual(cat4, repo.Evaluations[13]);
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(14, repo.Evaluations.Count);

			//remove complex
			repo.Evaluations.RemoveAt(13);
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			//reset
			repo.Evaluations.Clear();
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(0, repo.Evaluations.Count);
		}

		[TestMethod]
		public void TestComplexChanges()
		{
			var repo = new TestStudentEvaluationRepository();

			var st = repo.Students[0];
			st.Evaluations.Clear();

			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);

			st.Evaluations.Add(
				new Evaluation()
				{
					Category = repo.Categories[0],
				});

			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			st.Evaluations[0].Category = new Category()
			{
				Name = "Added",
			};

			Assert.AreEqual(5, repo.Categories.Count);
			Assert.AreEqual("Added", repo.Categories[4].Name);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(13, repo.Evaluations.Count);

			var cat = new Category()
			{
				Name = "Added2",
			};

			cat.Evaluations.Add(new Evaluation()    //no student
				{
					Points = 5,
				});

			cat.Evaluations.Add(new Evaluation()    //existing student
			{
				Points = 6,
				Student = repo.Students[1], 
			});

			Assert.AreEqual(6, repo.Categories.Count);          //"Added 2"
			Assert.AreEqual("Added2", repo.Categories[5].Name);            
			Assert.AreEqual(3, repo.Students.Count);            //no new student            
			Assert.AreEqual(15, repo.Evaluations.Count);        //2 new Evaluations

			cat.Evaluations.Add(new Evaluation()    //new student
			{
				Points = 6,
				Student = new Student()
				{
					FirstName = "Andrea"
				},
			});

			//new student with other evaluations
			var stNew = new Student()
			{
				FirstName = "Bara",
			};

			stNew.Evaluations.Add(new Evaluation()
				{
					Points = 7,     //no parent
				});

			stNew.Evaluations.Add(new Evaluation()
			{
				Points = 8,     //existing parent
				Category = repo.Categories[1],
			});

			stNew.Evaluations.Add(new Evaluation()
			{
				Points = 8,     //new parent
				Category = new Category()
				{
				   Name = "ComplexCategory"
				}
			});

			//new student with other evaluations - continue
			cat.Evaluations.Add(new Evaluation()
			{
				Points = 6,
				Student = stNew             
			});

			st.Evaluations[0].Category = cat;

			Assert.AreEqual(7, repo.Categories.Count);  //2 new categories created
			Assert.AreEqual("Added2", repo.Categories[5].Name);
			Assert.AreEqual("ComplexCategory", repo.Categories[6].Name);
			Assert.AreEqual(5, repo.Students.Count);    //2 new students, Andrea and Bara
			Assert.AreEqual("Andrea", repo.Students[3].FirstName);
			Assert.AreEqual("Bara", repo.Students[4].FirstName);
			Assert.AreEqual(20, repo.Evaluations.Count);    //7 new Evaluations

			repo.Categories.Clear();
			Assert.AreEqual(0, repo.Categories.Count);
			foreach (var item in repo.Evaluations)
			{
				Assert.AreEqual(null, item.Category);
			}

			int evalCnt = repo.Evaluations.Count;
			repo.Evaluations[evalCnt - 1].Student = null;
			Assert.AreEqual(evalCnt - 1, repo.Evaluations.Count);

			repo.Evaluations[evalCnt - 2].Student = new Student();			
			Assert.AreEqual(evalCnt - 1, repo.Evaluations.Count);
			Assert.AreEqual(6, repo.Students.Count);

			repo.Students.Clear();
			Assert.AreEqual(0, repo.Students.Count);
			Assert.AreEqual(0, repo.Categories.Count);
			Assert.AreEqual(0, repo.Evaluations.Count);
		}

		[TestMethod]
		public void TestComplexChanges2()
		{
			var repo = new TestStudentEvaluationRepository();
			Assert.AreEqual(4, repo.Categories.Count);
			Assert.AreEqual(3, repo.Students.Count);
			Assert.AreEqual(12, repo.Evaluations.Count);

			//adding a new category with tree evaluations:
			//one without any student assigned
			//one for a student already in the collection
			//one for a new student that will have two new evaluations
			//- one for another new category that will have one new evaluation for the third new student
			//- one for a category already in the collection

			var catExisting = repo.Categories[0];
			var catNew = new Category()
			{
				Name = "CatWithEvalFor3rdSt",
			};

			catNew.Evaluations.Add(new Evaluation()
				{
					Points = 101,
					Student = new Student() { Surname = "3rdStudent"}
				});

			var stNew = new Student()
			{
				Surname = "2ndStudent",
			};

			stNew.Evaluations.Add(new Evaluation() { Category = catNew, Points = 102, });
			stNew.Evaluations.Add(new Evaluation() { Category = catExisting, Points = 103, });
			//Category is set to some category in the repository => when catExisting.Evaluations is modified,
			//OnCollectionChanged is issued => this new Evaluation is added into the repository
			//Upon stNew.Evaluations.Add, we are thus adding an evaluation (103) that is already in the repository =>
			//when Ev103.Student is set to stNew, OnPropertyChanged is issued => 
			//stNew is added into the repository => its evaluations are added into the repository =>
			//Ev102 is added and upon its adding, its Student and its Category (its parents) are to be add:
			//stNew is already in the repository, so it is skipped, catNew is not there, i.e., it is to be add.
			//catNew is added => its evaluations are to be added

			var catNewToAdd = new Category() { Name = "CatComplex"};
			catNewToAdd.Evaluations.Add(new Evaluation() { Points = 104, });
			catNewToAdd.Evaluations.Add(new Evaluation()
				{
					Student = repo.Students[0],
					Points = 105, 
				});
			catNewToAdd.Evaluations.Add(new Evaluation()
				{
					Student = stNew,
					Points = 106, 
				});

			//actually catNewToAdd is already included in the repository, so
			//there is 6 categories, 5 students, 18 evaluations
			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(18, repo.Evaluations.Count);

			//check links
			try
			{
				repo.Categories.Add(catNewToAdd);
				Assert.Fail("Adding an item already in the repository should throw an exception.");
			}
			catch (ArgumentException)
			{ 

			}			

			Assert.AreEqual(6, repo.Categories.Count);
			Assert.AreEqual(5, repo.Students.Count);
			Assert.AreEqual(18, repo.Evaluations.Count);
		}
	}
}
