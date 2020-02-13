namespace Stammbaum
{
	using System;

	public class Ausgabe : EventArgs
	{
		public string Message { get; }
		public object Sender { get; }
		public Ausgabe(object sender, string message)
		{
			this.Message = message;
			this.Sender = sender;
		}
	}

	public class Changed : EventArgs
	{
		public Person Person { get; }
		public ChangedProperty Property { get; }
		public object Value { get; }
		public Changed(Person person, ChangedProperty changedProperty, object value = null)
		{
			this.Person = person;
			this.Property = changedProperty;
			this.Value = value;
		}

		public enum ChangedProperty
		{
			Alter,
			Partner,
			Kinder,
			Geschwister,
			Lebend
		}
	}
}