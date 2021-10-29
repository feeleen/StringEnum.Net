using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTests
{
	[TestClass]
	public class DataTestsEntityFW
	{
		[TestMethod]
		public void TestMethod1()
		{
			var newPerconType = PersonType.EM;

			using (EntityFWContext db = new EntityFWContext())
			{
				var pers = db.Persons.Find(DataTestsInit.TestEntityID);
				pers.PersonType = pers.PersonType == PersonType.EM ? PersonType.IN : PersonType.EM;
				newPerconType = pers.PersonType;
				db.Entry(pers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

				Assert.AreEqual(db.SaveChanges(), 1);
			}

			using (EntityFWContext db = new EntityFWContext())
			{
				var persRes = db.Persons.Find(DataTestsInit.TestEntityID);

				Assert.IsTrue(persRes.PersonType == newPerconType);
			}
		}

		[TestMethod]
		public void TestMethod3()
		{
			var newPerconType = PersonType.SC;

			using (EntityFWContext db = new EntityFWContext())
			{
				var pers = db.Persons.Find(DataTestsInit.TestEntityID);
				pers.PersonType = pers.PersonType == PersonType.SC ? PersonType.SP : PersonType.SC;
				newPerconType = pers.PersonType;
				db.Entry(pers).CurrentValues.SetValues(pers);
				Assert.AreEqual(db.SaveChanges(), 1);
			}

			using (EntityFWContext db = new EntityFWContext())
			{
				var persRes = db.Persons.Find(DataTestsInit.TestEntityID);

				Assert.IsTrue(persRes.PersonType == newPerconType);
			}
		}

		[TestMethod]
		public void TestMethod2()
		{
			var newPerconType = PersonType.VC;

			using (EntityFWContext db = new EntityFWContext())
			{
				var pers = new Person() { PersonType = newPerconType, BusinessEntityID = DataTestsInit.TestEntityID };
				db.Attach(pers);
				db.Entry(pers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

				Assert.AreEqual(db.SaveChanges(), 1);
			}

			using (EntityFWContext db = new EntityFWContext())
			{
				var persRes = db.Persons.Find(DataTestsInit.TestEntityID);

				Assert.IsTrue(persRes.PersonType == newPerconType);
			}
		}
	}
}
