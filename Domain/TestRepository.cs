using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Domain;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;
using System.Collections.ObjectModel;

namespace Zcu.StudentEvaluator.Domain.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class TestRepository : IStudentsRepository
    {        
        /// <summary>
        /// Gets the list of students.
        /// </summary>
        /// <value>
        /// The list of students to be contained.
        /// </value>
        public StudentCourseEvaluationCollection StudentsCourseEvaluation {get; private set; }
        
        public TestRepository()
        {
            var schema = new EvaluationDefinitionCollection();
            schema.Add(new EvaluationDefinition() { Name = "Design", MinPoints = 2m });
            schema.Add(new EvaluationDefinition() { Name = "Implementation", MinPoints = 5m, MaxPoints=10, });
            schema.Add(new EvaluationDefinition() { Name = "CodeCulture" });
            schema.Add(new EvaluationDefinition() { Name = "Documentation", MaxPoints = 2 });

            this.StudentsCourseEvaluation = new StudentCourseEvaluationCollection(schema);
            this.StudentsCourseEvaluation.Add("A12B0001P", "Anna", "Aysle");
            this.StudentsCourseEvaluation.Add("A12B0002P", "Barbora", "Bílá");
            this.StudentsCourseEvaluation.Add("A12B0003P", "Cyril", "Cejn");
        }
    }
}
