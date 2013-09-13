using System.Diagnostics.Contracts;

namespace Zcu.StudentEvaluator.Domain
{
	/// <summary>
	/// This interface is designed for a repository that allows storing/loading its state, e.g., into/from XML files, database, ...
	/// </summary>
	[ContractClass(typeof(IPersistantRepositoryContract))]
	public interface IPersistantRepository
	{
		/// <summary>
		/// Resets the repository to the initial empty state.
		/// </summary>
		void InitNew();

		/// <summary>
		/// Loads the previously stored repository state.
		/// </summary>
		void Load();

		/// <summary>
		/// Saves the current state of the repository.
		/// </summary>
		void Save();
	}

	[ContractClassFor(typeof(IPersistantRepository))]
	internal abstract class IPersistantRepositoryContract : IPersistantRepository
	{
		public void InitNew()
		{
			throw new System.NotImplementedException();
		}

		public void Load()
		{
			throw new System.NotImplementedException();
		}

		public void Save()
		{
			throw new System.NotImplementedException();
		}
	}
}
