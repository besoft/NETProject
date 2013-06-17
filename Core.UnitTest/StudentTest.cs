using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StudentTest
    {
        [TestMethod]
        [TestCategory("Data.Student")]
        public void TestStudentExceptions()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    new Student(
                        i == 0 ? null : "1234",
                        i == 1 ? null : "Anna",
                        i == 2 ? null : "Nova"
                        );
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {

                }
                catch (Exception)
                {
                    Assert.Fail();
                }
            }
        }
            

        [TestMethod]
        [TestCategory("Data.Student")]
        public void TestStudent()
        {
            var st = new Student("1234", "Anna", "Nova");
            Assert.AreEqual("1234", st.PersonalNumber);
            Assert.AreEqual("Anna", st.FirstName);
            Assert.AreEqual("Nova", st.Surname);

            Assert.AreEqual("NOVA Anna", st.FullName);
            Assert.AreEqual("1234\tNOVA Anna", st.ToString());
        }       
    }
}
