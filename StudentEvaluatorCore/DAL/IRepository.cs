using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zcu.StudentEvaluator.Model;

/**
 * Lecture Notes: vysvetlit rozdil mezi 
 * 1) Func<Student, bool> filter, Func<IEnumerable<Student>, IOrderedEnumerable<Student>> orderBy
 * 2) Expression<Func<Student, bool>> filter, Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy
 */ 

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Generic repository
	/// </summary>
	public interface IRepository<TEntity> where TEntity : class, IEntity, new()
	{
		/// <summary>
		/// Gets the collection of all items.
		/// </summary>
		/// <param name="filter">The filter specification - see remarks.</param>
		/// <param name="orderBy">The order by specification - see remarks.</param>
		/// <param name="includeProperties">The include properties - see remarks.</param>
		/// <returns>
		/// A collection of items in the repository.
		/// </returns>
		/// <remarks>
		/// Expression&lt;Func&lt;TEntity, bool&gt;&gt; filter means the caller will provide a lambda expression based on the TEntity
		/// type, and this expression will return a Boolean value. For example, if the repository is instantiated for the Student entity type,
		/// the code in the calling method might specify student =&gt; student.LastName == "Smith" for the filter parameter.
		/// N.B. implementations should use AsQuerable() to filter in-memory only collections.
		///
		/// Func&lt;IQueryable&lt;TEntity&gt;, IOrderedQueryable&lt;TEntity&gt;&gt; orderBy also means the caller will provide a lambda
		/// expression for ordering the collection. But in this case, the input to the expression is an IQueryable object for the TEntity type.
		/// The expression will return an ordered version of that IQueryable object. For example, if the repository is instantiated for the
		/// Student entity type, the code in the calling method might specify q =&gt; q.OrderBy(s =&gt; s.LastName).ThenBy(s=&gt; s.FirstName)
		/// for the orderBy parameter.
		/// 
		/// includeProperties is used when Entity Framework is used to specify which related objects should be retrieved from the database. 
		/// For example, if the repository is instantiated for the Student entity type, the code in the calling method might specify 
		/// new string[]{"Class"} to get valid Class object to which the student belongs.
		/// </remarks>
		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string[] includeProperties = null);

		/// <summary>
		/// Gets the item identified by its Id.
		/// </summary>
		/// <param name="Id">The unique identifier.</param>
		/// <returns>Item with the Id</returns>
		TEntity Get(int Id);

		/// <summary>
		/// Inserts a new item into the repository.
		/// </summary>
		/// <param name="item">The item to be inserted.</param>
		void Insert(TEntity item);

		/// <summary>
		/// Deletes the item from the repository.
		/// </summary>
		/// <param name="Id">The unique identifier of the item.</param>
		void Delete(int Id);

		/// <summary>
		/// Deletes the item from the repository.
		/// </summary>
		/// <param name="item">The item to be deleted.</param>
		void Delete(TEntity item);

		/// <summary>
		/// Updates the item data in the repository.
		/// </summary>
		/// <param name="item">The new item data.</param>
		void Update(TEntity item);

		/// <summary>
		/// Commits the changes that have been done to this repository since the last call of this method.
		/// </summary>		
		/// <remarks>Implementations typically stores the local changes (in-memory) into a persistent stream, e.g., file or database,
		/// which may throw different exceptions regarding the concrete implementation. 
		/// For example, Entity Framework may throw DbEntityValidationException (if the model is not valid) or
		///DbUpdateConcurrencyException (two users has changed the same item). </remarks>
		void Save();
	}
}
