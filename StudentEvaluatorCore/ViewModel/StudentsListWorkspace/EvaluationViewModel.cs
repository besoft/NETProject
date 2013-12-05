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

		public EvaluationViewModel(Evaluation model, IRepository<Evaluation> repository)
			: base(model, repository)
		{
			
		}

		public decimal? ValidPoints { get; set; }

		public string ValidPointsReason { get; set; }

		public bool HasPassed { get; set; }
	}
}
