using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zcu.StudentEvaluator.Core.Data;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Zcu.StudentEvaluator.Core.UnitTest
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class RecursionPointTest
	{
		[TestMethod]
		public void TestRecursion()
		{
			using (RecursionPoint rp = new RecursionPoint(this))
			{
				Assert.AreEqual(0, rp.RecursiveDepth());
				Assert.AreEqual(false, rp.IsRecursive());
			}

			using (RecursionPoint rp = new RecursionPoint(this))
			{
				Assert.AreEqual(0, rp.RecursiveDepth());
				Assert.AreEqual(false, rp.IsRecursive());
			}
		}

		[TestMethod]
		public void TestRecursion2()
		{
			MethodA();
		}

		private void MethodA(int entry = 0)
		{
			using (RecursionPoint rp = new RecursionPoint(this))
			{
				if (entry == 0)
				{
					Assert.AreEqual(0, rp.RecursiveDepth());
					Assert.AreEqual(false, rp.IsRecursive());

					MethodA(entry + 1);
				}
				else
				{
					Assert.AreEqual(1, rp.RecursiveDepth());
					Assert.AreEqual(true, rp.IsRecursive());
				}
			}
		}

		[TestMethod]
		public void TestRecursion3()
		{
			MethodStatic();
		}

		private static void MethodStatic(int entry = 0)
		{
			using (RecursionPoint rp = new RecursionPoint(typeof(RecursionPointTest)))
			{
				if (entry == 0)
				{
					Assert.AreEqual(0, rp.RecursiveDepth());
					Assert.AreEqual(false, rp.IsRecursive());

					MethodStatic(entry + 1);
				}
				else
				{
					Assert.AreEqual(1, rp.RecursiveDepth());
					Assert.AreEqual(true, rp.IsRecursive());
				}
			}
		}
	}
}
