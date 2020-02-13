namespace Stammbaum
{
	using System;
	using ApS.Databases;

	public class PersonenV : Business
	{
		public PersonenV() : base("PersonenV", "", "", "", false, SqlAction.Null)
		{
		}
		public PersonenV(SqlAction eSqlAction) : base("PersonenV", "", "", "", false, eSqlAction)
		{
		}
		public PersonenV(string sWhere, string sOrder) : base("PersonenV", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public PersonenV(string sColumns, string sWhere, string sOrder) : base("PersonenV", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public int? NummerVater()
		{
			if (string.IsNullOrEmpty(this.DbVater))
				return null;

			using (Personen p = new Personen())
			{
				p.Where = "Vorname_Nr = " + Vornamen.VornameToNummer(this.DbVater.Substring(0, this.DbVater.IndexOf(' '))) + " AND Familie_Nr = " + Familien.NachnameToNummer(this.DbVater.Substring(this.DbVater.IndexOf(' ') + 1));
				p.Read();

				if (p.EoF)
					return null;
				else if (p.RecordCount > 1)
					return null;
				else
					return p.Nummer;
			}
		}

		public int? NummerMutter()
		{
			if (string.IsNullOrEmpty(this.DbMutter))
				return null;

			using (Personen p = new Personen())
			{
				p.Where = "Vorname_Nr = " + Vornamen.VornameToNummer(this.DbMutter.Substring(0, this.DbMutter.IndexOf(' '))) + " AND Familie_Nr = " + Familien.NachnameToNummer(this.DbMutter.Substring(this.DbMutter.IndexOf(' ') + 1));
				p.Read();

				if (p.EoF)
					return null;
				else if (p.RecordCount > 1)
					return null;
				else
					return p.Nummer;
			}
		}

		public string DbVorname
		{
			get { return this.GetString("VORNAME"); }
		}
		public string DbFamilienname
		{
			get { return this.GetString("FAMILIENNAME"); }
		}
		public DateTime DbGeburtsdatum
		{
			get { return this.GetDateTime("GEBURTSDATUM"); }
		}
		public int DbGeneration
		{
			get { return this.GetInt("GENERATION"); }
		}
		public bool DbLebend
		{
			get { return this.GetBool("LEBEND"); }
		}
		public string DbMutter
		{
			get { return this.GetString("MUTTER"); }
		}
		public string DbVater
		{
			get { return this.GetString("VATER"); }
		}
		public string DbPartner
		{
			get { return this.GetString("PARTNER"); }
		}
	}
}
