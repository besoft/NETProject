using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using System.Diagnostics.CodeAnalysis;
using Zcu.StudentEvaluator.Core.Collection;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class CollectionTest
	{
		class ItemClass
		{
			public string Name { get; set; }
			public ParentClass ParentClass { get; set; }
			public ParentClass ParentClassSpecial { get; set; }
		}

		class ParentClass
		{
			public ObservableCollectionWithParentReference<ItemClass, ParentClass> Collection;
			public ObservableCollectionWithParentReference<ItemClass, ParentClass> CollectionSpecial;
			
			public ParentClass ()
			{
				Collection = new ObservableCollectionWithParentReference<ItemClass, ParentClass>(this);
                CollectionSpecial = new ObservableCollectionWithParentReference<ItemClass, ParentClass>(this, "ParentClassSpecial");
			}            
		}

		[TestMethod]
		public void TestLink()
		{
			var parent = new ParentClass();
			Assert.AreEqual(null, parent.Collection.ParentReferencePropertyName);            
			Assert.AreEqual(parent, parent.Collection.Parent);

			Assert.AreEqual("ParentClassSpecial", parent.CollectionSpecial.ParentReferencePropertyName);
			Assert.AreEqual(parent, parent.CollectionSpecial.Parent);

			var item = new ItemClass()
			{
				Name = "#1",
			};

			parent.Collection.Add(item);
			Assert.AreEqual(1, parent.Collection.Count);
			Assert.AreEqual(item, parent.Collection[0]);
			Assert.AreEqual(parent, item.ParentClass);

			var item2 = new ItemClass()
			{
				Name = "#2",
			};

			parent.Collection[0] = item2;
			Assert.AreEqual(1, parent.Collection.Count);
			Assert.AreEqual(item2, parent.Collection[0]);
			Assert.AreEqual(parent, item2.ParentClass);
			Assert.AreEqual(null, item.ParentClass);

			parent.Collection.RemoveAt(0);
			Assert.AreEqual(0, parent.Collection.Count);            
			Assert.AreEqual(null, item2.ParentClass);

			///
			parent.CollectionSpecial.Add(item);
			Assert.AreEqual(1, parent.CollectionSpecial.Count);
			Assert.AreEqual(item, parent.CollectionSpecial[0]);
			Assert.AreEqual(parent, item.ParentClassSpecial);

			parent.CollectionSpecial[0] = item2;
			Assert.AreEqual(1, parent.CollectionSpecial.Count);
			Assert.AreEqual(item2, parent.CollectionSpecial[0]);
			Assert.AreEqual(parent, item2.ParentClassSpecial);
			Assert.AreEqual(null, item.ParentClassSpecial);

			parent.CollectionSpecial.RemoveAt(0);
			Assert.AreEqual(0, parent.CollectionSpecial.Count);
			Assert.AreEqual(null, item2.ParentClassSpecial);
		}
	}
}
