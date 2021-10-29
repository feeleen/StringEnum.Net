using System;
using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using Microsoft.Extensions.Configuration;
using StringEnum;

namespace DataTests
{
	public class SetupDbContext : DataConnection
	{
		public static IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();

		public SetupDbContext() : base("SqlServer", configuration.GetConnectionString("SetupConnection"))
		{
		}
	}

	public class Linq2DbContext : DataConnection
	{
		public static IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();

		public Linq2DbContext() : base("SqlServer", configuration.GetConnectionString("DefaultConnection"))
		{
		}

		public void InitStringEnumContextMappings()
		{
			var ms = new MappingSchema();
			_ = AddMappingSchema(ms);

			var builder = ms.GetFluentMappingBuilder();

			// For StrigEnum
			_ = builder.Entity<Person>().Property(e => e.PersonType).HasConversion(v => v.Value, s => PersonType.Parse(s));
			_ = builder.Entity<Person>().Property(e => e.Title).HasConversion(v => v.Value, s => PersonTitle.Parse(s));
		}

		public void InitStringEnumConverterContextMappings()
		{
			var ms = new MappingSchema();
			_ = AddMappingSchema(ms);

			var builder = ms.GetFluentMappingBuilder();

			ms.SetConverter<string, PersonType>(s => PersonType.Parse(s));
			ms.SetConverter<PersonType, DataParameter>(val => new DataParameter { Value = val, DataType = DataType.VarChar });

			ms.SetConverter<string, PersonTitle>(s => PersonTitle.Parse(s));
			ms.SetConverter<PersonTitle, DataParameter>(val => new DataParameter { Value = val, DataType = DataType.VarChar });
		}
	}
}
