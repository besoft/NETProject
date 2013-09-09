using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EvaluationTest
    {       
        [TestMethod]
        public void TestEvaluationCtor()
        {
            var eval = new Evaluation();

            Assert.IsNull(eval.Category);
            
            Assert.IsNull(eval.Name);
            Assert.IsNull(eval.MaxPoints);
            Assert.IsNull(eval.MinPoints);
            Assert.IsNull(eval.Points);
            Assert.IsNull(eval.ValidPoints);
            Assert.IsNull(eval.Reason);

            Assert.IsTrue(eval.HasResultPassed);
            Assert.IsFalse(eval.HasResultFailed);            
        }

        [TestMethod]
        public void TestEvaluationWithCategory()
        {
            var eval = new Evaluation()
            {
                Category = new Category(),
            };
                        
            Assert.IsNull(eval.Name);
            Assert.IsNull(eval.MaxPoints);
            Assert.IsNull(eval.MinPoints);
            Assert.IsNull(eval.Points);
            Assert.IsNull(eval.ValidPoints);
            Assert.IsNull(eval.Reason);

            Assert.IsTrue(eval.HasResultPassed);
            Assert.IsFalse(eval.HasResultFailed);

            const string name = "Category";

            eval.Category.Name = name;
            Assert.AreEqual(eval.Category.Name, name);
            Assert.AreEqual(eval.Category.Name, eval.Name);

            decimal? minValue = 5m;
            eval.Category.MinPoints = minValue;
            Assert.AreEqual(eval.Category.MinPoints, minValue);
            Assert.AreEqual(eval.Category.MinPoints, eval.MinPoints);

            decimal? maxValue = 10m;
            eval.Category.MaxPoints = maxValue;
            Assert.AreEqual(eval.Category.MaxPoints, maxValue);
            Assert.AreEqual(eval.Category.MaxPoints, eval.MaxPoints);

            //check, if all other things are unmodified
            Assert.IsNull(eval.Points);
            Assert.IsNull(eval.ValidPoints);
            Assert.IsNull(eval.Reason);

            Assert.IsFalse(eval.HasResultPassed);
            Assert.IsTrue(eval.HasResultFailed);
        }
        
        [TestMethod]
        public void TestEvaluationWithCategoryAndValues()
        {
            var eval = CreateNewEvaluation();

            Assert.IsNull(eval.Points);
            Assert.IsNull(eval.ValidPoints);
            Assert.IsNull(eval.Reason);

            Assert.IsTrue(eval.HasResultPassed);
            Assert.IsFalse(eval.HasResultFailed);

            const string reason = "Reason";
            eval.Reason = reason;
            Assert.AreEqual(eval.Reason, reason);
            Assert.AreEqual(eval.Reason, eval.Reason);

            decimal? points = 2m;
            eval.Points = points;
            Assert.AreEqual(eval.Points, points);
            Assert.AreEqual(eval.Points, eval.Points);
            Assert.AreEqual(eval.ValidPoints, eval.Points); //no maximum set

            Assert.IsTrue(eval.HasResultPassed);    //no minimum  set
            Assert.IsFalse(eval.HasResultFailed);
            
            eval.Category.MinPoints = 5m;
            Assert.IsFalse(eval.HasResultPassed);
            Assert.IsTrue(eval.HasResultFailed);
            
            eval.Category.MaxPoints = 10m;
            eval.Points = 12m;
            Assert.IsTrue(eval.HasResultPassed);
            Assert.IsFalse(eval.HasResultFailed);
            Assert.AreEqual(eval.ValidPoints, eval.MaxPoints);
        }      

        [TestMethod]
        public void TestEvaluationToStringMethod()
        {
            //empty
            var eval = new Evaluation();
            Assert.AreEqual(eval.ToString(), "<?>: ?b");

            eval.Category = new Category()
            {
                Name = "Category",
            };

            Assert.AreEqual(eval.ToString(), "Category: ?b");

            eval.Points = 5;
            
            Assert.AreEqual(eval.ToString(), "Category: 5b");
        }

        [TestMethod]
        public void TestEvaluationCategorySetter()
        {
            var cat = CategoryTest.CreateCategoryTestInstance();
            var eval = new Evaluation();

            eval.Category = cat;
            Assert.AreEqual(cat, eval.Category);
            Assert.AreEqual(1, cat.Evaluations.Count);
            Assert.AreEqual(eval, cat.Evaluations[0]);

            var cat2 = CategoryTest.CreateCategoryTestInstance();
            eval.Category = cat2;
            Assert.AreEqual(cat2, eval.Category);
            Assert.AreEqual(0, cat.Evaluations.Count);
            Assert.AreEqual(1, cat2.Evaluations.Count);
            Assert.AreEqual(eval, cat2.Evaluations[0]);

            eval.Category = null;
            Assert.AreEqual(null, eval.Category);            
            Assert.AreEqual(0, cat2.Evaluations.Count);
        }

        [TestMethod]
        public void TestEvaluationStudentSetter()
        {
            var st = StudentTest.CreateStudentTestInstance();
            var eval = new Evaluation();

            eval.Student = st;
            Assert.AreEqual(st, eval.Student);
            Assert.AreEqual(1, st.Evaluations.Count);
            Assert.AreEqual(eval, st.Evaluations[0]);

            var st2 = StudentTest.CreateStudentTestInstance();
            eval.Student = st2;
            Assert.AreEqual(st2, eval.Student);
            Assert.AreEqual(0, st.Evaluations.Count);
            Assert.AreEqual(1, st2.Evaluations.Count);
            Assert.AreEqual(eval, st2.Evaluations[0]);

            eval.Student = null;
            Assert.AreEqual(null, eval.Student);
            Assert.AreEqual(0, st2.Evaluations.Count);
        }


        private Evaluation CreateNewEvaluation(decimal? minPoints = null, decimal? maxPoints = null)
        {            
            return new Evaluation()
            {
                Category = new Category() 
                { 
                    Name = "EvaluationCategory",
                    MinPoints = minPoints,
                    MaxPoints = maxPoints,
                },                
            };
        }      
    }
}
