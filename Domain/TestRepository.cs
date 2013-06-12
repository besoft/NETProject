using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Domain;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;

namespace Zcu.StudentEvaluator.Domain.Test
{
    public class TestRepository : IRepository
    {
        private StudentCourseEvaluation[] _students = null;

        /// <summary>
        /// Gets the list of students.
        /// </summary>
        /// <value>
        /// The list of students to be contained.
        /// </value>
        public StudentCourseEvaluation[] StudentsCourseEvaluation
        {
            get
            {
                return _students;
            }
            set
            {
                _students = value;
            }
        }

        public TestRepository()
        {
            const int Ecnt = 4;
            var schema = new EvaluationDefinition[Ecnt]{
                new EvaluationDefinition() {Name="Design"},
                new EvaluationDefinition() {Name="Implementation"},
                new EvaluationDefinition() {Name="CodeCulture"},
                new EvaluationDefinition() {Name="Documentation"},
            };
            /*
            var schema = new List<EvaluationDefinition>(Ecnt);
            schema.Add(new EvaluationDefinition() { Name = "Design" });
            schema.Add(new EvaluationDefinition() { Name = "Implementation" });
            schema.Add(new EvaluationDefinition() { Name = "CodeCulture" });
            schema.Add(new EvaluationDefinition() { Name = "Documentation" });
             * */


            this.StudentsCourseEvaluation = new StudentCourseEvaluation[3]{
                new StudentCourseEvaluation() {
                    Student = new Student() {PersonalNumber = "A12B0001P", FirstName="Anna", Surname="Aysle", },
                    Evaluation = new CourseEvaluation(schema),
                },

                new StudentCourseEvaluation() {
                    Student = new Student() {PersonalNumber = "A12B0002P", FirstName="Barbora", Surname="Bílá", },
                    Evaluation = new CourseEvaluation(schema),
                },

                new StudentCourseEvaluation() {
                    Student = new Student() {PersonalNumber = "A12B0003P", FirstName="Cyril", Surname="Cejn", },
                    Evaluation = new CourseEvaluation(schema),
                },
            };
        }
    }
}
