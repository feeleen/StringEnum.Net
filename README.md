```cs

public class MyPet : StringEnumBase<MyPet>
{
	public static MyPet Cat => New(); // = "Cat"
  public static MyPet Rabbit => New("Rabbit");
	public static MyPet Dog => New(EnumCase.Upper, "Dog"); // = DOG
	public static MyPet Mouse => New(EnumCase.Lower);
}

```
