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
        private Student[] _students = null;

        /// <summary>
        /// Gets the list of students.
        /// </summary>
        /// <value>
        /// The list of students to be contained.
        /// </value>
        public Student[] Students
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
            var schema = new EvaluationItemSchema[Ecnt]{
                new EvaluationItemSchema() {Name="Design"},
                new EvaluationItemSchema() {Name="Implementation"},
                new EvaluationItemSchema() {Name="CodeCulture"},
                new EvaluationItemSchema() {Name="Documentation"},
            };

            this.Students = new Student[3]{
                new Student() {PersonalNumber = "A12B0001P", FirstName="Anna", Surname="Aysle", 
                    EvaluationSchema = schema, Evaluation = new EvaluationItem[Ecnt]},

                new Student() {PersonalNumber = "A12B0002P", FirstName="Barbora", Surname="Bílá", 
                    EvaluationSchema = schema, Evaluation = new EvaluationItem[Ecnt]},

                new Student() {PersonalNumber = "A12B0003P", FirstName="Cyril", Surname="Cejn", 
                    EvaluationSchema = schema, Evaluation = new EvaluationItem[Ecnt]},
            };
        }
    }
}
