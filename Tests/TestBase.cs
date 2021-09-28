using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StringEnum;

namespace Tests
{
    [TestClass]
    public class TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var pet = MyPet.Dog;

            Assert.IsTrue(pet == MyPet.Dog);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mouse = MyPet.Mouse;
            Assert.IsTrue(mouse == "Mouse");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var cat = MyPet.Cat;
            Assert.IsTrue(cat == "Cat");
        }

        [TestMethod]
        public void TestMethod4()
        {
            MyPet cat = (MyPet)"Cat";
            Assert.IsTrue(cat == "Cat");
        }

        [TestMethod]
        public void TestMethod5()
        {
            string cat = MyPet.Cat;
            Assert.IsTrue(cat == "Cat");
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<MyPet>))]
    public class MyPet : StringEnumBase<MyPet>
    {
        public static MyPet Cat => New(); // = "Cat"
        public static MyPet Dog => New("Dog");
        public static MyPet Mouse => New("Mouse");
    }
}
