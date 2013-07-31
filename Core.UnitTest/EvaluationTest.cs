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
        public void TestDefaultValues()
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
        public void TestCategory()
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
        public void TestEvaluationValue()
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
        public void TestCategoryToStringMethod()
        {
            //empty
            var eval = new Category();
            Assert.AreEqual(eval.ToString(), "<?>");
            
            eval.Name = "Category";
            Assert.AreEqual(eval.ToString(), "Category");

            eval.MinPoints = 2m;
            Assert.AreEqual(eval.ToString(), "Category [min 2b]");

            eval.MaxPoints = 4m;
            Assert.AreEqual(eval.ToString(), "Category [2-4b]");

            eval.MinPoints = null;
            Assert.AreEqual(eval.ToString(), "Category [max 4b]");
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
