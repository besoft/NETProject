using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;
using System.Collections.Generic;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StudentCourseEvaluationCollectionTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            var scol = new StudentCourseEvaluationCollection();
            Assert.IsNotNull(scol.DefaultEvaluationDefinition);
            Assert.AreEqual(0, scol.DefaultEvaluationDefinition.Count);

            var defs = CreateDefinition();
            scol = new StudentCourseEvaluationCollection(defs);            
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);

            var list = new List<StudentCourseEvaluation>();
            list.Add(new StudentCourseEvaluation("1234", "Anna", "Nova", defs));
            list.Add(new StudentCourseEvaluation("4321", "Bara", "Nova", defs));

            //IList
            scol = new StudentCourseEvaluationCollection(list, defs);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //IEnumerable
            scol = new StudentCourseEvaluationCollection((IEnumerable<StudentCourseEvaluation>)list, defs);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //IList
            scol = new StudentCourseEvaluationCollection(list);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //IEnumerable, automatic definition
            scol = new StudentCourseEvaluationCollection((IEnumerable<StudentCourseEvaluation>)list);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //Test with empty initial collection
            list.Clear();
            //IList
            scol = new StudentCourseEvaluationCollection(list);
            Assert.IsNotNull(scol.DefaultEvaluationDefinition);
            Assert.AreEqual(0, scol.Count);            

            //IEnumerable, automatic definition
            scol = new StudentCourseEvaluationCollection((IEnumerable<StudentCourseEvaluation>)list);
            Assert.IsNotNull(scol.DefaultEvaluationDefinition);
            Assert.AreEqual(0, scol.Count);            
        }

        [TestMethod]
        public void TestConstruction2()
        {
            var scol = new StudentCourseEvaluationCollection();
            Assert.IsNotNull(scol.DefaultEvaluationDefinition);
            Assert.AreEqual(0, scol.DefaultEvaluationDefinition.Count);

            var defs = CreateDefinition();
            scol = new StudentCourseEvaluationCollection(defs);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);

            var list = new List<Student>();
            list.Add(new Student("1234", "Anna", "Nova"));
            list.Add(new Student("4321", "Bara", "Nova"));

            //IList
            scol = new StudentCourseEvaluationCollection(list, defs);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //IEnumerable
            scol = new StudentCourseEvaluationCollection((IEnumerable<Student>)list, defs);
            Assert.AreEqual(defs, scol.DefaultEvaluationDefinition);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //IList
            scol = new StudentCourseEvaluationCollection(list);
            Assert.AreEqual(0, scol.DefaultEvaluationDefinition.Count);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //IEnumerable, automatic definition
            scol = new StudentCourseEvaluationCollection((IEnumerable<Student>)list);
            Assert.AreEqual(0, scol.DefaultEvaluationDefinition.Count);
            Assert.AreEqual(2, scol.Count);
            Assert.AreEqual("1234", scol[0].Student.PersonalNumber);

            //Test with empty initial collection
            list.Clear();
            //IList
            scol = new StudentCourseEvaluationCollection(list);
            Assert.IsNotNull(scol.DefaultEvaluationDefinition);
            Assert.AreEqual(0, scol.Count);

            //IEnumerable, automatic definition
            scol = new StudentCourseEvaluationCollection((IEnumerable<Student>)list);
            Assert.IsNotNull(scol.DefaultEvaluationDefinition);
            Assert.AreEqual(0, scol.Count);
        }

        [TestMethod]
        public void TestAddingStudents()
        {
            var defs = CreateDefinition();
            var list = new List<StudentCourseEvaluation>();
            list.Add(new StudentCourseEvaluation("1234", "Anna", "Nova", defs));
            list.Add(new StudentCourseEvaluation("4321", "Bara", "Nova", defs));

            var scol = new StudentCourseEvaluationCollection(list);
            scol.Add(new Student("5678", "Cyril", "Novy"));
            Assert.AreEqual(3, scol.Count);
            Assert.AreEqual(defs, scol[2].Evaluation.EvaluationDefinitions);
            Assert.AreEqual("5678", scol[2].Student.PersonalNumber);

            scol.Add("8901", "Dana","Nova");
            Assert.AreEqual(4, scol.Count);
            Assert.AreEqual(defs, scol[3].Evaluation.EvaluationDefinitions);
            Assert.AreEqual("8901", scol[3].Student.PersonalNumber);
            Assert.AreEqual("Dana", scol[3].Student.FirstName);
            Assert.AreEqual("Nova", scol[3].Student.Surname);

            scol.Insert(1, new Student("2468", "Cenda", "Novy"));
            Assert.AreEqual(5, scol.Count);
            Assert.AreEqual(defs, scol[1].Evaluation.EvaluationDefinitions);
            Assert.AreEqual("2468", scol[1].Student.PersonalNumber);

            scol.Insert(1, "8902", "Dana","Nova");
            Assert.AreEqual(6, scol.Count);
            Assert.AreEqual("8902", scol[1].Student.PersonalNumber);
            Assert.AreEqual("Dana", scol[1].Student.FirstName);
            Assert.AreEqual("Nova", scol[1].Student.Surname);
        }

        [TestMethod]
        public void TestRemovingStudents()
        {
            var st = new Student("1234", "Anna", "Nova");

            var scol = new StudentCourseEvaluationCollection();            
            Assert.AreEqual(-1, scol.IndexOf(st));
            Assert.AreEqual(false, scol.Contains(st));
            Assert.AreEqual(false, scol.Remove(st));

            scol.Add(st);
            Assert.AreEqual(0, scol.IndexOf(st));
            Assert.AreEqual(true, scol.Contains(st));
            Assert.AreEqual(true, scol.Remove(st));
            Assert.AreEqual(0, scol.Count);

            scol.Add("2435", "Bara", "Nova");
            scol.Add(st);
            Assert.AreEqual(1, scol.IndexOf(st));
            Assert.AreEqual(true, scol.Contains(st));
            Assert.AreEqual(true, scol.Remove(st));
            Assert.AreEqual(1, scol.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddingStudentsException1()
        {
            var scol = new StudentCourseEvaluationCollection();
            scol.Add((Student)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddingStudentsException2()
        {
            var scol = new StudentCourseEvaluationCollection();
            scol.Insert(0, (Student)null);
        }

        private EvaluationDefinitionCollection CreateDefinition()
        {
            var defs = new EvaluationDefinitionCollection();
            defs.Add(new EvaluationDefinition()
                {
                    Name = "Category",
                });

            return defs;
        }
    }
}
