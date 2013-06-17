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
    public class EvaluationDefinitionCollectionTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            var defs = new EvaluationDefinitionCollection();
            Assert.AreEqual(0, defs.Count);

            var list = new List<EvaluationDefinition>();
            defs = new EvaluationDefinitionCollection(list);
            Assert.AreEqual(0, defs.Count);

            defs = new EvaluationDefinitionCollection((IEnumerable<EvaluationDefinition>)list);
            Assert.AreEqual(0, defs.Count);

            list.Add(new EvaluationDefinition());
            defs = new EvaluationDefinitionCollection(list);
            Assert.AreEqual(1, defs.Count);
            Assert.AreEqual(list[0], defs[0]);

            defs = new EvaluationDefinitionCollection((IEnumerable<EvaluationDefinition>)list);
            Assert.AreEqual(1, defs.Count);
            Assert.AreEqual(list[0], defs[0]);
        }
    }
}
