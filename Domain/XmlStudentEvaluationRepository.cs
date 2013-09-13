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

namespace Zcu.StudentEvaluator.Domain
{
	/// <summary>
	/// Represents a repository of students and their evaluations that can be stored / load from XML.
	/// </summary>
	public class XmlStudentEvaluationRepository : StudentEvaluationRepository, IPersistantRepository
	{
		[DataContract]
		protected class RepositoryRoot
		{
			[DataMember]
			public List<Category> Categories { get; set; }

			[DataMember]
			public List<Student> Students { get; set; }

			[DataMember]
			public List<Evaluation> Evaluations { get; set; }			
		}

		protected const string DefaultXmlPathName = @"StudentEvaluationRepository.xml";

		/// <summary>
		/// Gets or sets the path name of the XML to be created.
		/// </summary>
		/// <value>
		/// The path name of the XML file where the repository is stored.
		/// </value>
		public string XmlPathName { get; set; }

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
			string pathname = this.XmlPathName ?? XmlStudentEvaluationRepository.DefaultXmlPathName;

			//RELEASE NOTE: the easiest and original way would be to use XmlSerializer, however, that class does not
			//support neither cyclic references (Student#1 -> Evaluation#2 -> Student#1) nor cross-references 
			//(Student#1 -> Evaluation#2, Course#3 -> Evaluation#2). For cross-references, two individual objects
			//are created instead of one shared, for cyclic references the serialization fails with an exception

			//Hence, WCF serialization will be used instead (requires System.Runtime reference)
			DataContractSerializer serializer = CreateSerializer();
			using (var fs = new FileStream(pathname, FileMode.Open))
			{
				using (var reader = XmlDictionaryReader.CreateTextReader(fs, XmlDictionaryReaderQuotas.Max))
				{					
					RepositoryRoot root = (RepositoryRoot)serializer.ReadObject(reader);

					this.Categories.Clear();	//empty state = empty repository
					this.Students.Clear();

					foreach (var item in root.Categories)
					{
						if (!this.Categories.Contains(item))
							this.Categories.Add(item);
					}

					foreach (var item in root.Students)
					{
						if (!this.Students.Contains(item))
							this.Students.Add(item);
					}

					//all evaluations should be restored now
		
					reader.Close();
				}
			}

			OnRepositoryLoaded(pathname);
		}

		/// <summary>
		/// Saves the current state of the repository.
		/// </summary>		
		public void Save()
		{
			string pathname = this.XmlPathName ?? XmlStudentEvaluationRepository.DefaultXmlPathName;

			//use WCF serializer
			DataContractSerializer serializer = CreateSerializer();
			using (var fs = new FileStream(pathname, FileMode.Create))
			{
				using (var writer = XmlDictionaryWriter.CreateTextWriter(fs, Encoding.UTF8))
				{
					var root = new RepositoryRoot()
					{
						Students = this.Students.ToList(),
						Categories = this.Categories.ToList(),
						Evaluations = this.Evaluations.ToList(),
					};

					serializer.WriteObject(writer, root);

					writer.Close();
				}
			}

			OnRepositorySaved(pathname);
		}

		/// <summary>
		/// Creates the new serializer.
		/// </summary>
		/// <returns>Serializer for this class</returns>
		private DataContractSerializer CreateSerializer()
		{
			return new DataContractSerializer(typeof(RepositoryRoot), null,
				int.MaxValue /*maxItemsInObjectGraph*/,
				false /*ignoreExtensionDataObject*/,
				true /*preserveObjectReferences : this is where the magic happens */,
				null /*dataContractSurrogate*/);
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
		/// <param name="pathname">The pathname of XML from which the repository has been loaded.</param>
		protected virtual void OnRepositoryLoaded(string pathname)
		{
		}

		/// <summary>
		/// Called when repository has been successfully saved.
		/// </summary>
		/// <param name="pathname">The pathname of XML into which the repository has been saved.</param>
		protected virtual void OnRepositorySaved(string pathname)
		{
		}
	}
}
