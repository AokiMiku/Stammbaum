namespace Stammbaum
{
	using System;
	using System.Linq;
	using ApS;
	using ApS.Databases;
	using System.Collections.Generic;
	using System.Collections;
    using System.Windows.Threading;
	using FirebirdSql.Data.FirebirdClient;

	public class StammbaumBusiness
	{
		#region Instanzvariablen
		private Random rand;
		private Queue<KeyValuePair<string, object>[]> outputQueue;
		private string where = "";
		private static List<Person> workingList;
		private static Queue<Person> neuHinzuzufuegende;
		private static Queue<Person> zuEntfernende;
		public event EventHandler<Ausgabe> Ausgabe;
		public event EventHandler<EventArgs> LoadAllFinished;
		public event EventHandler<EventArgs> ListChanged;
		private static DateTime aktuellerTag;
		public bool Initialized = false;
		#endregion Instanzvariablen

		#region Properties
		/// <summary>
		/// output some data
		/// </summary>
		public Queue<KeyValuePair<string, object>[]> OutputQueue
		{
			get
			{
				if (outputQueue.Count == 0)
					AusgabeErzeugen();
				return outputQueue;
			}
		}
		/// <summary>
		/// Gets or sets the where-clause for which data should be displayed
		/// </summary>
		public string Where
		{
			get { return where; }
			set { where = value; }
		}
		/// <summary>
		/// Gets the list where all persons are saved and waiting for their editing by the simulation
		/// </summary>
		public static List<Person> WorkingList
		{
			get { return workingList; }
		}
		/// <summary>
		/// Gets the queue where all persons are queued they have to be added to the WorkingList
		/// </summary>
		public static Queue<Person> NeuHinzuzufuegende
		{
			get { return neuHinzuzufuegende; }
		}
		/// <summary>
		/// Gets the queue where all persons are queued they have to be removed from the WorkingList
		/// </summary>
		public static Queue<Person> ZuEntfernende
		{
			get { return zuEntfernende; }
		}
		/// <summary>
		/// Gets the count of days the simulation runs
		/// </summary>
		public static DateTime AktuellerTagInSimulation
		{
			get { return aktuellerTag; }
		}
		/// <summary>
		/// Gets the object that holds the settings-table
		/// </summary>
		public Einstellungen Settings { get; set; }
		#endregion Properties

		#region Konstruktoren
		/// <summary>
		/// constructor
		/// </summary>
		public StammbaumBusiness()
		{
			//ApS.Settings.ConnectionString = "ServerType=0;Database=" + Services.GetAppDir() + @"\Datenbank\FamilienDb.fdb;User=sysdba;Password=masterkey;Port=3058";
			FbConnectionStringBuilder connectionString = new FbConnectionStringBuilder();
			connectionString.ServerType = FbServerType.Embedded;
			connectionString.Database = Services.GetAppDir() + @"\Datenbank\FamilienDb.fdb";
			connectionString.UserID = "sysdba";
			connectionString.Password = " ";
			ApS.Databases.Settings.ConnectionString = connectionString.ToString();
			
            outputQueue = new Queue<KeyValuePair<string, object>[]>();
			workingList = new List<Person>();
			neuHinzuzufuegende = new Queue<Person>();
			zuEntfernende = new Queue<Person>();

			rand = new Random();

			Settings = new Einstellungen();
			this.GetAktuellerTagFromDb();
		}
		#endregion Konstruktoren

		#region Methoden
		public void LoadAll()
		{
			this.LoadPersons();
			foreach (Person p in StammbaumBusiness.WorkingList)
			{
				this.LoadPartner(p);
				this.LoadChildren(p);
			}
			this.LoadAllFinished?.Invoke(this, new EventArgs());
			this.Initialized = true;
		}

		/// <summary>
		/// create an object for each person in the table and store each object in a list
		/// </summary>
		public void LoadPersons()
		{
			using (Personen personen = new Personen())
			{
				personen.Where = "Nummer is not null AND Lebend = 1";
				personen.OrderBy = "nummer asc";
				personen.Read();

				//checking if there are some persons
				if(personen.EoF)
				{
					CreateFirst();
					LoadPersons();
					return;
				}

				using (Familien familien = new Familien())
				{
					using (Vornamen vornamen = new Vornamen())
					{
						while (!personen.EoF)
						{
							vornamen.Where = "Nummer = " + personen.DbVorname_Nr;
							vornamen.Read();

							familien.Where = "Nummer = " + personen.DbFamilie_Nr;
							familien.Read();

							//load all persons from database into a cache
							Person person = new Person(personen.Nummer, vornamen.DbVorname, familien.DbFamilienname, personen.DbGeburtsdatum, personen.DbGeneration ?? 0, personen.DbVater_Nr, personen.DbMutter_Nr, personen.DbPartner_Nr, personen.DbLebend);
							if (person.Vater != null && person.Vater != 0)
							{
								var v = WorkingList.Where(x => x.Nummer == person.Vater);
								if (v.Count() > 0)
									person.MeinVater = v.First();
							}
							if (person.Mutter != null && person.Mutter != 0)
							{
								var m = WorkingList.Where(x => x.Nummer == person.Mutter);
								if (m.Count() > 0)
									person.MeineMutter = m.First();
							}

							if (personen.DbBekommtKindAnTag != null && personen.DbBekommtKindAnTag != new DateTime(1, 1, 1))
							{
								person.IsSchwanger = true;
								person.WirdKindGebaerenAnTag = personen.DbBekommtKindAnTag;
							}
							person.PersonAusgabe += Person_PersonAusgabe;
							WorkingList.Add(person);

							FireAusgabe("loaded '" + person.Vorname + " " + person.Familienname + "'");
							personen.Skip();
						}
					}
				}
			}
		}

		public void LoadPartner(Person pers)
		{
			if (pers.Partner != 0)
			{
				//personen.Where = "Nummer = " + pers.Partner;
				//personen.Read();
				//familien.Where = "Nummer = " + personen.DbFamilie_Nr;
				//familien.Read();
				//vornamen.Where = "Nummer = " + personen.DbVorname_Nr;
				//vornamen.Read();
				var p = workingList.Where(P => P.Nummer == pers.Partner);
				if (p.Count() > 0)
					pers.Partner = p.First().Nummer;

				FireAusgabe("Partner für " + pers.Vorname + " " + pers.Familienname + " gesetzt.");
			}
		}

		public void LoadChildren(Person pers)
		{
			//var kinder = WorkingList.Where(x => x.Vater == pers.Nummer || x.Mutter == pers.Nummer);
			//foreach (Person p in kinder)
			//{
			//	pers.Kinder.Add(p);

			//	FireAusgabe(p.Vorname + " " + p.Familienname + " als Kind von " + pers.Vorname + " " + pers.Familienname + " gesetzt.");

			//	if (p.Geschwister.Count == 0)
			//	{
			//		p.Geschwister.AddRange(kinder);
			//		p.Geschwister.Remove(p);

			//		if (p.Geschwister.Count > 0)
			//			FireAusgabe("Geschwister von " + p.Vorname + " " + p.Familienname + " gesetzt.");
			//	}
			//}
			//this.Initialized = true;
		}

		private void CreateFirst()
		{
			using (Vornamen vNamen = new Vornamen())
			{
				vNamen.Where = "Nummer is not null";
				vNamen.Read();
				using (Familien fNamen = new Familien())
				{
					fNamen.Where = "Nummer is not null";
					fNamen.Read();
					using (Personen personen = new Personen())
					{
						for (int i = 1; i <= 40; i++)
						{
							personen.Where = "Nummer = " + i;
							personen.Read();

							vNamen.GoTop();
							fNamen.GoTop();

							if (i == 1)
							{
								personen.DbVorname_Nr = 64;
								personen.DbFamilie_Nr = 4;
								personen.DbGeburtsdatum = new DateTime(1993, 12, 30);
							}
							else
							{
								vNamen.Skip(rand.Next(vNamen.RecordCount));
								fNamen.Skip(rand.Next(fNamen.RecordCount));
								DateTime gebDat = aktuellerTag.AddDays(-(4380 + rand.Next(-730, 730)));

								personen.DbVorname_Nr = vNamen.Nummer;
								personen.DbFamilie_Nr = fNamen.Nummer;
								personen.DbGeburtsdatum = gebDat;
							}
							personen.DbGeneration = 0;
							personen.DbLebend = true;

							personen.BuildSaveStmt(SqlAction.Insert);
						}
						ArrayList stmts = personen.FbSave.Statements;
						personen.SaveStmtsOnly();
					}
				}
			}
		}

		private void AusgabeErzeugen()
		{
			//check the definition of a where-clause; if empty create some standard
			if (where.Length == 0)
			{
				where = "Nummer is not null";
			}

			try
			{
				using (PersonenV personenV = new PersonenV())
				{
					personenV.Where = where;
					personenV.Read();
					
					const int columnCount = 7;
					while (!personenV.EoF)
					{
						KeyValuePair<string, object>[] pair = new KeyValuePair<string, object>[columnCount]
						{
							new KeyValuePair<string, object>("Nummer", personenV.Nummer),
							new KeyValuePair<string, object>("Vorname", personenV.DbVorname),
							new KeyValuePair<string, object>("Familienname", personenV.DbFamilienname),
							new KeyValuePair<string, object>("Geburtsdatum", personenV.DbGeburtsdatum.ToShortDateString()),
							new KeyValuePair<string, object>("Mutter", personenV.DbMutter),
							new KeyValuePair<string, object>("Vater", personenV.DbVater),
							new KeyValuePair<string, object>("Partner", personenV.DbPartner)
						};

						outputQueue.Enqueue(pair);
						personenV.Skip();
					}
				}
			}
			catch (Exception ex)
			{
				FireAusgabe(ex.Message);
			}
		}


		/// <summary>
		/// what the simulation every day does
		/// </summary>
		public void Simulation()
		{
			if (!Initialized)
			{
				FireAusgabe("Initialisierung noch nicht abgeschlossen! Zuerst müssen alle Personen geladen werden.");
				return;
			}

			try
			{
				this.SaveAktuellerTagToDb();
				foreach (Person person in workingList)
				{
					//check if person is still alive; if not you have nothing to do here
					if (!person.Lebend)
						continue;

					//gets the age of the person and his/her partner when a partner exists
					//int alter = startDatum.AddDays(vergangeneTage).Year - person.Geburtsdatum.Year;
					//int alterPartner = 0;
					int alterDurchschnitt = 0;
					if (person.MeinPartner != null)
					{
						//person.MeinPartner.Alter = startDatum.AddDays(vergangeneTage).Year - person.MeinPartner.Geburtsdatum.Year;
						alterDurchschnitt = (person.Alter + person.MeinPartner.Alter) / 2;
					}

					//after 9 months carrying that little person in you, you feel good to see your baby
					if (person.IsSchwanger && person.WirdKindGebaerenAnTag == aktuellerTag)
						person.GebaereKind();

					//celebrate your birthday by sharing it with others
					if (person.Geburtsdatum.Month == aktuellerTag.Month && person.Geburtsdatum.Day == aktuellerTag.Day && person.Alter == 16)
						FireAusgabe(person.Vorname + " " + person.Familienname + " hat Geburtstag und wird " + person.Alter + " Jahre alt.");

					//chance of max 19.4% to get the girlfriend pregnant (chance depends on the summed up age of both partners and on the count of children they already have and decreases when the people gets older and get more children)
					if (person.MeinPartner != null && !person.IsSchwanger && !person.MeinPartner.IsSchwanger)
					{
						double x = 0;
						int iNext = rand.Next();
						if (alterDurchschnitt < 32)
						{
							if (person.Partner != 0 && iNext % 10000 < ((200.0 / alterDurchschnitt) / (Math.Pow(person.Kinder.Count + 1, 2))))
								person.ZeugeKind();
						}
						else if (alterDurchschnitt < 46)
						{
							if (person.Partner != 0 && iNext % 10000 < ((350.0 / alterDurchschnitt) / (Math.Pow(person.Kinder.Count + 1, 2))))
								person.ZeugeKind();
						}
						else if (person.Partner != 0 && iNext % 10000 < (x = (55.0 / alterDurchschnitt) / (Math.Pow(person.Kinder.Count + 1, 3))) && x > 0)
							person.ZeugeKind();
					}

					//chance of 0.1% to find a partner
					if (person.DarfPartner && rand.Next() % 1000 < 1)
						person.SuchePartner();


					//chance of min 0.0001% to kill the person by old age when they reached 80 years (increases the older the person is)
					if (person.Alter > 80)
					{
						int iNext = rand.Next();
						if (iNext % 10000 < 0.1 * person.Alter)
						{
							person.Sterben();
						}
					}

					//zuEntfernende.Enqueue(person);
					//neuHinzuzufuegende.Enqueue(person);
				}
			}
			finally
			{
				if (ZuEntfernende.Count > 0 || NeuHinzuzufuegende.Count > 0)
				{
					while (ZuEntfernende.Count > 0)
					{
						Person entf = ZuEntfernende.Dequeue();
						entf.PersonAusgabe -= Person_PersonAusgabe;
						WorkingList.Remove(entf);
					}
					while (NeuHinzuzufuegende.Count > 0)
					{
						Person add = NeuHinzuzufuegende.Dequeue();
						add.PersonAusgabe += Person_PersonAusgabe;
						WorkingList.Add(add);
					}
					WorkingList.Sort(SortTheList);
					this.ListChanged?.Invoke(this, new EventArgs());
				}
				aktuellerTag = aktuellerTag.AddDays(1);
			}
		}

		private int SortTheList(Person x, Person Y)
		{
			if (x == null)
			{
				if (Y == null)
					return 0;
				else
					return - 1;
			}
			else
			{
				if (Y == null)
					return 1;
				else
				{
					if (x.Nummer < Y.Nummer)
						return -1;
					else if (x.Nummer > Y.Nummer)
						return 1;
					else
						return 0;
				}
			}
		}

		private DateTime GetAktuellerTagFromDb()
		{
			return aktuellerTag = this.Settings.GetSetting("aktuellerTag").ToDateTime();
		}

		private void SaveAktuellerTagToDb()
		{
			try
			{
				string s = this.Settings.SetSetting("aktuellerTag", AktuellerTagInSimulation.ToShortDateString());
				if (!string.IsNullOrEmpty(s))
					this.Ausgabe?.Invoke(this, new Ausgabe(this, s));
			}
			catch
			{

			}
		}
		#endregion Methoden

		#region EventHandler

		private void FireAusgabe(string message)
		{
			if (Ausgabe != null)
				Ausgabe.Invoke(this, new Ausgabe(this, aktuellerTag.ToShortDateString() + ": " + message));
		}
		private void Person_PersonAusgabe(object sender, Ausgabe e)
		{
			FireAusgabe(e.Message);
		}
		#endregion EventHandler
	}
}