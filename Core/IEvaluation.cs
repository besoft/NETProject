using System;
namespace Zcu.StudentEvaluator.Core
{
    /// <summary>
    /// Provides the information about the evaluation.
    /// </summary>
    public interface IEvaluation
    {
        /// <summary>
        /// Gets the name of the evaluation.
        /// </summary>
        /// <value>
        /// The name of the evaluation, e.g. "Comments in code".
        /// </value>   
        string Name { get; }

        /// <summary>
        /// Gets the minimal number of points required to pass.
        /// </summary>
        /// <value>
        /// The number of points to pass.
        /// </value>
        decimal? MinPoints { get; }

        /// <summary>
        /// Gets the maximal number of points that will count (will be valid).
        /// </summary>
        /// <value>
        /// The maximal number of points that counts
        /// </value>
        decimal? MaxPoints { get; }

        /// <summary>
        /// Gets the number of points.
        /// </summary>
        /// <value>
        /// The number of points.
        /// </value>
        decimal? Points { get; }

        /// <summary>
        /// Gets the number of points that can be counted.
        /// </summary>
        /// <value>
        ///  Number of points that counts, i.e., Points truncated by the available Max
        /// </value>
        decimal? ValidPoints { get; }

        /// <summary>
        /// Gets the reason for the points given.
        /// </summary>
        /// <value>
        /// The reason for the points give, e.g. "the solution lacks OO design".
        /// </value>
        string Reason { get; }

        /// <summary>
        /// Gets a value indicating whether the result of this evaluation is "Pass".
        /// </summary>
        /// <remarks>Evaluation result is "pass", if the number of received points is greater than or equal the minimum requested.
        /// Note that the result is also "Pass", if the minimum requested is not specified. </remarks>
        /// <value>
        /// <c>true</c> if the evaluation result is "passed"; otherwise, <c>false</c>.
        /// </value>
        bool HasResultPassed { get; }  
        
        /// <summary>
        /// Gets a value indicating whether the result of this evaluation is "Fail".
        /// </summary>
        /// <remarks>Evaluation result is "Fail", if the number of received points is less than the minimum requested. 
        /// Note that the result is also "Fail", if the number of received points is not specified whilst the minimum requested is
        /// but the result is NOT "Fail", if the minimum requested is not specified. </remarks>
        /// <value>
        /// <c>true</c> if the evaluation result is "Fail"; otherwise, <c>false</c>.
        /// </value>
        bool HasResultFailed { get; }      
    }
}
