```cs

public class MyPet : StringEnumBase<MyPet>
{
	public static MyPet Cat => New(); // = "Cat"
	public static MyPet Rabbit => New("Rabbit");
	public static MyPet Dog => New(EnumCase.Upper, "Dog"); // = DOG
	public static MyPet Mouse => New(EnumCase.AsIs);
}


// usage

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
		
		string cat = MyPet.Cat;
            	Assert.IsTrue(cat == "Cat");
		
		string cat = (MyPet)"cat";
            	Assert.IsTrue(cat == "Cat");
	}
}


// 


```
