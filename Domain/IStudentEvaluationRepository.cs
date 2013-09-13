using System.Diagnostics.Contracts;

namespace Zcu.StudentEvaluator.Domain
{
	/// <summary>
	/// This interface defines routines for operating with the repository of students and their evaluations.
	/// </summary>
	[ContractClass(typeof(IStudentEvaluationRepositoryContract))]	
	public interface IStudentEvaluationRepository
	{
		//TODO: doplnit
	}

	[ContractClassFor(typeof(IStudentEvaluationRepository))]
	internal abstract class IStudentEvaluationRepositoryContract : IStudentEvaluationRepository
	{
	}
}