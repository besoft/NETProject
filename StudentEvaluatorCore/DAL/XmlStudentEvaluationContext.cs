using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Zcu.StudentEvaluator.Model;

namespace Zcu.StudentEvaluator.DAL
{
	/// <summary>
	/// Data context for student evaluations.
	/// </summary>
	/// <remarks>
	/// This is the main class that coordinates functionality for a given data model with Xml persistency. 
	/// </remarks>
	public class XmlStudentEvaluationContext : LocalStudentEvaluationContext
	{
        /// <summary>
        /// Gets the filename of Xml file used to store the data.
        /// </summary>
        /// <value>
        /// The XML connection filename.
        /// </value>
		protected string XmlConnectionFilename { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalStudentEvaluationContext"/> class.
		/// </summary>
		/// <remarks>XmlConnectionFilename containing the data is automatically retrieved from application Properties</remarks>
		public XmlStudentEvaluationContext() : base()
		{
			try
			{
				this.XmlConnectionFilename = (
					from ConnectionStringSettings x in ConfigurationManager.ConnectionStrings
					where x.Name == "XmlConnectionFilename"
					select x.ConnectionString
					).SingleOrDefault();
			}
			catch (Exception)
			{
				
			}

			if (this.XmlConnectionFilename == null)
			{
				this.XmlConnectionFilename = "localData.xml";
			}

			Load();
		}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStudentEvaluationContext"/> class.
        /// </summary>
        /// <param name="xmlConnectionFilename">PathName to XML containing the data.</param>
		public XmlStudentEvaluationContext(string xmlConnectionFilename) : base()
		{
			this.XmlConnectionFilename = xmlConnectionFilename;

			Load();
		}

		/// <summary>
		/// Loads the data from the underlying Xml file into the local in-memory unitOfWork.
		/// </summary>
		protected virtual void Load()
		{
			if (!System.IO.File.Exists(this.XmlConnectionFilename))
				return;	//nothing to load

			using (XmlReader xmlReader = XmlReader.Create(this.XmlConnectionFilename))
			{
				xmlReader.ReadStartElement();

				XmlAttributeOverrides xmlOvers = CreateXmlAttributeOverrides();

				var serStudent = new XmlSerializer(this.Students.GetType(), xmlOvers);
				this.Students = (ICollection<Student>)serStudent.Deserialize(xmlReader);

				var serCategory = new XmlSerializer(this.Categories.GetType(), xmlOvers);
				this.Categories = (ICollection<Category>)serCategory.Deserialize(xmlReader);

				var serEvals = new XmlSerializer(this.Evaluations.GetType(), xmlOvers);
				this.Evaluations = (ICollection<Evaluation>)serEvals.Deserialize(xmlReader);

				xmlReader.ReadEndElement();
				xmlReader.Close();
			}
			
			//create connections
			foreach (var ev in this.Evaluations)
			{
				if (ev.Category != null)
				{
					ev.Category = this.Categories.Where(x => x.Id == ev.Category.Id).Single();
					ev.Category.Evaluations.Add(ev);
				}

				if (ev.Student != null)
				{
					ev.Student = this.Students.Where(x => x.Id == ev.Student.Id).Single();
					ev.Student.Evaluations.Add(ev);
				}
			}						
		}

		/// <summary>
		/// Saves all changes made in this context to the underlying physical stuff. 
		/// </summary>
		/// <returns>The number of objects written to the underlying Xml.</returns>
		public override int SaveChanges()
		{			
			using (XmlWriter xmlWriter = XmlWriter.Create(this.XmlConnectionFilename, new XmlWriterSettings()
				{
					Encoding = Encoding.UTF8,
					Indent = true,
				}))
			{
				xmlWriter.WriteStartElement("Root");

				XmlAttributeOverrides xmlOvers = CreateXmlAttributeOverrides();

				var serStudent = new XmlSerializer(this.Students.GetType(), xmlOvers);				
				serStudent.Serialize(xmlWriter, this.Students);				

				var serCategory = new XmlSerializer(this.Categories.GetType(), xmlOvers);				
				serCategory.Serialize(xmlWriter, this.Categories);				

				var serEvals = new XmlSerializer(this.Evaluations.GetType(), xmlOvers);				
				serEvals.Serialize(xmlWriter, this.Evaluations);				

				xmlWriter.WriteEndElement();
				xmlWriter.Close();
			}

			return this.Students.Count + this.Categories.Count + this.Evaluations.Count;
		}

		/// <summary>
		/// Creates the XML attribute overrides to be used with XmlSerializer.
		/// </summary>
		/// <returns>Created overrides</returns>
		private XmlAttributeOverrides CreateXmlAttributeOverrides()
		{
			XmlAttributeOverrides xmlOvers = new XmlAttributeOverrides();
			var xmlEvaluationsAttr = new XmlAttributes();
			xmlEvaluationsAttr.XmlIgnore = true;

			xmlOvers.Add(typeof(Student), "Evaluations", xmlEvaluationsAttr);
			xmlOvers.Add(typeof(Category), "Evaluations", xmlEvaluationsAttr);

			/*var xmlStudents = new XmlAttributes();
			xmlStudents.XmlArray = new XmlArrayAttribute("Students");				
			xmlOvers.Add(this.Students.GetType(), xmlStudents);
				
			var xmlCategories = new XmlAttributes();
			xmlCategories.XmlArray = new XmlArrayAttribute()
			{
				ElementName = "Categories"
			};

			xmlOvers.Add(this.Categories.GetType(), xmlCategories);

			var xmlEvaluations = new XmlAttributes();
			xmlEvaluations.XmlArray = new XmlArrayAttribute()
			{
				ElementName = "Evaluations"
			};

			xmlOvers.Add(this.Evaluations.GetType(), xmlEvaluations);
			*/
			return xmlOvers;
		}
	}
}
