namespace Stammbaum
{
	using ApS.Databases;

	public class Vornamen : Business
	{
		public Vornamen() : base("Vornamen", "", "", "", false, SqlAction.Null)
		{
		}
		public Vornamen(SqlAction eSqlAction) : base("Vornamen", "", "", "", false, eSqlAction)
		{
		}
		public Vornamen(string sWhere, string sOrder) : base("Vornamen", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Vornamen(string sColumns, string sWhere, string sOrder) : base( "Vornamen", sColumns, sWhere, sOrder,  true, SqlAction.Null)
		{
		}

		public static int? VornameToNummer(string vorname)
		{
			using (Vornamen v = new Vornamen())
			{
				v.where = "Vorname = '" + vorname + "'";
				v.Read();

				if (v.EoF)
					return null;
				else
					return v.Nummer;
			}
		}
		
		public string DbVorname
		{
			get { return this.GetString("VORNAME"); }
			set { this.Put("VORNAME", value); }
		}
		public char DbGeschlecht
		{
			get { return char.Parse(this.GetString("GESCHLECHT")); }
			set { this.Put("GESCHLECHT", value); }
		}
	}
}