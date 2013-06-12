using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zcu.StudentEvaluator.Core.Data
{
    public interface ICourseEvaluation
    {
        /// <summary>
        /// Gets all evaluations available.
        /// </summary>
        /// <returns>Collection of evaluations.</returns>
        IEnumerable<Evaluation> GetAllEvaluations();

        /// <summary>
        /// Gets only the valid evaluations, i.e., evaluations with specified number of points.
        /// </summary>
        /// <returns>Collection of evaluations satisfying the required criterion.</returns>
        IEnumerable<Evaluation> GetValidEvaluations();

        /// <summary>
        /// Gets the missing evaluations, i.e., evaluations with not specified number of points.
        /// </summary>
        /// <returns>Collection of evaluations  satisfying the required criterion.</returns>
        IEnumerable<Evaluation> GetMissingEvaluations();

        /// <summary>
        /// Gets the valid evaluations that with the status "passed".
        /// </summary>
        /// <remarks>A valid evaluation has the status "passed", if the number of points received is greater than or equal the minimum required. </remarks>
        /// <returns>Collection of evaluations  satisfying the required criterion.</returns>
        IEnumerable<Evaluation> GetHasPassedEvaluations();

        /// <summary>
        /// Gets the valid evaluations that with the status "failed".
        /// </summary>
        /// <remarks>A valid evaluation has the status "failed", if the number of points received is less than the minimum required. </remarks>
        /// <returns>Collection of evaluations  satisfying the required criterion.</returns>
        IEnumerable<Evaluation> GetHasFailedEvaluations();

        /// <summary>
        /// Gets the total counted points.
        /// </summary>
        /// <returns>The overall number of the points (which were counted).</returns>
        decimal? GetTotalPoints();

        /// <summary>
        /// Gets the reason for the total points given
        /// </summary>
        /// <returns>The explanation of the evaluation given.</returns>
        string GetTotalPointsReason();
    }
}
