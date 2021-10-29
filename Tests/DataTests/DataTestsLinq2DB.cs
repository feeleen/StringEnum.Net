using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringEnum;

namespace DataTests
{
	[TestClass]
	public class DataTestsLinq2DB
	{
		public static async Task<List<Person>> GetPersonsTableAsync()
		{
			using (var db = new Linq2DbContext())
			{
				db.InitMappingBuilderMappings();

				var persons = db.GetTable<Person>().Select(x=> x).ToList();

				var pers = new Person() { PersonType = PersonType.VC, BusinessEntityID = DataTestsInit.TestEntityID };

				var res = await db.UpdateAsync(pers, (a, b) => b.ColumnName == nameof(Person.PersonType));
				return new List<Person>() { pers };
			}
		}

		public static async Task<List<Person>> GetPersonsTableAsync2()
		{
			using (var db = new Linq2DbContext())
			{
				db.InitConverterMappings();

				var persons = await db.GetTable<Person>().ToListAsync();

				var pers = new Person() { PersonType = PersonType.VC, BusinessEntityID = DataTestsInit.TestEntityID };

				var res = await db.UpdateAsync(pers, (a, b) => b.ColumnName == nameof(Person.PersonType));
				return new List<Person>() { pers };
			}
		}

		[TestMethod]
		public async Task TestStringEnum()
		{
			var res = await GetPersonsTableAsync();
			Assert.IsTrue(res.Count > 0);
		}

		[TestMethod]
		public async Task TestStringEnum2()
		{
			var res = await GetPersonsTableAsync2();
			Assert.IsTrue(res.Count > 0);
		}

		[TestMethod]
		public async Task TestGroup()
		{
			using (var db = new Linq2DbContext())
			{
				db.InitConverterMappings();

				var uniqueLastNames = await
					db.GetTable<Person>()
						.GroupBy(x => x.PersonType)
						.Where(xs => xs.Count() == 1)
						.SelectMany(xs => xs)
						.ToListAsync();

				Assert.IsTrue(uniqueLastNames.Count == 1);
			}
		}

		[TestMethod]
		public async Task TestUpdate()
		{
			using (var db = new Linq2DbContext())
			{
				db.InitConverterMappings();

				var res = await db.GetTable<Person>().Where(x => x.BusinessEntityID == 1).Set(x=> x.PersonType, PersonType.IN).UpdateAsync();

				Assert.IsTrue(res > 0);
			}
		}

		[TestMethod]
		public async Task TestUpdate2()
		{
			using (var db = new Linq2DbContext())
			{
				db.InitMappingBuilderMappings();

				var res = await db.GetTable<Person>().Where(x => x.BusinessEntityID == 1).Set(x => x.PersonType, PersonType.IN).UpdateAsync();

				Assert.IsTrue(res > 0);
			}
		}
	}
}
