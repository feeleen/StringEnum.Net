using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToDB;
using LinqToDB.Data;
using System.Linq;
using System;

namespace DataTests
{
    [TestClass]
    public class DataTestsInit 
    {
        public static int TestEntityID { get; set; }
        
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            using (var db = new SetupDbContext())
            {
                try
                {
                    db.Execute("DROP DATABASE IF EXISTS [StringEnumTest]");
                }
                catch { }

                try
                {
                    db.Execute("CREATE DATABASE [StringEnumTest]");
                }
                catch { }
            }

            using (var db = new Linq2DbContext())
            {
                db.InitMappingBuilderMappings();
                
                //or use converters, which is equivalent to mapping builder and would work fine too
                //db.InitStringEnumConverterContextMappings();

                try
                {
                    db.DropTable<Person>();
                }
                catch { }

                db.CreateTable<Person>();
                TestEntityID = Convert.ToInt32(db.InsertWithIdentity(new Person() { Title = PersonTitle.Mss, PersonType = PersonType.EM }));
            }
        }
    }
}
