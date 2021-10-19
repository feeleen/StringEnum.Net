using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StringEnum;
using System;
using System.ComponentModel;
using System.Threading;

namespace Tests
{
	[TestClass]
	public class TestBase
	{
		// This even wont compile, use "var Bag = (string) Foo" instead of "var Bag = Foo as string"
		//[TestMethod]
		//public void TestOperatorAs()
		//{
		//	var pet = MyPet.Dog as string;
		//	Assert.IsTrue(pet == "Dog");
		//}

		[TestMethod]
		public void TestIConvertibleChangeTypeToString()
		{
			var pet = MyPet.Dog;
			var res = Convert.ChangeType(pet, typeof(string));

			Assert.IsTrue((string)res == "Dog");
		}

		[TestMethod]
		public void TestMethod2()
		{
			var mouse = MyPet.Mouse;
			Assert.IsTrue(mouse == "Mouse");
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
		public void TestMethodNull()
		{
			string ghost = (MyPet)null;
			Assert.IsTrue(ghost == MyPet.Ghost);
		}

		[TestMethod]
		public void TestMethodNullTest()
		{
			string ghost = MyPet.Ghost;
			Assert.IsTrue(MyPet.Ghost is not null);
		}

		[TestMethod]
		public void TestMethodParseTest()
		{
			MyPet empty = MyPet.Parse("Cat");
			Assert.IsTrue(empty == MyPet.Cat);
		}

		[TestMethod]
		public void TestMethodEmptyTest()
		{
			MyPet empty = (MyPet) "";
			Assert.IsTrue(empty == MyPet.Empty);
		}

		[TestMethod]
		public void TestMethodParseEmptyTest()
		{
			MyPet empty = MyPet.Parse("");
			Assert.IsTrue(empty == MyPet.Empty);
		}

		[TestMethod]
		public void TestMethodImplicitOperator()
		{
			MyFlower fl = "Rose";
			Assert.IsTrue(fl == MyFlower.Rose);
		}

		[TestMethod]
		public void TestMethodTypeConverter()
		{
			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(MyFlower));
			var rose = (MyFlower) typeConverter.ConvertFrom(null, Thread.CurrentThread.CurrentCulture, "Rose");

			//this fails since string has limitations when converting to custom types:
			//MyFlower rosex = (MyFlower) Convert.ChangeType((object)"Rose", typeof(MyFlower)); 

			Assert.IsTrue(rose == MyFlower.Rose.ToString());
		}

		[TestMethod]
		public void TestMethodTypeConverterNull()
		{
			TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(MyFlower));
			var fl = (MyFlower)typeConverter.ConvertFrom(null, Thread.CurrentThread.CurrentCulture, null);

			Assert.IsTrue(fl == MyFlower.GhostFlower);
		}

		[TestMethod]
		public void TestMethodJson()
		{
			string jsonData = "{'FlowerType' : 'Rose', 'Quantity' : 2 }";
			var obj = JsonConvert.DeserializeObject<GardenFlower>(jsonData);

			Assert.IsTrue(obj.FlowerType == MyFlower.Rose.ToString());
		}

		[TestMethod]
		public void TestMethodJsonCompare()
		{
			string jsonData = "{'FlowerType' : 'Rose', 'Quantity' : 2 }";
			string jsonData2 = "{'FlowerType' : '', 'Quantity' : 1 }";
			var obj = JsonConvert.DeserializeObject<GardenFlower>(jsonData);
			var obj2 = JsonConvert.DeserializeObject<GardenFlower>(jsonData2);

			Assert.IsTrue(obj.FlowerType == MyFlower.Rose.ToString());
			Assert.IsTrue(obj2.FlowerType == MyFlower.Empty.ToString());
		}

		[TestMethod]
		public void TestMethodJsonWithNulls()
		{
			string jsonData = "{'FlowerType' : null, 'Quantity' : 2 }";
			var obj = JsonConvert.DeserializeObject<GardenFlower>(jsonData);

			Assert.IsTrue(obj.FlowerType == MyFlower.GhostFlower.ToString());
			Assert.IsTrue(obj.FlowerType == MyFlower.GhostFlower);
			Assert.IsFalse(obj.FlowerType == MyFlower.Hibiscus);
		}

		[TestMethod]
		public void TestAdditionalPropertyValue()
		{
			MyPet dog = (MyPet)"Dog";

			Assert.IsTrue(dog.AdditionalInfo == "Big bad dog!");
			
		}

		[TestMethod]
		public void TestTargetTypeConstructor()
		{
			MyFriend andy = (MyFriend)"Andy";

			Assert.IsTrue(andy == MyFriend.Andy);
		}
	}

	public class MyFriend : StringEnum<MyFriend>
	{
		public static MyFriend Andy => new();
		public static MyFriend George => new();
		public int? DefaultQuantity { get; protected set; } = 0;
	}

	[JsonConverter(typeof(JsonStringEnumConverter<MyPet>))]
	public class MyPet : StringEnum<MyPet>
	{
		//Use only New("") and don't use casting here, like: (MyPet)"Horse" -> this will lead to stack overflow, due to initialization looping

		public static MyPet Pig => new(); // = "Pig"
		public static MyPet Cat => New(); // = "Cat"
		public static MyPet Dog => New("Dog")
			.HasPropertyValue(x => x.AdditionalInfo, "Big bad dog!");

		public static MyPet Mouse => New("Mouse")
			.HasPropertyValue(x => x.DefaultQuantity, 15);

		public static MyPet Ghost => New(null);
		public static MyPet Empty => New(string.Empty);

		// You can mix values
		public static MyPet OldCat => New(Cat + Mouse);

		public string AdditionalInfo { get; protected set; }
		public int? DefaultQuantity { get; protected set; } = 0;
	}


	[JsonConverter(typeof(JsonStringEnumConverter<MyFlower>))]
	[TypeConverter(typeof(StringEnumConverter<MyFlower>))]
	public class MyFlower : StringEnum<MyFlower>
	{
		public static MyFlower Rose => New(EnumCase.Upper);
		public static MyFlower Hibiscus => New();

		public static MyFlower GhostFlower => New(null);

		public static MyFlower Empty => New(string.Empty);

		public static implicit operator MyFlower(string input)
		{
			return Parse(input);
		}
	}


	public class GardenFlower
	{
		public MyFlower FlowerType { get; set; }

		public int Quantity { get; set; }
	}
}
