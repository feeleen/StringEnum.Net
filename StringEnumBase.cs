using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StringEnum
{
	public abstract class StringEnumBase<T> : IEquatable<T>
		where T : StringEnumBase<T>
	{
		public string Value { get; protected set; }

		public override string ToString() => this.Value;

		public static T New(string value)
		{
			var objValue = Activator.CreateInstance<T>();
			objValue.Value = value;
			return objValue;
		}

		public static List<T> AsList()
		{
			return typeof(T)
				.GetProperties(BindingFlags.Public | BindingFlags.Static)
				.Where(p => p.PropertyType == typeof(T))
				.Select(p => (T)p.GetValue(null))
				.ToList();
		}

		public static T Parse(string value)
		{
			List<T> all = AsList();

			if (!all.Any(a => a.Value == value))
				throw new InvalidOperationException($"Value \"{value}\" is not a valid value for the type {typeof(T).Name}");

			return all.Single(a => a.Value == value);
		}

		public bool Equals(T other)
		{
			if (other == null) return false;
			return this.Value == other?.Value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj is T other) return this.Equals(other);
			return false;
		}

		public override int GetHashCode() => this.Value.GetHashCode();

		public static bool operator ==(StringEnumBase<T> a, StringEnumBase<T> b) => a?.Equals(b) ?? false;

		public static bool operator !=(StringEnumBase<T> a, StringEnumBase<T> b) => !(a?.Equals(b) ?? false);
	}
}
