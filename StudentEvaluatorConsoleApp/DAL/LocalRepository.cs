using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Generic repository of entities
	/// </summary>
	public class LocalRepository<TEntity> : IRepository<TEntity>
		where TEntity: IEntity	
	{		
		/// <summary>
		/// The data context to be worked with
		/// </summary>
		protected LocalStudentEvaluationContext Context { get; private set; }

		/// <summary>
		/// The collection containing the data for TEntity
		/// </summary>
		protected ICollection<TEntity> Items {get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="StudentRepository"/> class.
		/// </summary>
		/// <param name="context">The context of this repository.</param>
		public LocalRepository(LocalStudentEvaluationContext context)
		{
			this.Context = context;
			
			//discover Items in the context
			this.Items = DiscoverCollection(context);
		}

		/// <summary>
		/// Discovers the collection.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>Instance of the collection of TEntity existing in the given context</returns>		
		private ICollection<TEntity> DiscoverCollection(LocalStudentEvaluationContext context)
		{
			//Reflection :-)
			var getter = (from x in context.GetType().GetProperties()
							 where x.PropertyType.IsGenericType &&
								   x.PropertyType.GenericTypeArguments.Contains(typeof(TEntity))
							 select x.GetMethod).SingleOrDefault();

			if (getter == null)
				throw new ArgumentException("Public property returning a collection implementing ICollection<" +
					typeof(TEntity).Name + "> could not been found in " + context.GetType().Name + "class.", "context");
			
			return (ICollection<TEntity>)getter.Invoke(context, null);
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

			return orderBy == null ? query : orderBy(query);
		}

		/// <summary>
		/// Gets the item identified by its Id.
		/// </summary>
		/// <param name="Id">The unique identifier.</param>
		/// <returns>Item with the Id</returns>
		public TEntity Get(int Id)
		{
			return this.Items.Where(x => x.Id == Id).Single();
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
			this.Items.Remove(this.Items.Where(x => x.Id == Id).Single());
		}

		/// <summary>
		/// Deletes the item from the repository.
		/// </summary>
		/// <param name="item">The item to be deleted.</param>
		public void Delete(TEntity item)
		{
			this.Items.Remove(item);
		}

		/// <summary>
		/// Updates the item data in the repository.
		/// </summary>
		/// <param name="item">The new item data.</param>
		public void Update(TEntity item)
		{
			//nothing to do
		}
	}	
}
