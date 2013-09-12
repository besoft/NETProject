using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Zcu.StudentEvaluator.Core.Data
{
    /// <summary>
    /// Evaluation object describes one particular evaluation in one evaluation parent (Category) for one student (Student)
    /// </summary>
    public class Evaluation : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the number of points.
        /// </summary>
        /// <value>
        /// The number of points.
        /// </value>
        public decimal? Points { get; set; }

        /// <summary>
        /// Gets the reason for the points given.
        /// </summary>
        /// <value>
        /// The reason for the points give, e.g. "the solution lacks OO design".
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the evaluation parent.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {                            
                if (value != _category)
                {
                    //to prevent recursion
                    using (var rp = new RecursionPoint(this))
                    {
                        if (rp.IsRecursive())
                            return; //recursion detected, do nothing

                        if (_category != null)
                        {
                            var oldcat = _category;
                            _category = null;           //set it to null, so RemoveEvaluation will not call us again

                            oldcat.RemoveEvaluation(this);
                        }

                        if (value != null)
                        {
                            _category = value;      //set it to this, so AddEvaluation will not call us again

                            _category.AddEvaluation(this);
                        }                        
                    }

                    //notify observer that Category value has changed
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The  evaluation parent. For internal use only.
        /// </summary>
        private Category _category;

        /// <summary>
        /// Gets or sets the student to whom this evaluation belongs.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        public Student Student 
        {
            get
            {
                return _student;
            }
            set
            {
                if (value != _student)
                {
                    //to prevent recursion
                    using (var rp = new RecursionPoint(this))
                    {
                        if (rp.IsRecursive())
                            return; //recursion detected, do nothing

                        if (_student != null)
                        {
                            var oldst = _student;
                            _student = null;           //set it to null, so RemoveEvaluation will not call us again

                            oldst.RemoveEvaluation(this);
                        }

                        if (value != null)
                        {
                            _student = value;      //set it to this, so AddEvaluation will not call us again

                            _student.AddEvaluation(this);
                        }
                    }

                    //notify observer that Student value has changed
                    NotifyPropertyChanged();
                }                
            }
        }


        /// <summary>
        /// The student to whom this evaluation belongs.  For internal use only.
        /// </summary>
        private Student _student;

        /// <summary>
        /// Occurs when some property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName  (from .NET 4.5)
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Category shortcuts
        /// <summary>
        /// Gets the name of the evaluation.
        /// </summary>
        /// <value>
        /// The name of the evaluation, e.g. "Comments in code".
        /// </value>
        public string Name { get { return this.Category != null ? this.Category.Name : null; } }

        /// <summary>
        /// Gets the minimal number of points required to pass.
        /// </summary>
        /// <value>
        /// The number of points to pass.
        /// </value>
        public decimal? MinPoints { get { return this.Category != null ? this.Category.MinPoints : null; } }

        /// <summary>
        /// Gets the maximal number of points that will count.
        /// </summary>
        /// <value>
        /// The maximal number of points that counts
        /// </value>
        public decimal? MaxPoints { get { return this.Category != null ? this.Category.MaxPoints : null; } }
        #endregion
        
        /// <summary>
        /// Gets the number of points that can be counted.
        /// </summary>
        /// <value>
        ///  Number of points that counts, i.e., Points truncated by the available Max
        /// </value>
        public decimal? ValidPoints {
            get
            {
                if (this.Points == null || this.MaxPoints == null)
                    return this.Points;
                else
                    return Math.Min(this.Points.Value, this.MaxPoints.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the result of this evaluation is "Pass".
        /// </summary>
        /// <remarks>Evaluation result is "pass", if the number of received points is greater than or equal the minimum requested.
        /// Note that the result is also "Pass", if the minimum requested is not specified. </remarks>
        /// <value>
        /// <c>true</c> if the evaluation result is "passed"; otherwise, <c>false</c>.
        /// </value>
        public bool HasResultPassed
        {
            get
            {
                if (this.MinPoints == null)
                    return true;
                else if (this.Points == null)
                    return false;                
                else
                    return this.Points.Value >= this.MinPoints.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the result of this evaluation is "Fail".
        /// </summary>
        /// <remarks>Evaluation result is "Fail", if the number of received points is less than the minimum requested. 
        /// Note that the result is also "Fail", if the number of received points is not specified whilst the minimum requested is
        /// but the result is NOT "Fail", if the minimum requested is not specified. </remarks>
        /// <value>
        /// <c>true</c> if the evaluation result is "Fail"; otherwise, <c>false</c>.
        /// </value>
        public bool HasResultFailed
        {
            get
            {
                if (this.MinPoints == null)
                    return false;
                else if (this.Points == null)
                    return true;
                else
                    return this.Points.Value < this.MinPoints.Value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string strValue;
            if (!this.Points.HasValue)
                strValue = "?b";
            else
            {
                strValue = this.Reason == null ? this.Points.Value + "b" :
                    this.Points.Value + "b (" + this.Reason + ")";
            }


            return (this.Category ?? new Category()).ToString() + ": " + strValue;
        }
    }
}
