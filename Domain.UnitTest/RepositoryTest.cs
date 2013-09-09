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
            var repo = new TestRepository();
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
            Assert.AreEqual(st4, repo.Students[4]);
            Assert.AreEqual(5, repo.Categories.Count);
            Assert.AreEqual(5, repo.Students.Count);
            Assert.AreEqual(12, repo.Evaluations.Count);

            //remove simplex
            repo.Students.RemoveAt(4);
            Assert.AreEqual(5, repo.Categories.Count);
            Assert.AreEqual(4, repo.Students.Count);
            Assert.AreEqual(12, repo.Evaluations.Count);

            //remove complex
            repo.Students.RemoveAt(0);
            Assert.AreEqual(5, repo.Categories.Count);
            Assert.AreEqual(3, repo.Students.Count);
            Assert.AreEqual(8, repo.Evaluations.Count);

            //reset
            repo.Students.Clear();
            Assert.AreEqual(5, repo.Categories.Count);
            Assert.AreEqual(0, repo.Students.Count);
            Assert.AreEqual(0, repo.Evaluations.Count);
        }

        [TestMethod]
        public void TestCategoriesManipulation()
        {
            var repo = new TestRepository();
            Assert.AreEqual(4, repo.Categories.Count);
            Assert.AreEqual(3, repo.Students.Count);
            Assert.AreEqual(12, repo.Evaluations.Count);

            //simple add
            var cat1 = new Category()
            {
                Name = "Cat1"                
            };

            repo.Categories.Add(cat1);
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
            Assert.AreEqual(cat4, repo.Categories[5]);
            Assert.AreEqual(6, repo.Categories.Count);
            Assert.AreEqual(4, repo.Students.Count);
            Assert.AreEqual(12, repo.Evaluations.Count);

            //remove simplex
            repo.Categories.RemoveAt(5);
            Assert.AreEqual(5, repo.Categories.Count);
            Assert.AreEqual(4, repo.Students.Count);
            Assert.AreEqual(12, repo.Evaluations.Count);

            //remove complex
            repo.Categories.RemoveAt(0);
            Assert.AreEqual(4, repo.Categories.Count);
            Assert.AreEqual(4, repo.Students.Count);
            Assert.AreEqual(9, repo.Evaluations.Count);

            //reset
            repo.Categories.Clear();
            Assert.AreEqual(0, repo.Categories.Count);
            Assert.AreEqual(4, repo.Students.Count);
            Assert.AreEqual(0, repo.Evaluations.Count);
        }

        [TestMethod]
        public void TestEvaluationsManipulation()
        {
            var repo = new TestRepository();
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
            var repo = new TestRepository();

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
        }
    }
}
