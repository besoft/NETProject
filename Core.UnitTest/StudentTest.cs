using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StudentTest
    {
        public const string StudentPersonalNumber = "1234";
        public const string StudentFirstName = "Anna";
        public const string StudentSurname = "Nova";
        

        public static Student CreateStudentTestInstance()
        {
            return new Student()
            {
                PersonalNumber = StudentPersonalNumber,
                FirstName = StudentFirstName,
                Surname = StudentSurname,
            };
        }

        [TestMethod]
        public void TestStudentCtor()
        {
            var st = new Student();
                        
            Assert.IsNull(st.FirstName);
            Assert.IsNull(st.PersonalNumber);
            Assert.IsNull(st.Surname);
            Assert.IsNull(st.FullName);

            Assert.IsNotNull(st.Evaluations);

            st = new Student()
            {
                PersonalNumber = StudentPersonalNumber,
                FirstName = StudentFirstName,
                Surname = StudentSurname,
            };

            Assert.AreEqual(StudentPersonalNumber, st.PersonalNumber);
            Assert.AreEqual(StudentFirstName, st.FirstName);
            Assert.AreEqual(StudentSurname, st.Surname);

            Assert.AreEqual("NOVA Anna", st.FullName);
        }
           

        [TestMethod]        
        public void TestStudentToString()
        {
            var st = CreateStudentTestInstance();
            Assert.AreEqual("1234\tNOVA Anna\t\n\t", st.ToString());
            
            st.AddEvaluation(
                new Evaluation()
                {
                    Points = 2,
                    Reason = "BAD",                                        
                });

            Assert.AreEqual("1234\tNOVA Anna\t2\n\t<?>: 2b (BAD), ", st.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentAddEvaluationWithNull()
        {
            var st = CreateStudentTestInstance();
            st.AddEvaluation(null);

            Assert.Fail();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentAddEvaluationWithInvalidArgument()
        {
            var st = CreateStudentTestInstance();
            st.AddEvaluation(new Evaluation() { Student = new Student(), }
            );

            Assert.Fail();
        }

        [TestMethod]
        public void TestStudentAddEvaluation()
        {
            var st = CreateStudentTestInstance();
            var eval = new Evaluation();

            st.AddEvaluation(eval);
            Assert.AreEqual(1, st.Evaluations.Count);
            Assert.AreEqual(eval, st.Evaluations[0]);
            Assert.AreEqual(st, eval.Student);

            //try it again to check duplicities
            st.AddEvaluation(eval);
            Assert.AreEqual(1, st.Evaluations.Count);
            Assert.AreEqual(eval, st.Evaluations[0]);
            Assert.AreEqual(st, eval.Student);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestStudentRemoveEvaluationWithNull()
        {
            var st = CreateStudentTestInstance();
            st.RemoveEvaluation(null);

            Assert.Fail();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCategorRemoveEvaluationWithInvalidArgument()
        {
            var st = CreateStudentTestInstance();
            st.RemoveEvaluation(new Evaluation() { Student = new Student(), }
            );

            Assert.Fail();
        }

        [TestMethod]
        public void TestStudentRemoveEvaluation()
        {
            var st = CreateStudentTestInstance();
            var eval = new Evaluation();

            st.AddEvaluation(eval);
            st.RemoveEvaluation(eval);

            Assert.AreEqual(0, st.Evaluations.Count);
            Assert.AreEqual(null, eval.Category);

            st.RemoveEvaluation(eval);
            Assert.AreEqual(0, st.Evaluations.Count);
        }
    }
}
