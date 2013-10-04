using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Generic repository of entities
	/// </summary>
	public class DbRepository<TEntity> : IRepository<TEntity>
		where TEntity: class, IEntity, new()
	{		
		/// <summary>
		/// The data context to be worked with
		/// </summary>
		protected DbStudentEvaluationContext Context { get; private set; }

		/// <summary>
		/// The collection containing the data for TEntity
		/// </summary>
		protected DbSet<TEntity> Items {get; private set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DbRepository"/> class.
		/// </summary>
		/// <param name="context">The context of this repository.</param>
		public DbRepository(DbStudentEvaluationContext context)
		{
			this.Context = context;
			
			//discover Items in the context
			this.Items = this.Context.Set<TEntity>();
		}
		

		/// <summary>
		/// Gets the collection of all items.
		/// </summary>
		/// <param name="filter">The filter specification - see remarks.</param>
		/// <param name="orderBy">The order by specification - see remarks.</param>
		/// <param name="includeProperties">The include properties - see remarks.</param>
		/// <remarks>See IRepository</remarks>
		/// <returns>
		/// A collection of items in the repository.
		/// </returns>
		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
			string[] includeProperties = null)
		{
			IQueryable<TEntity> query = this.Items.AsQueryable();
			if (filter != null)
			{
				query = query.Where(filter);			
			}

			if (includeProperties != null)
			{
				foreach (var inc in includeProperties)
				{
					query = query.Include(inc);
				}
			}


			return orderBy == null ? query : orderBy(query);
		}

		/// <summary>
		/// Gets the item identified by its Id.
		/// </summary>
		/// <param name="Id">The unique identifier.</param>
		/// <returns>Item with the Id</returns>
		public TEntity Get(int Id)
		{
			return this.Items.Find(Id);
		}

		/// <summary>
		/// Inserts a new item into the repository.
		/// </summary>
		/// <param name="item">The item to be inserted.</param>
		public void Insert(TEntity item)
		{			
			this.Items.Add(item);
		}

		/// <summary>
		/// Deletes the item from the repository.
		/// </summary>
		/// <param name="Id">The unique identifier of the item.</param>
		public void Delete(int Id)
		{
			Delete(new TEntity() { Id = Id});	//This is faster than getting the data from Db just to remove it		
		}

		/// <summary>
		/// Deletes the item from the repository.
		/// </summary>
		/// <param name="item">The item to be deleted.</param>
		public void Delete(TEntity item)
		{
			if (this.Context.Entry(item).State == EntityState.Detached)
			{
				this.Items.Attach(item);
			}

			this.Context.Entry(item).State = EntityState.Deleted;	//mark item to be deleted
		}

		/// <summary>
		/// Updates the item data in the repository.
		/// </summary>
		/// <param name="item">The new item data.</param>
		public void Update(TEntity item)
		{
			if (this.Context.Entry(item).State == EntityState.Detached)
			{
				this.Items.Attach(item);
			}

			this.Context.Entry(item).State = EntityState.Modified;	//mark item to be modified
		}

		/// <summary>
		/// Called to reset the changes to its original state.
		/// </summary>
		/// <param name="item">The item whose changes are to discard.</param>
		public void Reset(TEntity item)
		{
			if (this.Context.Entry(item).State != EntityState.Detached)
			{
				this.Context.Entry(item).State = EntityState.Unchanged;	//mark item to be modified	
			}			
		}
	}	
}
