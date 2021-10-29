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

		public void InitMappingBuilderMappings()
		{
			var ms = new MappingSchema();
			
			var builder = ms.GetFluentMappingBuilder();

			// Mappings for StrigEnum properties using mapping builder
			_ = builder.Entity<Person>().Property(e => e.PersonType).HasConversion(v => v.Value, s => PersonType.Parse(s));
			_ = builder.Entity<Person>().Property(e => e.Title).HasConversion(v => v.Value, s => PersonTitle.Parse(s));

			// this is required mapping too, when using mapping builder 
			ms.SetDataType(typeof(PersonType), DataType.VarChar);
			ms.SetDataType(typeof(PersonTitle), DataType.VarChar);

			_ = AddMappingSchema(ms);
		}

		public void InitConverterMappings()
		{
			var ms = new MappingSchema();
			_ = AddMappingSchema(ms);

			// Mappings for StrigEnum properties using SetConverter()
			ms.SetConverter<string, PersonType>(s => PersonType.Parse(s));
			ms.SetConverter<PersonType, DataParameter>(val => new DataParameter { Value = val, DataType = DataType.VarChar });

			ms.SetConverter<string, PersonTitle>(s => PersonTitle.Parse(s));
			ms.SetConverter<PersonTitle, DataParameter>(val => new DataParameter { Value = val, DataType = DataType.VarChar });
		}
	}
}
