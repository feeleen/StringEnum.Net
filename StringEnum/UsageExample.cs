using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringEnum
{
	[JsonConverter(typeof(JsonStringEnumConverter<MyPet>))]
	public class MyPet : StringEnumBase<MyPet>
	{
		public static MyPet Cat => New(); // = "Cat"
		public static MyPet Dog => New("Dog");
		public static MyPet Mouse => New("Mouse");
	}

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
		}
	}
}
