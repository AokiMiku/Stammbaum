namespace Stammbaum
{
	using System;
	using ApS.Databases;

	public class Personen : Business
	{

		public Personen() : base("Personen", "", "", "", false, SqlAction.Null)
		{
			this.AddToNullErlaubt("Mutter_Nr", "Vater_Nr", "Partner_Nr", "BekommtKindAnTag");
		}
		public Personen(SqlAction eSqlAction) : base("Personen", "", "", "", false, eSqlAction)
		{
			this.AddToNullErlaubt("Mutter_Nr", "Vater_Nr", "Partner_Nr", "BekommtKindAnTag");
		}
		public Personen(string sWhere, string sOrder) : base("Personen", "", sWhere, sOrder, true, SqlAction.Null)
		{
			this.AddToNullErlaubt("Mutter_Nr", "Vater_Nr", "Partner_Nr", "BekommtKindAnTag");
		}
		public Personen(string sColumns, string sWhere, string sOrder) : base("Personen", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
			this.AddToNullErlaubt("Mutter_Nr", "Vater_Nr", "Partner_Nr", "BekommtKindAnTag");
		}
		
		public int? DbVorname_Nr
		{
			get { return this.GetInt("VORNAME_NR"); }
			set { this.Put("VORNAME_NR", value); }
		}
		public int? DbFamilie_Nr
		{
			get { return this.GetInt("FAMILIE_NR"); }
			set { this.Put("FAMILIE_NR", value); }
		}
		public DateTime DbGeburtsdatum
		{
			get { return this.GetDateTime("GEBURTSDATUM"); }
			set { this.Put("GEBURTSDATUM", value); }
		}
		public int? DbGeneration
		{
			get { return this.GetInt("GENERATION"); }
			set { this.Put("GENERATION", value); }
		}
		public int? DbMutter_Nr
		{
			get { return this.GetInt("MUTTER_NR"); }
			set { this.Put("MUTTER_NR", value); }
		}
		public int? DbVater_Nr
		{
			get { return this.GetInt("VATER_NR"); }
			set { this.Put("VATER_NR", value); }
		}
		public int? DbPartner_Nr
		{
			get { return this.GetInt("PARTNER_NR"); }
			set { this.Put("PARTNER_NR", value); }
		}
		public bool DbLebend
		{
			get { return this.GetBool("LEBEND"); }
			set { this.Put("LEBEND", value); }
		}
		public DateTime? DbBekommtKindAnTag
		{
			get { return this.GetDateTime("BEKOMMTKINDANTAG"); }
			set { this.Put("BEKOMMTKINDANTAG", value); }
		}
	}
}