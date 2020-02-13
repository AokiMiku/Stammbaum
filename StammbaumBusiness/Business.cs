namespace Stammbaum
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data;
	using ApS;
    using ApS.Databases;
	using ApS.Databases.Firebird;

	/// <summary>
	/// Basisklasse für Business-Objekte
	/// </summary>
	public class Business : ApS.Databases.Firebird.Business
	{
		protected DataSetAdvanced _oDsAddition;
		public DataSetAdvanced _oDsInfoDataSet = null;
		public Save FbSave
		{
			get { return this.fbSave; }
		}
		public Business() : base()
		{

		}

		protected Business(string tabelle, string spalten,
			string where, string orderBy, bool read,
			SqlAction eSqlAction) : base(tabelle)
		{
			this.tabelle = tabelle;
			this.spalten = spalten;
			this.where = where;
			this.orderBy = orderBy;
			if (read)
				this.Read();

			if (eSqlAction != SqlAction.Null)
			{
				this.fbSave.Action = eSqlAction;
				this.saveKind = this.SQLActionToDefines(eSqlAction);
			}
		}
		public void InitInfoDataSet()
		{
			this._oDsInfoDataSet = new DataSetAdvanced("Info");
			this._oDsInfoDataSet.AddColumn("Bezeichnung");
			this._oDsInfoDataSet.AddColumn("Ident");
			this._oDsInfoDataSet.AddColumn("Tag", typeof(object));
		}

		public DataSetAdvanced InfoDataSet
		{
			get
			{
				if (this._oDsInfoDataSet == null)
					this.InitInfoDataSet();
				return this._oDsInfoDataSet;
			}
			set { this._oDsInfoDataSet = value; }
		}
		/// <summary>
		///
		/// </summary>
		public override void Dispose(bool bDisposing)
		{
			base.Dispose(bDisposing);
			try
			{
				if (this._oDsInfoDataSet != null)
				{
					this._oDsInfoDataSet.Clear();
					this._oDsInfoDataSet.Dispose();
				}
				if (this._oDsAddition != null)
				{
					this._oDsAddition.Clear();
					this._oDsAddition.Dispose();
				}
			}
			catch (Exception ex)
			{
				Services.WriteErrorLog(ex, "silent Business4All.Dispose()");
			}
		}
		
		/// <summary>
		/// abgeleitete Where wg. Prüfung _ in Like-Suche
		/// </summary>
		public string Where
		{
			get
			{
				return base.where;
			}
			set
			{
				if (value.IndexOf("_") != -1 && value.ToUpper().IndexOf("LIKE") != -1)
				{
					string[] a = value.Split(' ');
					string sValue = "";
					foreach (string s in a)
					{
						if (s.IndexOf("%") != -1 && s.IndexOf("_") != -1 && s.IndexOf("[_]") != -1)
						{
							sValue += s.Replace("_", "[_]") + " ";
						}
						else
							sValue += s + " ";
					}
					value = sValue;
				}
				base.where = value;
			}
		}

		public string OrderBy
		{
			get { return this.orderBy; }
			set { this.orderBy = value; }
		}

		public virtual DataSetAdvanced Addition()
		{
			if (this._oDsAddition == null)
			{
				this._oDsAddition = new DataSetAdvanced("Addition");
				this._oDsAddition.AddColumn("Bezeichnung");
				this._oDsAddition.AddColumn("Wert", Type.GetType("System.Decimal"));
			}
			int iAnzahl = 0;
			int iRecno = this.DataSet.RecordNumber;
			this.DataSet.GoTop();
			while (!this.DataSet.EoF)
			{
				iAnzahl += 1;
				this.DataSet.Skip();
			}
			this.DataSet.GoTo(iRecno);
			this._oDsAddition.Append();
			this._oDsAddition.Put("Bezeichnung", "Anzahl");
			this._oDsAddition.Put("Wert", Services.ToDecimal(iAnzahl));

			return this._oDsAddition;
		}
		public bool IsIdentWhere
		{
			get { return this.where.IndexOf("IDENT = '") == 0 && this.where.IndexOf("AND") == -1 && this.where.IndexOf("OR") == -1; }
		}

		public virtual List<KeyValuePair<string, string>> SortColumns
		{
			get { return null; }
		}


		public static void SQLExport(Business oB, string sFile, bool bCurrentRecord) //R090316
		{
			Business.SQLExport(oB.DataSet, sFile, bCurrentRecord);
		}
		public static void SQLExport(DataSetAdvanced oDs, string sFile, bool bCurrentRecord) //R090316
		{
			if (oDs != null)
			{
				string sColumns = "";
				foreach (DataColumn oCol in oDs.Tables[0].Columns)
				{
					sColumns += oCol.Caption + ",";
				}

				char[] charsToTrim = { ',' };
				sColumns = sColumns.TrimEnd(charsToTrim);

				string sSql = "";

				Business oB = new Business();
				oB.tabelle = oDs.Tables[0].TableName.Replace("dba", "");
				oB.spalten = sColumns;
				oB.tabelle = oB.tabelle.Replace(".", "");
				if (oB.tabelle.EndsWith("V"))
					oB.tabelle = oB.tabelle.Substring(0, oB.tabelle.Length - 1);
				
				if (System.IO.File.Exists(sFile))
					System.IO.File.Delete(sFile);


				Services.WriteLog(sFile, "-- " + oB.tabelle);
				if (bCurrentRecord)
					oB.Where = Services.WhereNummer(oDs.GetInt("NUMMER"));

				oB.Read();

				if (bCurrentRecord)
				{
					oB.PutRecord(oB.DataSet);
					sSql = oB.BuildSaveStmt(SqlAction.Insert);
					Services.WriteLog(sFile, sSql + ";");
				}
				else
				{
					oB.DataSet.GoTop();
					while (!oB.DataSet.EoF)
					{
						oB.Clear();
						oB.PutRecord(oB.DataSet);
						sSql = oB.BuildSaveStmt(SqlAction.Insert);
						Services.WriteLog(sFile, sSql + ";");
						oB.DataSet.Skip();
					}
				}
			}
		}

		#region statische Methoden
		public static string SuchenVolltextWhere(string sSessionId, string sBegriff, System.Collections.Generic.List<string> aSpalten)
		{
			string[] aBegriffe = sBegriff.Split(' ');
			string sWhere = "";

			foreach (string s in aBegriffe)
			{
				sWhere += "(";
				foreach (string s2 in aSpalten)
				{
					if (Services.ToDateTime(s) != ApS.Settings.NullDate)
					{
						sWhere += s2 + " like '%" + ApS.Databases.Firebird.Save.DateTime2SqlDate(Services.ToDateTime(s)) + "%' or ";
					}
					sWhere += s2 + " like '%" + s + "%' or ";
				}
				sWhere = Services.Substring(sWhere, 0, sWhere.Length - 4);

				sWhere += ") and ";
			}
			if (sWhere.Length != 0)
			{
				sWhere = Services.Substring(sWhere, 0, sWhere.Length - 5);
			}
			return sWhere;
		}
		#endregion statische Methoden

		#region Save-Methoden
		//M090407 - Neu
		public override bool Save() //R100301 public
		{
			bool bReturn = base.Save();

			return bReturn;
		}
		//M090407 - Neu
		public override bool Save(SqlAction eSqlAction)
		{
			bool bReturn = base.Save(eSqlAction);

			return bReturn;
		}
		//M090407 - Neu
		public override bool Save(SqlAction eSqlAction, string sWhere)
		{
			bool bReturn = base.Save(eSqlAction, sWhere);

			return bReturn;
		}
		#endregion Save-Methoden

	}
}
