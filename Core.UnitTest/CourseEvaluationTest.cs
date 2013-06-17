using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Zcu.StudentEvaluator.Core.UnitTest
{ 
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CourseEvaluationTest
    {
        public EvaluationDefinitionCollection Definition { get; private set; }

        [TestInitialize]
        public void InitDefinition()
        {
            this.Definition = new EvaluationDefinitionCollection();
            this.Definition.Add(
                new EvaluationDefinition()
                {
                    Name = "A",
                    MinPoints = 10,
                    MaxPoints = 20,
                }
                );

            this.Definition.Add(
                new EvaluationDefinition()
                {
                    Name = "B",
                    MinPoints = 5,
                    MaxPoints = 10,
                }
                );

            this.Definition.Add(
                new EvaluationDefinition()
                {
                    Name = "C",                    
                    MaxPoints = 20,
                }
                );

            this.Definition.Add(
                new EvaluationDefinition()
                {
                    Name = "D",
                    MinPoints = 10,                    
                }
                );

            this.Definition.Add(
                new EvaluationDefinition()
                {
                    Name = "E",                    
                }
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]        
        public void TestConstructionWithNull()
        {
            var eval = new CourseEvaluation(null);
            Assert.Fail("CourseEvaluation should throw exception if the definition is null");
        }

        [TestMethod]       
        public void TestConstructionWithEmptyDefinition()
        {
            var eval = new CourseEvaluation();
            
            Assert.AreEqual(null, eval.GetTotalPoints());
            Assert.AreEqual(null, eval.GetTotalPointsReason());
        }


        [TestMethod]
        public void TestConstruction()
        {
            var eval = new CourseEvaluation(this.Definition);
            Assert.AreEqual(this.Definition, eval.EvaluationDefinitions);

            Assert.AreEqual(this.Definition.Count, eval.Evaluations.Count);
            for (int i = 0; i < this.Definition.Count; i++)
            {
                Assert.AreEqual(this.Definition[i], eval.Evaluations[i].Definition);
            }
        }

        [TestMethod]
        public void TestModifyingValues()
        {
            if (this.Definition.Count != 5) {
                Assert.Inconclusive("Test corrupted! Definition is in an unexpected state.");
                return;
            }

            var eval = new CourseEvaluation(this.Definition);

            var col = new List<Evaluation>(eval.GetAllEvaluations());
            Assert.AreEqual(5, col.Count, "GetAllEvaluations [Init]."); 
            
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(this.Definition[i], col[i].Definition);            
            }

            col = new List<Evaluation>(eval.GetValidEvaluations());
            Assert.AreEqual(0, col.Count, "GetValidEvaluations [Init].");

            col = new List<Evaluation>(eval.GetMissingEvaluations());
            Assert.AreEqual(5, col.Count, "GetMissingEvaluations [Init].");

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(this.Definition[i], col[i].Definition);
            }

            col = new List<Evaluation>(eval.GetHasPassedEvaluations());
            Assert.AreEqual(2, col.Count, "GetHasPassedEvaluations [Init].");

            col = new List<Evaluation>(eval.GetHasFailedEvaluations());
            Assert.AreEqual(3, col.Count, "GetHasFailedEvaluations [Init].");           

            Assert.AreEqual(null, eval.GetTotalPoints());
            Assert.AreEqual(null, eval.GetTotalPointsReason());
            Assert.AreEqual("?b", eval.ToString());


            //let us add some value
            eval.Evaluations[0].Value.Points = 11;  //A - 10-20
            eval.Evaluations[1].Value.Points = 11;  //B - 5-10
            eval.Evaluations[2].Value.Points = 11;  //C - <=20
            eval.Evaluations[3].Value.Points = 9;   //D - >=10
                                                    //E - 

            col = new List<Evaluation>(eval.GetValidEvaluations());
            Assert.AreEqual(4, col.Count, "GetValidEvaluations [Filled]."); //A-D
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(this.Definition[i], col[i].Definition);
            }

            col = new List<Evaluation>(eval.GetMissingEvaluations());   //E
            Assert.AreEqual(1, col.Count, "GetMissingEvaluations [Filled].");
            Assert.AreEqual(this.Definition[4], col[0].Definition);

            //check HasPassed
            col = new List<Evaluation>(eval.GetHasPassedEvaluations());   //A,B,C,E
            Assert.AreEqual(4, col.Count, "GetHasPassedEvaluations [Filled].");
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(this.Definition[i], col[i].Definition);
            }
            Assert.AreEqual(this.Definition[4], col[3].Definition);

            //check HasFailed
            col = new List<Evaluation>(eval.GetHasFailedEvaluations());   //D
            Assert.AreEqual(1, col.Count, "GetHasFailedEvaluations [Filled].");            
            Assert.AreEqual(this.Definition[3], col[0].Definition);

            decimal? dec = 11 + 10 + 11 + 9;
            Assert.AreEqual(dec, eval.GetTotalPoints());

            //add reasons
            eval.Evaluations[1].Value.Reason = "Reason1";
            eval.Evaluations[3].Value.Reason = "Reason2";

            Assert.AreEqual("A [10-20b]: 11b, B [5-10b]: 11b (Reason1), C [max 20b]: 11b, D [min 10b]: 9b (Reason2)",   
                  eval.GetTotalPointsReason());

            Assert.AreEqual("41b = A [10-20b]: 11b, B [5-10b]: 11b (Reason1), C [max 20b]: 11b, D [min 10b]: 9b (Reason2)",
                  eval.ToString());
        }

        [TestMethod]
        public void TestModyfingDefinition()
        {
            //Add some value
            var eval = new CourseEvaluation(this.Definition);
            eval.Evaluations[0].Value.Points = 11;  //A - 10-20
            eval.Evaluations[1].Value.Points = 11;  //B - 5-10
            eval.Evaluations[1].Value.Reason = "Reason1";
            eval.Evaluations[2].Value.Points = 11;  //C - <=20
            eval.Evaluations[3].Value.Points = 9;   //D - >=10
            eval.Evaluations[3].Value.Reason = "Reason2";
            eval.Evaluations[4].Value.Points = 10;  //E - nothing

            Assert.AreEqual("A [10-20b]: 11b, B [5-10b]: 11b (Reason1), C [max 20b]: 11b, D [min 10b]: 9b (Reason2), E: 10b",
                  eval.GetTotalPointsReason());

            eval.Evaluations[3].Definition.Name = "F";
            Assert.AreEqual("A [10-20b]: 11b, B [5-10b]: 11b (Reason1), C [max 20b]: 11b, F [min 10b]: 9b (Reason2), E: 10b",
                  eval.GetTotalPointsReason());

            this.Definition.Move(4, 3);
            Assert.AreEqual("A [10-20b]: 11b, B [5-10b]: 11b (Reason1), C [max 20b]: 11b, E: 10b, F [min 10b]: 9b (Reason2)",
                  eval.GetTotalPointsReason());

            this.Definition.RemoveAt(1);
            Assert.AreEqual("A [10-20b]: 11b, C [max 20b]: 11b, E: 10b, F [min 10b]: 9b (Reason2)",
                  eval.GetTotalPointsReason());

            this.Definition.Add(new EvaluationDefinition()
                {
                    Name = "G",
                    MaxPoints = 20,
                }
                );
            Assert.AreEqual(5, eval.Evaluations.Count);
            eval.Evaluations[4].Value.Points = 18;  //G - <= 20
            Assert.AreEqual("A [10-20b]: 11b, C [max 20b]: 11b, E: 10b, F [min 10b]: 9b (Reason2), G [max 20b]: 18b",
                  eval.GetTotalPointsReason());

            this.Definition.Insert(1, new EvaluationDefinition()
            {
                Name = "B",
            }
                );
            Assert.AreEqual(6, eval.Evaluations.Count);
            eval.Evaluations[1].Value.Points = 15;  //B - nothing
            Assert.AreEqual("A [10-20b]: 11b, B: 15b, C [max 20b]: 11b, E: 10b, F [min 10b]: 9b (Reason2), G [max 20b]: 18b",
                  eval.GetTotalPointsReason());

            this.Definition[0] = new EvaluationDefinition()
            {
                Name = "AR",
            };

            Assert.AreEqual("AR: 11b, B: 15b, C [max 20b]: 11b, E: 10b, F [min 10b]: 9b (Reason2), G [max 20b]: 18b",
                  eval.GetTotalPointsReason());

            decimal? sum = 11 + 15 + 11 + 10 + 9 + 18;
            Assert.AreEqual(sum, eval.GetTotalPoints());

            this.Definition.Clear();
            Assert.AreEqual(0, eval.Evaluations.Count);
            Assert.AreEqual(null, eval.GetTotalPointsReason());            

            this.Definition.Add(new EvaluationDefinition()
            {
                Name = "A",                
            }
                );
            Assert.AreEqual(1, eval.Evaluations.Count);
            eval.Evaluations[0].Value.Points = 10;  //A - nothing
            Assert.AreEqual("A: 10b", eval.GetTotalPointsReason());
        }
    }
}
