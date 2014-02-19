
using System.Runtime.CompilerServices;
namespace Zcu.StudentEvaluator.Model
{
    /// <summary>
    /// This namespace contains classes and interfaces for defining the data model.
    /// </summary>       
    /// <remarks>Model defines in-memory stored data entities that are used to exchange data to/from repositories.</remarks>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
        //Trick to document a namespace
    }

    /// <summary>
    /// Represents the entity (of the model)
    /// </summary>
	public interface IEntity
	{
		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		int Id { get; set; }
	}
}
