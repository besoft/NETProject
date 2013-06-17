using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StudentCourseEvaluationTest
    {
        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        public void TestStudentCourse()
        {
            var st = new Student("1234", "Anna", "Nova");

            var sc = new StudentCourseEvaluation(st);
            Assert.AreEqual(st, sc.Student);
            Assert.AreEqual("1234\tNOVA Anna\t?b", sc.ToString());

            var eval = new CourseEvaluation();
            sc = new StudentCourseEvaluation(st, eval);
            Assert.AreEqual(st, sc.Student);
            Assert.AreEqual(eval, sc.Evaluation);
            Assert.AreEqual("1234\tNOVA Anna\t?b", sc.ToString());

            sc = new StudentCourseEvaluation("1234", "Anna", "Nova");
            Assert.AreEqual("1234\tNOVA Anna\t?b", sc.ToString());

            sc = new StudentCourseEvaluation("1234", "Anna", "Nova", eval);
            Assert.AreEqual("1234\tNOVA Anna\t?b", sc.ToString());

            var evalDef = new EvaluationDefinitionCollection();
            sc = new StudentCourseEvaluation(st, evalDef);
            Assert.AreEqual("1234\tNOVA Anna\t?b", sc.ToString());

            sc = new StudentCourseEvaluation("1234", "Anna", "Nova", evalDef);
            Assert.AreEqual("1234\tNOVA Anna\t?b", sc.ToString());
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException1()
        {
            var sc = new StudentCourseEvaluation(null);
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException2()
        {
            var st = new Student("1234", "Anna", "Nova");
            var sc = new StudentCourseEvaluation(st, (EvaluationDefinitionCollection)null);
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException3()
        {
            var sc = new StudentCourseEvaluation("1234", "Anna", "Nova", (EvaluationDefinitionCollection)null);
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException4()
        {
            var sc = new StudentCourseEvaluation(null, "Anna", "Nova", new EvaluationDefinitionCollection());
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException5()
        {
            var sc = new StudentCourseEvaluation("1234", null, "Nova", new EvaluationDefinitionCollection());
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException6()
        {
            var sc = new StudentCourseEvaluation("1234", "Nova", null, new EvaluationDefinitionCollection());
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException7()
        {
            var sc = new StudentCourseEvaluation(null, "Anna", "Nova");
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException8()
        {
            var sc = new StudentCourseEvaluation("1234", null, "Nova");
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException9()
        {
            var sc = new StudentCourseEvaluation("1234", "Nova", null);
        }

        [TestMethod]
        [TestCategory("Data.StudentCourseEvaluation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentCourseException10()
        {
            var sc = new StudentCourseEvaluation(null, new EvaluationDefinitionCollection());
        }
    }
}
