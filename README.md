[![Build status](https://ci.appveyor.com/api/projects/status/dkm6x44rnqwsrbly?svg=true)](https://ci.appveyor.com/project/feeleen/stringenum-net) [![NuGet Badge](https://buildstats.info/nuget/StringEnum.Net)](https://www.nuget.org/packages/StringEnum.Net/)

# StringEnum.NET - Enumeration-like string values

.NET 5.0

Defining your StringEnum:

```cs

public class MyPet : StringEnumBase<MyPet>
{
	public static MyPet Cat => New(); // = "Cat"
	public static MyPet Rabbit => New("Rabbit");
	public static MyPet Dog => New(EnumCase.Upper, "Dog"); // MyPet.Dog.ToString() -> "DOG"
	public static MyPet Ghost => New(null);  // handy when values in dataobject may have null values
	public static MyPet Empty => New(String.Empty);
}

```

Usage example:

```cs
public class Foo
{
	public MyPet pet { get; }

	public Foo(MyPet pet) => this.pet = pet;

	public bool Bar()
	{
		var mouse = MyPet.Mouse;
		if (mouse == "Mouse")
			return true;
		else
		{
			if (this.pet == MyPet.Dog || this.pet == MyPet.Mouse)
				return true;
			else
				return false;
		}
		
		
		// implicit conversions example
		string cat = MyPet.Cat;
            	Assert.IsTrue(cat == "Cat");
		
		string cat = (MyPet)"cat";
            	Assert.IsTrue(cat == "Cat");
	}
}
```

JSON conversion example

```cs
[JsonConverter(typeof(JsonStringEnumConverter<MyFlower>))]
public class MyFlower : StringEnumBase<MyFlower>
{
    public static MyFlower Rose => New();
    public static MyFlower Hibiscus => New();
}

string jsonData = "{'FlowerType' : 'Rose', 'Quantity' : 2 }";
var obj = JsonConvert.DeserializeObject<MyFlower>(jsonData);

Assert.IsTrue(obj.FlowerType == "Rose");

```
