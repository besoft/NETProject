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
        [TestMethod]
        public void TestDefaultValues()
        {
            var st = new Student();
                        
            Assert.IsNull(st.FirstName);
            Assert.IsNull(st.PersonalNumber);
            Assert.IsNull(st.Surname);
            Assert.IsNull(st.FullName);

            Assert.IsNull(st.Evaluations);
        }
           

        [TestMethod]
        [TestCategory("Data.Student")]
        public void TestStudent()
        {
            var st = new Student()
            {
                PersonalNumber = "1234", 
                FirstName = "Anna", 
                Surname = "Nova",
            };

            Assert.AreEqual("1234", st.PersonalNumber);
            Assert.AreEqual("Anna", st.FirstName);
            Assert.AreEqual("Nova", st.Surname);

            Assert.AreEqual("NOVA Anna", st.FullName);
            Assert.AreEqual("1234\tNOVA Anna\t\n\t", st.ToString());

            st.Evaluations = new System.Collections.Generic.List<Evaluation>();
            st.Evaluations.Add(
                new Evaluation()
                {
                    Points = 2,
                    Reason = "BAD",                                        
                });

            Assert.AreEqual("1234\tNOVA Anna\t2\n\t<?>: 2b (BAD), ", st.ToString());
        }       
    }
}
