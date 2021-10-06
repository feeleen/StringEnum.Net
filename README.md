[![Build status](https://ci.appveyor.com/api/projects/status/dkm6x44rnqwsrbly?svg=true)](https://ci.appveyor.com/project/feeleen/stringenum-net) [![NuGet Badge](https://buildstats.info/nuget/StringEnum.Net)](https://www.nuget.org/packages/StringEnum.Net/)

# StringEnum.NET - Enumeration-like string values

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

# .NET 5.0

Define your StringEnum:

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

	public bool Bar()
	{
		var mouse = MyPet.Mouse;
		if (mouse == "Mouse")
		{
			return true;
		}
		
		if (this.pet == MyPet.Dog || this.pet == MyPet.Parse("Cat"))
		{
			return true;
		}
		
		
		// implicit conversions example
		string cat = MyPet.Cat;
            	Assert.IsTrue(cat == "Cat");
		
		string cat = (MyPet)"cat";
            	Assert.IsTrue(cat == "Cat");
	}
}
```

JSON conversion example:

```cs
[JsonConverter(typeof(JsonStringEnumConverter<MyFlower>))]
public class MyFlower : StringEnumBase<MyFlower>
{
    public static MyFlower Rose => New();
    public static MyFlower Hibiscus => New();
}

public class GardenFlower
{
    public MyFlower FlowerType { get; set; }
    public int Quantity { get; set; }
}

string jsonData = "{'FlowerType' : 'Rose', 'Quantity' : 2 }";
var obj = JsonConvert.DeserializeObject<GardenFlower>(jsonData);

Assert.IsTrue(obj.FlowerType == "Rose");
Assert.IsTrue(obj.FlowerType == MyFlower.Rose);

```


# Use with data entities 
[Linq2db](https://github.com/linq2db/linq2db) example:

```cs
public class PersonType : StringEnum<PersonType>
{
	public static PersonType EM => New();
	public static PersonType SP => New();
	public static PersonType SC => New();
	public static PersonType VC => New();
	public static PersonType IN => New();
	public static PersonType GC => New();
}

[Table(Name = "[Person].[Person]")]
public class EnumPerson
{
	[PrimaryKey, Identity]
	public int BusinessEntityID { get; set; }
	[Column]
	public PersonType PersonType { get; set; }
}
```
linq2db required Mapping initialization:
```cs
var ms = new MappingSchema();
_ = dbContext.AddMappingSchema(ms);

var builder = ms.GetFluentMappingBuilder();
```
Type conversion setup:
```cs
_ = builder.Entity<Person>().Property(e => e.PersonType).HasConversion(v => v.Value, s => PersonType.Parse(s));

//or:

ms.SetConverter<string, PersonType>(s => PersonType.Parse(s));
ms.SetConverter<PersonType, DataParameter>(val => new DataParameter { Value = val.Value, DataType = DataType.VarChar });
```
usage:
```cs
var pers = new Person() { PersonType = PersonType.VC, BusinessEntityID = 1675 };
var res = await db.UpdateAsync(pers)

```
