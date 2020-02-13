namespace Stammbaum
{
	using ApS.Databases;

	public class Einstellungen : Business
	{
		public Einstellungen() : base("Einstellungen", "", "", "", false, SqlAction.Null)
		{
		}
		public Einstellungen(SqlAction eSqlAction) : base("Einstellungen", "", "", "", false, eSqlAction)
		{
		}
		public Einstellungen(string sWhere, string sOrder) : base("Einstellungen", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Einstellungen(string sColumns, string sWhere, string sOrder) : base("Einstellungen", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Einstellungen(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("Einstellungen", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		
		public string GetSetting(string key)
		{
			this.Where = "SettingKey = '" + key + "'";
			this.Read();
			return this.DbSettingValue;
		}

		public string SetSetting(string key, string value)
		{
			try
			{
				this.Where = "SettingKey = '" + key + "'";
				this.Read();

				this.DbSettingKey = key;
				this.DbSettingValue = value;

				if (this.EoF)
					this.BuildSaveStmt(SqlAction.Insert);
				else
					this.BuildSaveStmt(SqlAction.Update);

				this.SaveStmtsOnly();
				return "";
			}
			catch (FirebirdSql.Data.FirebirdClient.FbException fbex)
			{
				return fbex.Message;
			}
		}

		public string DbSettingKey
		{
			get { return this.GetString("SETTINGKEY"); }
			set { this.Put("SETTINGKEY", value); }
		}
		public string DbSettingValue
		{
			get { return this.GetString("SETTINGVALUE"); }
			set { this.Put("SETTINGVALUE", value); }
		}
	}
}