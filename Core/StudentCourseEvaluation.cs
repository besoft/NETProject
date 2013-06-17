using System;
using Zcu.StudentEvaluator.Core.Data.Schema;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// This class serves as a container for the student and its evaluation
    /// </summary>
    public class StudentCourseEvaluation
    {
        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        public Student Student { get; private set; }

        /// <summary>
        /// Gets or sets the evaluation.
        /// </summary>
        /// <value>
        /// The evaluation.
        /// </value>
        public CourseEvaluation Evaluation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCourseEvaluation" /> class.
        /// </summary>
        /// <param name="student">The student.</param>
        /// <param name="evaluation">The evaluation.</param>
        /// <exception cref="System.ArgumentNullException">if student is null.</exception>
        public StudentCourseEvaluation(Student student, CourseEvaluation evaluation = null)
        {
            if (student == null)
                throw new ArgumentNullException("student");

            this.Student = student;
            this.Evaluation = evaluation ?? new CourseEvaluation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCourseEvaluation" /> class.
        /// </summary>
        /// <param name="student">The student.</param>
        /// <param name="evalDefinition">The eval definition.</param>
        /// <exception cref="System.ArgumentNullException">if student is null.</exception>
        public StudentCourseEvaluation(Student student, EvaluationDefinitionCollection evalDefinition)
        {
            if (student == null)
                throw new ArgumentNullException("student");
            if (evalDefinition == null)
                throw new ArgumentNullException("evalDefinition");

            this.Student = student;
            this.Evaluation = new CourseEvaluation(evalDefinition);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCourseEvaluation" /> class.
        /// </summary>
        /// <param name="studentPersonalNumber">The student personal number.</param>
        /// <param name="studentFirstName">First name of the student.</param>
        /// <param name="studentSurname">The student surname.</param>
        /// <param name="evaluation">The evaluation.</param>
        /// <exception cref="System.ArgumentNullException">if studentPersonalNumber, studentFirstName or studentSurname is null.</exception>
        public StudentCourseEvaluation(string studentPersonalNumber, 
            string studentFirstName, string studentSurname, CourseEvaluation evaluation = null)
        {
            if (studentPersonalNumber == null)
                throw new ArgumentNullException("studentPersonalNumber");
            if (studentFirstName == null)
                throw new ArgumentNullException("studentFirstName");
            if (studentSurname == null)
                throw new ArgumentNullException("studentSurname");

            this.Student = new Student(studentPersonalNumber, studentFirstName, studentSurname);
            this.Evaluation = evaluation ?? new CourseEvaluation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCourseEvaluation" /> class.
        /// </summary>
        /// <param name="studentPersonalNumber">The student personal number.</param>
        /// <param name="studentFirstName">First name of the student.</param>
        /// <param name="studentSurname">The student surname.</param>
        /// <param name="evalDefinition">The eval definition.</param>
        /// <exception cref="System.ArgumentNullException">if studentPersonalNumber, studentFirstName, studentSurname or evalDefinition is null.</exception>
        public StudentCourseEvaluation(string studentPersonalNumber,
            string studentFirstName, string studentSurname, EvaluationDefinitionCollection evalDefinition)
        {
            if (studentPersonalNumber == null)
                throw new ArgumentNullException("studentPersonalNumber");
            if (studentFirstName == null)
                throw new ArgumentNullException("studentFirstName");
            if (studentSurname == null)
                throw new ArgumentNullException("studentSurname");
            if (evalDefinition == null)
                throw new ArgumentNullException("evalDefinition");

            this.Student = new Student(studentPersonalNumber, studentFirstName, studentSurname);
            this.Evaluation = new CourseEvaluation(evalDefinition);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {                       
            return String.Format("{0}\t{1}", this.Student, this.Evaluation);
        }
    }
}
