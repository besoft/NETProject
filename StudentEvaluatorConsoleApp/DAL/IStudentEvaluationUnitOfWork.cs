﻿using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Represents ModelRepository, i.e., atomic part through which the rest of application access the model data.
	/// </summary>
	public interface IStudentEvaluationUnitOfWork
	{
		/// <summary>
		/// Gets the repository of students.
		/// </summary>
		/// <value>
		/// The students repository.
		/// </value>
		IRepository<Student> Students { get; }

		/// <summary>
		/// Gets the repository of categories.
		/// </summary>
		/// <value>
		/// The categories repository.
		/// </value>
		IRepository<Category> Categories { get; }

		/// <summary>
		/// Gets the repository of evaluations.
		/// </summary>
		/// <value>
		/// The evaluations repository.
		/// </value>
		IRepository<Evaluation> Evaluations { get; }
		
		/// <summary>
		/// Saves all changes done in unitOfWork.
		/// </summary>
		/// <remarks>
		/// Saves all changes into persistent stream.
		/// </remarks>
		void Save();
	}
}
