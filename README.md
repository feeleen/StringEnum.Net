[![Build status](https://ci.appveyor.com/api/projects/status/dkm6x44rnqwsrbly?svg=true)](https://ci.appveyor.com/project/feeleen/stringenum-net) [![NuGet Badge](https://buildstats.info/nuget/StringEnum.Net)](https://www.nuget.org/packages/StringEnum.Net/)

# StringEnum.NET - Enumeration-like string values

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

# .NET 5.0, .NET 6.0, .NET 7.0

Define your `StringEnum` clean and simple:

```cs
public class MyPet : StringEnum<MyPet>
{
	public static MyPet Monkey = New(); // = "Monkey" 
	public static MyPet Cat = New(); 
	public static MyPet Rabbit = New();
}
```

More initialization options:

```cs

public class MyPet : StringEnum<MyPet>
{
	public static MyPet Rabbit = New("Rabbits");
	public static MyPet Dog = New(EnumCase.Upper, "Dog"); // MyPet.Dog.ToString() -> "DOG"
	public static MyPet Ghost = New(null);  // handy when values in dataobject may have null values
	public static MyPet Empty = New(String.Empty);
}
```

Add additional properties for yout StringEnum:

```cs
public class MyFlower : StringEnum<MyFlower>
{
	public static MyFlower Rose = New().HasPropertyValue(x => x.AdditionalInfo, "Big");
	
	public static MyFlower Camomile = New()
		.HasPropertyValue(x => x.AdditionalInfo, "Small")
		.HasPropertyValue(x => x.DefaultQuantity, 15);
	
	//your custom properties here:
	public string AdditionalInfo { get; protected set; }
	public int? DefaultQuantity { get; protected set; }
}

```

Usage example:

```cs
var mouse = MyPet.Mouse;

if (mouse == "Mouse") // compare with string without .ToString()
{
	return true;
}

if (mouse == MyPet.Parse("mouse")) // parsing is case insensitive
{
	return true;
}


// implicit conversions example
string cat = MyPet.Cat;
Assert.IsTrue(cat == "Cat");

string dog = (MyPet)"Dog";
Assert.IsTrue(dog == "Dog");

```

`JSON` conversion example:

```cs
[JsonConverter(typeof(JsonStringEnumConverter<MyFlower>))]
public class MyFlower : StringEnum<MyFlower>
{
    public static MyFlower Rose = New();
    public static MyFlower Hibiscus = New();
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

`TypeConverter` and `IConvertible` support to work with strings:

```cs
[TypeConverter(typeof(StringEnumConverter<MyFlower>))] // default type converter
public class MyFlower : StringEnum<MyFlower>
{
    public static MyFlower Rose = New();
    public static MyFlower Hibiscus = New();
}

TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(MyFlower));
var rose = (MyFlower) typeConverter.ConvertFrom(null, Thread.CurrentThread.CurrentCulture, "Rose");

var fl = MyFlower.Hibiscus;
var res = Convert.ChangeType(fl, typeof(string));

```

# Use with data entities 
### [Linq2db](https://github.com/linq2db/linq2db):

1. Define your model:

	```cs
	public class PersonType : StringEnum<PersonType>
	{
		public static PersonType EM = New();
		public static PersonType SP = New();
		public static PersonType SC = New();
		public static PersonType VC = New();
		public static PersonType IN = New();
		public static PersonType GC = New();
	}

	[Table(Name = "[Person].[Person]")]
	public class Person
	{
		[PrimaryKey, Identity]
		public int BusinessEntityID { get; set; }
		[Column]
		public PersonType PersonType { get; set; } // string values "EM", "SP", "SC" ... etc.
	}
	```
2. Setup type conversion via `MappingSchema`:

	```cs
	var ms = new MappingSchema();
	_ = dbContext.AddMappingSchema(ms);

	var builder = ms.GetFluentMappingBuilder();
	```
	and type conversion setup:
	```cs
	_ = builder.Entity<Person>().Property(e => e.PersonType).HasConversion(v => v.Value, s => PersonType.Parse(s));
	ms.SetDataType(typeof(PersonType), DataType.VarChar);
	```
	or
	```cs

	ms.SetConverter<string, PersonType>(s => PersonType.Parse(s));
	ms.SetConverter<PersonType, DataParameter>(val => new DataParameter { Value = val, DataType = DataType.VarChar });
	```
3. Usage:

	```cs

	// select records
	var persons = await db.GetTable<Person>().ToListAsync();

	// update record
	var pers = new Person() { PersonType = PersonType.VC, BusinessEntityID = 1675 };
	var res = await db.UpdateAsync(pers)

	```


### EF Core:

```cs

[Keyless]
public class PersonTitle : StringEnum<PersonTitle>
{
	public static PersonTitle Mr = New("Mr.");
	public static PersonTitle Ms = New("Ms.");
	public static PersonTitle Mss = New("Ms");
	public static PersonTitle Mrs = New("Mrs.");
	public static PersonTitle Sr = New("Sr.");
	public static PersonTitle Sra = New("Sra.");
	public static PersonTitle Undefined = New(null);
}

// setup converter:
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
	modelBuilder
		.Entity<Person>()
		.Property(e => e.Title)
		.HasConversion(
			v => v.ToString(),
			v => PersonTitle.Parse(v));

	base.OnModelCreating(modelBuilder);
}

```
	
