using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using System.Diagnostics.CodeAnalysis;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CategoryTest
    {
        public const string CategoryName = "EvaluationCategory";
        public const decimal CategoryMinPoints = 5;
        public const decimal CategoryMaxPoints = 20;

        public static Category CreateCategoryTestInstance()
        {
            return new Category()
            {
                Name = CategoryName,
                MinPoints = CategoryMinPoints,
                MaxPoints = CategoryMaxPoints,
            };
        }

        [TestMethod]
        public void TestCategoryCtor()
        {
            var cat = new Category();
            Assert.IsNull(cat.Name);
            Assert.IsNull(cat.MinPoints);
            Assert.IsNull(cat.MaxPoints);
            Assert.IsNotNull(cat.Evaluations);
            Assert.AreEqual(0, cat.Evaluations.Count);

            cat = new Category()
            {
                Name = CategoryName,
                MinPoints = CategoryMinPoints,
                MaxPoints = CategoryMaxPoints,
            };
            Assert.AreEqual(CategoryName, cat.Name);
            Assert.AreEqual(CategoryMinPoints, cat.MinPoints);
            Assert.AreEqual(CategoryMaxPoints, cat.MaxPoints);
        }

        [TestMethod]
        public void TestCategoryToStringMethod()
        {
            //empty
            var cat = new Category();
            Assert.AreEqual(cat.ToString(), "<?>");

            cat.Name = "Category";
            cat.MinPoints = 2m;
            Assert.AreEqual(cat.ToString(), "Category [min 2b]");

            cat.MaxPoints = 4m;
            Assert.AreEqual(cat.ToString(), "Category [2-4b]");

            cat.MinPoints = null;
            Assert.AreEqual(cat.ToString(), "Category [max 4b]");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCategoryAddEvaluationWithNull()
        {
            var cat = CreateCategoryTestInstance();
            cat.AddEvaluation(null);

            Assert.Fail();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCategoryAddEvaluationWithInvalidArgument()
        {
            var cat = CreateCategoryTestInstance();
            cat.AddEvaluation(new Evaluation()
                {Category = new Category(), }
            );

            Assert.Fail();
        }

        [TestMethod]        
        public void TestCategoryAddEvaluation()
        {
            var cat = CreateCategoryTestInstance();
            var eval = new Evaluation();

            cat.AddEvaluation(eval);
            Assert.AreEqual(1, cat.Evaluations.Count);
            Assert.AreEqual(eval, cat.Evaluations[0]);
            Assert.AreEqual(cat, eval.Category);

            //try it again to check duplicities
            cat.AddEvaluation(eval);
            Assert.AreEqual(1, cat.Evaluations.Count);
            Assert.AreEqual(eval, cat.Evaluations[0]);
            Assert.AreEqual(cat, eval.Category);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCategoryRemoveEvaluationWithNull()
        {
            var cat = CreateCategoryTestInstance();
            cat.RemoveEvaluation(null);

            Assert.Fail();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCategorRemoveEvaluationWithInvalidArgument()
        {
            var cat = CreateCategoryTestInstance();
            cat.RemoveEvaluation(new Evaluation() { Category = new Category(), }
            );

            Assert.Fail();
        }

        [TestMethod]        
        public void TestCategoryRemoveEvaluation()
        {
            var cat = CreateCategoryTestInstance();
            var eval = new Evaluation();

            cat.AddEvaluation(eval);
            cat.RemoveEvaluation(eval);
                        
            Assert.AreEqual(0, cat.Evaluations.Count);
            Assert.AreEqual(null, eval.Category);

            cat.RemoveEvaluation(eval);
            Assert.AreEqual(0, cat.Evaluations.Count);
        }
    }
}
