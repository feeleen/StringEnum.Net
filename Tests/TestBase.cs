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

        [TestMethod]
        public void TestMethodIgnoreCase()
        {
            string cat = (MyPet)"cat";
            Assert.IsTrue(cat == "Cat");
        }

        [TestMethod]
        public void TestMethodEnumUpperCase()
        {
            string cat = (MyFlower)"rose";
            Assert.IsTrue(cat == "ROSE");
        }

        [TestMethod]
        public void TestMethodJson()
        {
            string jsonData = "{'FlowerType' : 'Rose', 'Quantity' : 2 }";
            var obj = JsonConvert.DeserializeObject<GardenFlower>(jsonData);

            Assert.IsTrue(obj.FlowerType == "ROSE");
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<MyPet>))]
    public class MyPet : StringEnumBase<MyPet>
    {
        public static MyPet Cat => New(); // = "Cat"
        public static MyPet Dog => New("Dog");
        public static MyPet Mouse => New("Mouse");
    }


    [JsonConverter(typeof(JsonStringEnumConverter<MyFlower>))]
    public class MyFlower : StringEnumBase<MyFlower>
    {
        public static MyFlower Rose => New(EnumCase.Upper);
        public static MyFlower Hibiscus => New();
    }


    public class GardenFlower
    {
        public MyFlower FlowerType { get; set; }

        public int Quantity { get; set; }
    }
}
