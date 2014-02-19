using System;
using Zcu.StudentEvaluator.DAL;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.ViewModel
{
    /// <summary>
    /// TODO: create THIS
    /// </summary>
	public class EvaluationViewModel : ViewModel<Evaluation>
	{				
		/// <summary>
		/// Gets the model (of evaluation) associated with this ViewModel.
		/// </summary>		
		protected Evaluation Evaluation { get; private set; }

		/// <summary>
		/// Gets the  repository of the model (evaluation).
		/// </summary>		
		protected IStudentEvaluationUnitOfWork UnitOfWork { get; private set; }
		

		/// <summary>
		/// Initializes a new instance of the <see cref="EvaluationViewModel"/> class.
		/// </summary>
		/// <param name="evaluation">The evaluation.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <exception cref="System.ArgumentNullException">
		/// evaluation
		/// or
		/// unitOfWork
		/// </exception>
		public EvaluationViewModel(Evaluation evaluation, IStudentEvaluationUnitOfWork unitOfWork)
		{
			if (evaluation == null)
				throw new ArgumentNullException("evaluation");

			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");

			this.Evaluation = evaluation;
			this.UnitOfWork = unitOfWork;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="repository">The repository.</param>
		public EvaluationViewModel(Evaluation model, IRepository<Evaluation> repository)
			: base(model, repository)
		{
			
		}

        /// <summary>
        /// Gets or sets the valid points.
        /// </summary>
        /// <value>
        /// The valid points.
        /// </value>
		public decimal? ValidPoints { get; set; }

        /// <summary>
        /// Gets or sets the valid points reason.
        /// </summary>
        /// <value>
        /// The valid points reason.
        /// </value>
		public string ValidPointsReason { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [has passed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has passed]; otherwise, <c>false</c>.
        /// </value>
		public bool HasPassed { get; set; }
	}
}
