﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Zcu.StudentEvaluator.Model
{
	/// <summary>
	/// Implements custom validations for Model.
	/// </summary>
	public class CustomValidator
	{
		/// <summary>
		/// Validates the evaluation points.
		/// </summary>
		/// <param name="evaluation">The evaluation.</param>
		/// <param name="validationContext">The validation context.</param>
		/// <param name="validationResult">The validation result.</param>
		/// <returns>true, if the value of "Points" of the give evaluation is valid; otherwise false and error message is returned in validationResult</returns>
		public static ValidationResult ValidateEvaluationPoints(decimal? points, ValidationContext validationContext)
		{
			Evaluation evaluation = validationContext.ObjectInstance as Evaluation;

			if (evaluation.Category.MaxPoints != null && points != null &&
				points > evaluation.Category.MaxPoints)
			{
				return new ValidationResult("The number of points may not exceed the maximum (" +
					evaluation.Category.MaxPoints + ") specified for category '" +
					evaluation.Category.Name + "'.");				
			}
			
			return ValidationResult.Success;
		}

		/// <summary>
		/// Validates the maximal number of points.
		/// </summary>
		/// <param name="evaluation">The evaluation.</param>
		/// <param name="validationContext">The validation context.</param>
		/// <param name="validationResult">The validation result.</param>
		/// <returns>true, if the value of "MaxPoints" of the given category is valid; otherwise false and error message is returned in validationResult</returns>
		public static ValidationResult ValidateCategoryMaxPoints(decimal? maxPoints, ValidationContext validationContext)
		{			
			if (maxPoints != null)
			{
				Category category = validationContext.ObjectInstance as Category;
				if (category.Evaluations.Count != 0)
				{
					var evaluation = (category.Evaluations.Where(x => x.Points != null && x.Points > category.MaxPoints)
											.OrderByDescending(x => x.Points)).FirstOrDefault();
					if (evaluation != null)
					{
						return new ValidationResult("The maximal number of points cannot be set to " +
							category.MaxPoints + " because at least one evaluation specifies the number of points that exceed this value." +
							"The minimal allowed value is " + evaluation.Points + ".");
					}
				}
			}

			return ValidationResult.Success;
		}
	}
}
