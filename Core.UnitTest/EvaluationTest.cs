using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;
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

            Assert.IsNull(eval.Definition);
            Assert.IsNull(eval.Value);

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
        public void TestEvaluationDefinition()
        {
            var eval = new Evaluation()
            {
                Definition =  new EvaluationDefinition(),
            };
            
            Assert.IsNull(eval.Value);

            Assert.IsNull(eval.Name);
            Assert.IsNull(eval.MaxPoints);
            Assert.IsNull(eval.MinPoints);
            Assert.IsNull(eval.Points);
            Assert.IsNull(eval.ValidPoints);
            Assert.IsNull(eval.Reason);

            Assert.IsTrue(eval.HasResultPassed);
            Assert.IsFalse(eval.HasResultFailed);

            const string name = "Category";

            eval.Definition.Name = name;
            Assert.AreEqual(eval.Definition.Name, name);
            Assert.AreEqual(eval.Definition.Name, eval.Name);

            decimal? minValue = 5m;
            eval.Definition.MinPoints = minValue;
            Assert.AreEqual(eval.Definition.MinPoints, minValue);
            Assert.AreEqual(eval.Definition.MinPoints, eval.MinPoints);

            decimal? maxValue = 10m;
            eval.Definition.MaxPoints = maxValue;
            Assert.AreEqual(eval.Definition.MaxPoints, maxValue);
            Assert.AreEqual(eval.Definition.MaxPoints, eval.MaxPoints);

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
            eval.Value.Reason = reason;
            Assert.AreEqual(eval.Value.Reason, reason);
            Assert.AreEqual(eval.Value.Reason, eval.Reason);

            decimal? points = 2m;
            eval.Value.Points = points;
            Assert.AreEqual(eval.Value.Points, points);
            Assert.AreEqual(eval.Value.Points, eval.Points);
            Assert.AreEqual(eval.ValidPoints, eval.Points); //no maximum set

            Assert.IsTrue(eval.HasResultPassed);    //no minimum  set
            Assert.IsFalse(eval.HasResultFailed);
            
            eval.Definition.MinPoints = 5m;
            Assert.IsFalse(eval.HasResultPassed);
            Assert.IsTrue(eval.HasResultFailed);
            
            eval.Definition.MaxPoints = 10m;
            eval.Value.Points = 12m;
            Assert.IsTrue(eval.HasResultPassed);
            Assert.IsFalse(eval.HasResultFailed);
            Assert.AreEqual(eval.ValidPoints, eval.MaxPoints);
        }

        [TestMethod]
        public void TestEvaluationDefinitionToStringMethod()
        {
            //empty
            var eval = new EvaluationDefinition();
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
        public void TestEvaluationValueToStringMethod()
        {
            //empty
            var eval = new EvaluationValue();
            Assert.AreEqual(eval.ToString(), "?b");

            eval.Points = 3m;
            Assert.AreEqual(eval.ToString(), "3b");

            eval.Reason = "Reason";

            Assert.AreEqual(eval.ToString(), "3b (Reason)");
        }

        [TestMethod]
        public void TestEvaluationToStringMethod()
        {
            //empty
            var eval = new Evaluation();
            Assert.AreEqual(eval.ToString(), "<?>: ?b");

            eval.Definition = new EvaluationDefinition()
            {
                Name = "Category",
            };

            Assert.AreEqual(eval.ToString(), "Category: ?b");

            eval.Value = new EvaluationValue()
            {
                Points = 5,
            };

            Assert.AreEqual(eval.ToString(), "Category: 5b");
        }

        private Evaluation CreateNewEvaluation(decimal? minPoints = null, decimal? maxPoints = null)
        {            
            return new Evaluation()
            {
                Definition = new EvaluationDefinition() 
                { 
                    Name = "EvaluationCategory",
                    MinPoints = minPoints,
                    MaxPoints = maxPoints,
                },

                Value = new EvaluationValue()
            };
        }      
    }
}
