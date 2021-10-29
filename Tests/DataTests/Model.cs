using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StringEnum;

namespace DataTests
{
	[Keyless]
	public class PersonType : StringEnum<PersonType>
	{
		public static PersonType EM =  New();
		public static PersonType SP =  New();
		public static PersonType SC =  New();
		public static PersonType VC =  New();
		public static PersonType IN =  New();
		public static PersonType GC =  New();
	}

	[Keyless]
	public class PersonTitle : StringEnum<PersonTitle>
	{
		public static PersonTitle Mr => New("Mr.");
		public static PersonTitle Ms => New("Ms.");
		public static PersonTitle Mss => New("Ms");
		public static PersonTitle Mrs => New("Mrs.");
		public static PersonTitle Sr => New("Sr.");
		public static PersonTitle Sra => New("Sra.");
		public static PersonTitle Undefined => New(null);
	}

	public class EntityBase
	{ 
		[Column]
		[Key]
		[LinqToDB.Mapping.PrimaryKey, LinqToDB.Mapping.Identity]
		[LinqToDB.Mapping.Column]
		public int BusinessEntityID { get; set; }
	}


	[LinqToDB.Mapping.Table(Name = "[Person]")]
	[Table("Person")]
	public class Person : EntityBase
	{
		[LinqToDB.Mapping.Column]
		[Column]
		public PersonType PersonType { get; set; }
		[LinqToDB.Mapping.Column]
		[Column]
		public PersonTitle Title { get; set; }
	}
}
