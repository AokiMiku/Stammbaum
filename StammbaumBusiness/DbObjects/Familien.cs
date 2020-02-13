namespace Stammbaum
{ 
	using ApS.Databases;

	public class Familien : Business
	{
		public Familien() : base("Familien", "", "", "", false, SqlAction.Null)
		{
		}
		public Familien(SqlAction eSqlAction) : base("Familien", "", "", "", false, eSqlAction)
		{
		}
		public Familien(string sWhere, string sOrder) : base("Familien", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Familien(string sColumns, string sWhere, string sOrder) : base("Familien", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		
		public static int? NachnameToNummer(string nachname)
		{
			using (Familien f = new Familien())
			{
				f.where = "Familienname = '" + nachname + "'";
				f.Read();

				if (f.EoF)
					return null;
				else
					return f.Nummer;
			}
		}

		public string DbFamilienname
		{
			get { return this.GetString("FAMILIENNAME"); }
			set { this.Put("FAMILIENNAME", value); }
		}
	}
}