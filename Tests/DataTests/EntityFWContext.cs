using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataTests
{
	public class EntityFWContext : Microsoft.EntityFrameworkCore.DbContext
	{
		public static IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();

		public DbSet<Person> Persons { get; set; }
		public EntityFWContext()
		{
			Database.EnsureCreated();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Person>()
				.Property(e => e.PersonType)
				.HasConversion(
					v => v.ToString(),
					v => PersonType.Parse(v));

			modelBuilder
					.Entity<Person>()
					.Property(e => e.Title)
					.HasConversion(
						v => v.ToString(),
						v => PersonTitle.Parse(v));

			// использование Fluent API
			base.OnModelCreating(modelBuilder);
		}
	}
}
