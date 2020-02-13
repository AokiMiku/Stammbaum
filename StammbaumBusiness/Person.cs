namespace Stammbaum
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using ApS.Databases;

	public class Person
	{
		#region Variablen
		private int nummer;
		private string vorname;
		private string famName;
		private DateTime bDay;
		private int generation;
		private int? vater;
		private int? mutter;
		private int? partner;
		private bool lebend;
		private char geschlecht;
		private int alter;
		private Random rand;
		private bool isSchwanger = false;
		private const int dauerSchwangerschaft = 270;
		private DateTime? wirdKindGebaerenAnTag;
		public event EventHandler<Ausgabe> PersonAusgabe;
		public event EventHandler<Changed> PersonChanged;
		private Person meinVater;
		private Person meineMutter;
		private Person meinPartner;
		private List<Person> kinder;
		private List<Person> geschwister;
		#endregion Variablen

		#region Eigenschaften
		public int Nummer
		{
			get { return this.nummer; }
		}
		public string Vorname
		{
			get { return this.vorname; }
		}
		public string Familienname
		{
			get { return this.famName; }
			set { this.famName = value; }
		}
		public DateTime Geburtsdatum
		{
			get { return this.bDay; }
		}
		public int Alter
		{
			get
			{
				DateTime derzeit = StammbaumBusiness.AktuellerTagInSimulation;
				int alter = derzeit.Year - this.bDay.Year;
				if (derzeit.Month < this.bDay.Month || (derzeit.Month == this.bDay.Month && derzeit.Day < this.bDay.Day))
				{
					alter--;
				}
				if (alter != this.alter)
				{
					this.alter = alter;
					this.PersonChanged?.Invoke(this, new Changed(this, Changed.ChangedProperty.Alter, this.alter));
				}
				return this.alter;
			}
		}
		public int Generation
		{
			get { return this.generation; }
		}
		public int? Vater
		{
			get { return this.vater; }
		}
		public int? Mutter
		{
			get { return this.mutter; }
		}
		public int? Partner
		{
			get { return this.partner; }
			set { this.partner = value; }
		}
		public bool Lebend
		{
			get { return this.lebend; }
			set { this.lebend = value; }
		}
		public char Geschlecht
		{
			get { return this.geschlecht; }
		}
		public bool DarfPartner
		{
			get
			{
				return (this.partner == 0 && this.Geburtsdatum.AddYears(16) < StammbaumBusiness.AktuellerTagInSimulation);
			}
		}
		public bool IsSchwanger
		{
			get { return this.isSchwanger; }
			set { this.isSchwanger = value; }
		}
		public DateTime? WirdKindGebaerenAnTag
		{
			get { return this.wirdKindGebaerenAnTag; }
			set { this.wirdKindGebaerenAnTag = value; }
		}
		public Person MeinPartner
		{
			get
			{
				if (this.meinPartner == null && this.partner != null && this.partner != 0)
				{
					var x = StammbaumBusiness.WorkingList.Where(P => P.nummer == this.partner);
					if (x.Count() > 0)
						this.meinPartner = x.First();
					else
					{
						using (PersonenV p = new PersonenV())
						{
							p.Where = "Nummer = " + this.partner;
							p.Read();

							if (p.EoF)
								return null;
							else
							{
								this.meinPartner = new Person(p.Nummer, p.DbVorname, p.DbFamilienname, p.DbGeburtsdatum, p.DbGeneration, p.NummerVater(), p.NummerMutter());
							}
						}
					}
				}

				return this.meinPartner;
			}
			set { this.meinPartner = value; }
		}

		public Person MeinVater
		{
			get
			{
				if (this.meinVater == null && this.vater != null && this.vater != 0)
				{
					var x = StammbaumBusiness.WorkingList.Where(P => P.nummer == this.vater);
					if (x.Count() > 0)
						this.meinVater = x.First();
					else
					{
						using (PersonenV p = new PersonenV())
						{
							p.Where = "Nummer = " + this.vater;
							p.Read();

							if (p.EoF)
								return null;
							else
							{
								this.meinVater = new Person(p.Nummer, p.DbVorname, p.DbFamilienname, p.DbGeburtsdatum, p.DbGeneration, p.NummerVater(), p.NummerMutter());
							}
						}
					}
				}
				return this.meinVater;
			}
			set { this.meinVater = value; }
		}

		public Person MeineMutter
		{
			get
			{
				if (this.meineMutter == null && this.mutter != null && this.mutter != 0)
				{
					var x = StammbaumBusiness.WorkingList.Where(P => P.nummer == this.mutter);
					if (x.Count() > 0)
						this.meineMutter = x.First();
					else
					{
						using (PersonenV p = new PersonenV())
						{
							p.Where = "Nummer = " + this.mutter;
							p.Read();

							if (p.EoF)
								return null;
							else
							{
								this.meineMutter = new Person(p.Nummer, p.DbVorname, p.DbFamilienname, p.DbGeburtsdatum, p.DbGeneration, p.NummerVater(), p.NummerMutter());
							}
						}
					}
				}
				return this.meineMutter;
			}
			set { this.meineMutter = value; }
		}
		public List<Person> Kinder
		{
			get
			{
				this.kinder.Clear();

				var childs = StammbaumBusiness.WorkingList.Where(P => P.vater == this.nummer || P.mutter == this.nummer);
				foreach (Person z in childs)
				{
					this.kinder.Add(z);
				}

				return this.kinder;
			}
		}

		public List<Person> Geschwister
		{
			get
			{
				this.geschwister.Clear();

				var siblings = StammbaumBusiness.WorkingList.Where(P => ((this.vater != 0 && this.mutter != 0 && (P.vater == this.vater || P.mutter == this.mutter)) || (P.Generation == 0 && this.Familienname == P.Familienname)) && this.Nummer != P.Nummer);
				if (siblings.Count() > 0)
				{
					//siblings = siblings.First().Kinder.Where(S => S.nummer != this.nummer).ToList();
					foreach (Person z in siblings)
					{
						this.geschwister.Add(z);
					}
				}
				return this.geschwister;
			}
		}
		#endregion Eigenschaften

		#region Konstruktoren
		/// <summary>
		/// constructs an object with all information provided
		/// </summary>
		/// <param name="nr">primary key of the person</param>
		/// <param name="vName">first name of the person</param>
		/// <param name="nName">last name of the person</param>
		/// <param name="gebDat">date of birth of the person</param>
		/// <param name="gen">number of generation</param>
		/// <param name="vat">primary key of the father from the person</param>
		/// <param name="mut">primary key of the mother from the person</param>
		/// <param name="part">primary key of the partner from the person</param>
		/// <param name="leb">true if the person is still alive</param>
		public Person(int nr, string vName, string nName, DateTime gebDat, int gen, int? vat, int? mut, int? part, bool leb)
		{
			this.nummer = nr;
			this.vorname = vName;
			this.famName = nName;
			this.bDay = gebDat;
			this.generation = gen;
			this.vater = vat;
			this.mutter = mut;
			this.partner = part;
			this.lebend = leb;

			this.Initialize();
		}

		/// <summary>
		/// constructs an object with almost all information provided; person is alive
		/// </summary>
		/// <param name="nr">primary key of the person</param>
		/// <param name="vName">first name of the person</param>
		/// <param name="nName">last name of the person</param>
		/// <param name="gebDat">date of birth of the person</param>
		/// <param name="gen">number of generation</param>
		/// <param name="vat">primary key of the father from the person</param>
		/// <param name="mut">primary key of the mother from the person</param>
		/// <param name="part">primary key of the partner from the person</param>
		public Person(int nr, string vName, string nName, DateTime gebDat, int gen, int? vat, int? mut, int? part)
		{
			this.nummer = nr;
			this.vorname = vName;
			this.famName = nName;
			this.bDay = gebDat;
			this.generation = gen;
			this.vater = vat;
			this.mutter = mut;
			this.partner = part;
			this.lebend = true;

			this.Initialize();
		}

		/// <summary>
		/// constructs an object with almost all information provided; person is alive and has no partner
		/// </summary>
		/// <param name="nr">primary key of the person</param>
		/// <param name="vName">first name of the person</param>
		/// <param name="nName">last name of the person</param>
		/// <param name="gebDat">date of birth of the person</param>
		/// <param name="gen">number of generation</param>
		/// <param name="vat">primary key of the father from the person</param>
		/// <param name="mut">primary key of the mother from the person</param>
		public Person(int nr, string vName, string nName, DateTime gebDat, int gen, int? vat, int? mut)
		{
			this.nummer = nr;
			this.vorname = vName;
			this.famName = nName;
			this.bDay = gebDat;
			this.generation = gen;
			this.vater = vat;
			this.mutter = mut;
			this.partner = 0;
			this.lebend = true;

			this.Initialize();
		}
		#endregion Konstruktoren

		#region Methoden

		private void Initialize()
		{
			this.rand = new Random();
			using (Vornamen vNamen = new Vornamen())
			{
				vNamen.Where = "Vorname = '" + this.vorname + "'";
				vNamen.Read();
				if (vNamen.DbGeschlecht == 'M')
					this.geschlecht = 'M';
				else if (vNamen.DbGeschlecht == 'W')
					this.geschlecht = 'W';
				this.kinder = new List<Person>();
				this.geschwister = new List<Person>();
			}
		}
		/// <summary>
		/// lets the person search for a partner
		/// </summary>
		public void SuchePartner()
		{
			//search for available partners
			List<Person> potentiellePartner = new List<Person>();
			var pP = StammbaumBusiness.WorkingList.Where(P => P.DarfPartner && P.Geschlecht != this.Geschlecht && !this.IsVerwandter(P) && !P.IsVerwandter(this));
			if (pP.Count() > 0)
				potentiellePartner.AddRange(pP);
			//foreach (Person p in StammbaumBusiness.WorkingList)
			//{
			//	if (p.DarfPartner && p.Geschlecht != this.Geschlecht && !this.IsVerwandter(p) && !p.IsVerwandter(this))
			//	{
			//		potentiellePartner.Add(p);
			//	}
			//}
			if (potentiellePartner.Count == 0)
				return;

			//random selection of a partner
			this.meinPartner = potentiellePartner[rand.Next(0, potentiellePartner.Count)];
			StammbaumBusiness.ZuEntfernende.Enqueue(this.meinPartner);
			StammbaumBusiness.ZuEntfernende.Enqueue(this);

			//setting of properties to save the new relationship
			this.meinPartner.partner = this.nummer;
			this.partner = this.meinPartner.nummer;
			if (this.geschlecht == 'M')
				this.meinPartner.famName = this.famName;
			else
				this.famName = this.meinPartner.famName;
			this.meinPartner.meinPartner = this;
			StammbaumBusiness.NeuHinzuzufuegende.Enqueue(this.meinPartner);
			StammbaumBusiness.NeuHinzuzufuegende.Enqueue(this);

			//save the new relationship into the database
			using (Familien fam = new Familien())
			{
				fam.Where = "Familienname = '" + this.famName + "'";
				fam.Read();

				//first partner
				using (Personen SaveRelationship = new Personen())
				{
					SaveRelationship.Where = "Nummer = " + this.nummer;
					SaveRelationship.Read();

					SaveRelationship.DbPartner_Nr = this.partner;
					SaveRelationship.DbFamilie_Nr = fam.Nummer;
					SaveRelationship.BuildSaveStmt(SqlAction.Update);

					//second partner
					SaveRelationship.Where = "Nummer = " + this.meinPartner.nummer;
					SaveRelationship.Read();

					SaveRelationship.DbPartner_Nr = this.meinPartner.partner;
					SaveRelationship.DbFamilie_Nr = fam.Nummer;

					SaveRelationship.BuildSaveStmt(SqlAction.Update);

					SaveRelationship.SaveStmtsOnly();
				}
			}

			//share the relationship with others
			this.FireAusgabe(this.vorname + " " + this.famName + " und " + this.meinPartner.vorname + " " + this.meinPartner.famName + " sind jetzt ein Paar!");
			this.PersonChanged?.Invoke(this, new Changed(this, Changed.ChangedProperty.Partner, this.partner));
		}

		/// <summary>
		/// lets the person create a child with his/her partner
		/// </summary>
		public void ZeugeKind()
		{
			//if the partner is female this method will be called again for the partner-object
			if (this.geschlecht == 'M')
				this.meinPartner.ZeugeKind();
			else if (this.geschlecht == 'W' && !this.isSchwanger)
			{
				this.isSchwanger = true;
				this.wirdKindGebaerenAnTag = StammbaumBusiness.AktuellerTagInSimulation.AddDays(dauerSchwangerschaft + rand.Next(-25, 25));

				//save the day of birth in database
				using (Personen temp = new Personen())
				{
					temp.Where = "Nummer = " + this.nummer;
					temp.Read();
					temp.DbBekommtKindAnTag = this.wirdKindGebaerenAnTag;
					temp.BuildSaveStmt(SqlAction.Update);
					temp.SaveStmtsOnly();
				}
				//share the news with others
				this.FireAusgabe(this.vorname + " " + this.famName + " ist schwanger!");
			}
		}

		/// <summary>
		/// give birth to the child
		/// </summary>
		public void GebaereKind()
		{
			//select if the child would be male or female
			bool isMale = false;
			int rnd = rand.Next(0, 2);
			if (rnd == 0)
				isMale = true;

			//select a random first name based on the gender
			using (Vornamen vNamen = new Vornamen())
			{
				vNamen.Where = "Geschlecht = '" + (isMale ? "M" : "W") + "'";
				vNamen.Read();
				vNamen.Skip(rand.Next(0, vNamen.RecordCount));
				var z = this.kinder.Where(x => x.vorname == vNamen.DbVorname);
				while (z.Count() > 0)
				{
					vNamen.GoTop();
					vNamen.Skip(rand.Next(0, vNamen.RecordCount));
					z = this.kinder.Where(x => x.vorname == vNamen.DbVorname);
				}
				string vNameKind = vNamen.DbVorname;

				//sets the birthday
				DateTime kindGebDat = StammbaumBusiness.AktuellerTagInSimulation;

				//add the new child to the List
				Person kind = new Person(2541, vNameKind, this.famName, kindGebDat, (this.generation > this.MeinPartner.generation ? this.generation : this.MeinPartner.generation) + 1, this.partner, this.nummer);
				kind.meineMutter = this;
				kind.meinVater = this.meinPartner;
				//this.meineKinder.Add(kind);
				//this.meinPartner.meineKinder.Add(kind);
				StammbaumBusiness.NeuHinzuzufuegende?.Enqueue(kind);

				//save the new child in the database
				using (Familien famTable = new Familien())
				{
					famTable.Where = "Familienname = '" + this.famName + "'";
					famTable.Read();
					using (Personen personenTable = new Personen())
					{
						personenTable.DbVorname_Nr = vNamen.Nummer;
						personenTable.DbFamilie_Nr = famTable.Nummer;
						personenTable.DbGeburtsdatum = kind.Geburtsdatum;
						personenTable.DbGeneration = kind.Generation;
						personenTable.DbVater_Nr = kind.vater;
						personenTable.DbMutter_Nr = kind.mutter;
						personenTable.DbPartner_Nr = null;
						personenTable.DbLebend = true;
						personenTable.BuildSaveStmt(SqlAction.Insert);
						personenTable.SaveStmtsOnly();

						//asks the database for the correct number of the new child
						personenTable.Where = "vorname_Nr = " + vNamen.Nummer + " and familie_nr = " + famTable.Nummer + " and geburtsdatum = '" + kindGebDat.ToShortDateString() + "'";
						personenTable.Read();
						kind.nummer = personenTable.Nummer;

						//deletes the day of birth in database because it is over
						Personen temp = new Personen();
						temp.Where = "Nummer = " + this.nummer;
						temp.Read();
						temp.DbBekommtKindAnTag = null;
						temp.BuildSaveStmt(SqlAction.Update);
						temp.SaveStmtsOnly();

						//share the news with others
						this.FireAusgabe("Ein Kind namens " + vNameKind + " " + this.Familienname + " wurde geboren!");
					}
				}
			}

			//at last: reset the IsSchwanger-Property
			this.IsSchwanger = false;
			this.PersonChanged?.Invoke(this, new Changed(this, Changed.ChangedProperty.Kinder, this.kinder));
			foreach (Person kind in this.Kinder)
			{
				kind.PersonChanged?.Invoke(this, new Changed(kind, Changed.ChangedProperty.Geschwister, kind.geschwister));
			}
		}

		public void Sterben()
		{
			Personen person = new Personen();
			person.Where = "Nummer = " + this.nummer;
			person.Read();
			person.DbLebend = false;
			person.Save(SqlAction.Update);
			this.Lebend = false;
			StammbaumBusiness.ZuEntfernende.Enqueue(this);
			FireAusgabe(this.Vorname + " " + this.Familienname + " ist an Altersschwäche gestorben!");
			this.PersonChanged?.Invoke(this, new Changed(this, Changed.ChangedProperty.Lebend, this.Lebend));
		}

		/// <summary>
		/// checks if the given person is a family-member, true if so
		/// </summary>
		/// <param name="prüfe">person that should be checked</param>
		/// <returns><see cref="bool"/></returns>
		public bool IsVerwandter(Person prüfe)
		{
			if (this.Generation == prüfe.Generation - 4)
				return false;
			if ((this.vater == 0 || this.mutter == 0 || prüfe.vater == 0 || prüfe.mutter == 0) && this.geschwister.Count == 0)
				return false;
			if (prüfe == this.MeinVater || prüfe == this.MeineMutter)
				return true;
			if (this.Geschwister.Contains(prüfe))
				return true;
			//foreach (Person sibling in this.Geschwister)
			for (int i=0;i<this.Geschwister.Count;i++)
			{
				if (this.Geschwister[i].Kinder.Contains(prüfe))
					return true;
				//foreach (Person siblingChild in sibling.Kinder)
				//{
				//	if (siblingChild.IsVerwandter(prüfe))
				//		return true;
				//}
			}
			if (this.MeinVater?.IsVerwandter(prüfe) == true)
				return true;
			if (this.MeineMutter?.IsVerwandter(prüfe) == true)
				return true;
			
			return false;
		}

		private void FireAusgabe(string message)
		{
			this.PersonAusgabe?.Invoke(this, new Ausgabe(this, message));
		}
		#endregion Methoden
	}
}