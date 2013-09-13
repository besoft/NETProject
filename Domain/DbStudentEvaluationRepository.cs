using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Zcu.StudentEvaluator.Core.Collection;
using System.Collections.ObjectModel;
using Zcu.StudentEvaluator.Core.Data;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Data.Entity;

namespace Zcu.StudentEvaluator.Domain
{
	/// <summary>
	/// Represents a repository of students and their evaluations that can be stored / load from XML.
	/// </summary>
	public class DbStudentEvaluationRepository : StudentEvaluationRepository, IPersistantRepository
	{		
		protected class DbRepositoryRoot : DbContext
		{
			public DbRepositoryRoot()
				//: base("Name=StudentEvaluationDatabase")
			{

			}

			public DbSet<Category> Categories { get; set; }		
			public DbSet<Student> Students { get; set; }			
			public DbSet<Evaluation> Evaluations { get; set; }			
		}

		/// <summary>
		/// Resets the repository to the initial empty state.
		/// </summary>		
		public void InitNew()
		{
			this.Categories.Clear();	//empty state = empty repository
			this.Students.Clear();

			OnRepositoryInitialized();
		}
		
		
		/// <summary>
		/// Loads the previously stored repository state.
		/// </summary>		
		public void Load()
		{
			using (var ctx = new DbRepositoryRoot())
			{
				this.Categories.Clear();	//empty state = empty repository
				this.Students.Clear();

				foreach (var item in ctx.Categories)
				{
					if (!this.Categories.Contains(item))
						this.Categories.Add(item);
				}

				foreach (var item in ctx.Students)
				{
					if (!this.Students.Contains(item))
						this.Students.Add(item);
				}

				foreach (var item in ctx.Evaluations)
				{
					if (!this.Evaluations.Contains(item))
						this.Evaluations.Add(item);					
				}
			}
			

			OnRepositoryLoaded();
		}

		
		/// <summary>
		/// Saves the current state of the repository.
		/// </summary>		
		public void Save()
		{		
			using (var ctx = new DbRepositoryRoot())
			{
				ctx.Database.Delete();
				ctx.Database.Initialize(true);	//ensures that database is recreated				

				foreach (var item in this.Categories)
				{
					ctx.Categories.Add(item);
				}

				foreach (var item in this.Students)
				{
					ctx.Students.Add(item);
				}

				foreach (var item in this.Evaluations)
				{
					ctx.Evaluations.Add(item);
				}

				ctx.SaveChanges();
			}

			OnRepositorySaved();
		}

		
		/// <summary>
		/// Called after the repository has been initialized.
		/// </summary>
		protected virtual void OnRepositoryInitialized()
		{
		}

		/// <summary>
		/// Called when repository has been loaded.
		/// </summary>		
		protected virtual void OnRepositoryLoaded()
		{
		}

		/// <summary>
		/// Called when repository has been successfully saved.
		/// </summary>		
		protected virtual void OnRepositorySaved()
		{
		}
	}
}
