namespace Simulation
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
	using Stammbaum;
	using ApS;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer gameTicks;
		DispatcherTimer refreshTimer;
		StammbaumBusiness business;
		object selectedItem;
		bool refreshInThread = false;
		bool buildDataInThread = false;
		event EventHandler<EventArgs> BuildDataFinished;
		private Person currentPerson = null;

		public MainWindow()
		{

			InitializeComponent();
			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;


			this.business = new StammbaumBusiness();
			string WidthStr = this.business.Settings.GetSetting("Fensterweite");
			if (WidthStr.Length > 0)
				this.Width = int.Parse(WidthStr);
			string HeightStr = this.business.Settings.GetSetting("Fensterhöhe");
			if (HeightStr.Length > 0)
				this.Height = int.Parse(HeightStr);

			this.gameTicks = new DispatcherTimer();
			this.gameTicks.Tick += GameTicks_Tick;
			this.gameTicks.Interval = new TimeSpan(0, 0, 0, 0, 10);

			this.refreshTimer = new DispatcherTimer();
			//this.refreshTimer.Tick += RefreshTimer_Tick;
			this.refreshTimer.Interval = new TimeSpan(0, 0, 0, 10);

			this.business.Ausgabe += Business_Ausgabe;
			this.business.ListChanged += Business_ListChanged;
			this.BuildDataFinished += MainWindow_BuildDataFinished;
		}

		private void Business_ListChanged(object sender, EventArgs e)
		{
			if (!refreshInThread)
				Threads.AddThread("RefreshPersonen", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.RefreshPersonen())), true);
		}

		private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{

		}

		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			if (!refreshInThread)
				Threads.AddThread("RefreshPersonen", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.RefreshPersonen())), true);
		}

		private void MainWindow_BuildDataFinished(object sender, EventArgs e)
		{
			Threads.RemoveThread("BuildData", false);
			buildDataInThread = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Threads.AddThread("LoadAll", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.business.LoadAll())), true);
			this.business.LoadAllFinished += Business_LoadAllFinished;
		}

		private void Business_LoadAllFinished(object sender, EventArgs e)
		{
			Threads.RemoveThread("LoadAll", false);
			if (!refreshInThread)
				Threads.AddThread("RefreshPersonen", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.RefreshPersonen())), true);
		}

		private void Business_Ausgabe(object sender, Ausgabe e)
		{
			this.Dispatcher.Invoke(() => this.eventLog.Items.Insert(0, e.Message));

			while (this.eventLog.Items.Count > 50)
			{
				this.Dispatcher.Invoke(() => this.eventLog.Items.RemoveAt(this.eventLog.Items.Count - 1));
			}
		}

		private void GameTicks_Tick(object sender, EventArgs e)
		{
			this.gameTicks.Stop();
			this.business.Simulation();
			this.lblDatum.Content = "Datum:" + Environment.NewLine + StammbaumBusiness.AktuellerTagInSimulation.ToShortDateString();
			this.business.Settings.SetSetting("Fensterweite", this.Width.ToString());
			this.business.Settings.SetSetting("Fensterhöhe", this.Height.ToString());

			this.gameTicks.Start();
		}

		private void RefreshPersonen()
		{
			refreshInThread = true;
			object selected = null;
			try
			{
				ListView oView = null;
				this.Dispatcher.Invoke(() => oView = new ListView());
				for (int i = 0; i < StammbaumBusiness.WorkingList.Count; i++)
				{
					this.Dispatcher.Invoke(() => oView.Items.Add(StammbaumBusiness.WorkingList[i].Nummer.ToString().PadLeft(4, '0') + " " + StammbaumBusiness.WorkingList[i].Vorname + " " + StammbaumBusiness.WorkingList[i].Familienname));
				}


				this.Dispatcher.Invoke(() => selected = this.personen.SelectedItem);
				this.Dispatcher.Invoke(() => this.personen.ItemsSource = oView.Items);
				this.Dispatcher.Invoke(() => this.personen.SelectedItem = selected);
			}
			catch (Exception ex)
			{

			}
			Threads.RemoveThread("RefreshPersonen", false);
			refreshInThread = false;
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{

		}

		private void personen_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (this.personen.SelectedItem == null)
				return;

			if (!buildDataInThread)
			{
				Threads.AddThread("BuildData", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.BuildData())), true);
				buildDataInThread = true;
			}

		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.gameTicks.IsEnabled)
				this.btnStartStop_Click(this, new RoutedEventArgs());
			this.business.Settings.SetSetting("Fensterweite", this.Width.ToString());
			this.business.Settings.SetSetting("Fensterhöhe", this.Height.ToString());
		}

		private void lblPartner_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (lblPartner.Content.ToString().Length > 1)
				foreach (var item in this.personen.Items)
				{
					if (item.ToString().Substring(item.ToString().IndexOf(' ') + 1) == lblPartner.Content.ToString())
					{
						this.personen.SelectedItem = item;
						break;
					}
				}
			this.personen_MouseUp(this.lblPartner, e);
		}

		private void lblMutter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (lblMutter.Content.ToString().Length > 1)
				foreach (var item in this.personen.Items)
				{
					if (item.ToString().Substring(item.ToString().IndexOf(' ') + 1) == lblMutter.Content.ToString())
					{
						this.personen.SelectedItem = item;
						break;
					}
				}
			this.personen_MouseUp(this.lblMutter, e);
		}

		private void lblVater_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (lblVater.Content.ToString().Length > 1)
				foreach (var item in this.personen.Items)
				{
					if (item.ToString().Substring(item.ToString().IndexOf(' ') + 1) == lblVater.Content.ToString())
					{
						this.personen.SelectedItem = item;
						break;
					}
				}
			this.personen_MouseUp(this.lblVater, e);
		}

		private void btnStartStop_Click(object sender, RoutedEventArgs e)
		{
			if (this.business.Initialized)
			{
				if (gameTicks.IsEnabled)
				{
					this.btnStartStop.Content = "Start";
					gameTicks.Stop();
					refreshTimer.Stop();
				}
				else
				{
					this.btnStartStop.Content = "Stop";
					gameTicks.Start();
					refreshTimer.Start();
				}
			}
		}

		private void cmbKinder_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.cmbKinder.Items.Count > 0)
			{
				foreach (object item in this.personen.Items)
				{
					if (this.cmbKinder.SelectedItem.Equals(item))
					{
						this.personen.SelectedItem = item;
						if (this.selectedItem != null && this.selectedItem != this.personen.SelectedItem)
							this.BuildData();
						break;
					}
				}
			}
		}

		private void BuildData(bool cmbOpen = false)
		{
			this.Dispatcher.Invoke(() => this.selectedItem = this.personen.SelectedItem);
			if (this.selectedItem == null)
			{
				this.BuildDataFinished?.Invoke(this, new EventArgs());
				return;
			}

			string selectedPerson = "";
			this.Dispatcher.Invoke(() => selectedPerson = this.selectedItem.ToString());

			var p = StammbaumBusiness.WorkingList.Where(P => P.Nummer == Int32.Parse(selectedPerson.Substring(0, selectedPerson.IndexOf(' '))));
			if (p.Count() == 0)
			{
				this.BuildDataFinished?.Invoke(this, new EventArgs());
				return;
			}

			if (currentPerson != null)
			{
				currentPerson.PersonChanged -= CurrentPerson_PersonChanged;
			}
			currentPerson = p.First();
			currentPerson.PersonChanged += CurrentPerson_PersonChanged;
			if (currentPerson.Nummer == Int32.Parse(selectedPerson.Substring(0, selectedPerson.IndexOf(' '))))
			{
				PersonenV p2 = new PersonenV();
				p2.Where = "Nummer = " + currentPerson.Nummer;
				p2.Read();

				this.Dispatcher.Invoke(() => this.lblNummer.Content = currentPerson.Nummer);
				this.Dispatcher.Invoke(() => this.lblVorname.Content = currentPerson.Vorname);
				this.Dispatcher.Invoke(() => this.lblFamName.Content = currentPerson.Familienname);
				this.Dispatcher.Invoke(() => this.lblGeschlecht.Content = currentPerson.Geschlecht);
				this.Dispatcher.Invoke(() => this.lblGebDat.Content = currentPerson.Geburtsdatum.ToShortDateString());
				this.Dispatcher.Invoke(() => this.lblAlter.Content = "(" + currentPerson.Alter + ")");
				this.Dispatcher.Invoke(() => this.lblGeneration.Content = currentPerson.Generation);
				this.Dispatcher.Invoke(() => this.lblVater.Content = p2.DbVater);
				this.Dispatcher.Invoke(() => this.lblMutter.Content = p2.DbMutter);
				this.Dispatcher.Invoke(() => this.lblPartner.Content = p2.DbPartner);
				this.Dispatcher.Invoke(() => this.lblLebend.Content = currentPerson.Lebend ? "Ja" : "Nein");
				this.Dispatcher.Invoke(() => this.lblKinder.Content = currentPerson.Kinder.Count);
				if (!cmbOpen)
				{
					this.Dispatcher.Invoke(() => this.cmbKinder.Items.Clear());
					for (int i = 0; i < currentPerson.Kinder.Count; i++)
					{
						this.Dispatcher.Invoke(() => this.cmbKinder.Items.Add(currentPerson.Kinder[i].Nummer.ToString().PadLeft(4, '0') + " " + currentPerson.Kinder[i].Vorname + " " + currentPerson.Kinder[i].Familienname));
					}

					for (int j = 0; j < this.cmbKinder.Items.Count; j++)
					{
						for (int i = this.cmbKinder.Items.Count - 1; i > 0; i--)
						{
							string tmp = this.cmbKinder.Items[i].ToString();
							int index1 = int.Parse(tmp.Substring(0, tmp.IndexOf(' ')));
							int index2 = int.Parse(this.cmbKinder.Items[i - 1].ToString().Substring(0, tmp.IndexOf(' ')));
							while (index1 < index2)
							{
								this.Dispatcher.Invoke(() => this.cmbKinder.Items[i] = this.cmbKinder.Items[i - 1]);
								this.Dispatcher.Invoke(() => this.cmbKinder.Items[i - 1] = tmp);
								index2 = int.Parse(this.cmbKinder.Items[i - 1].ToString().Substring(0, tmp.IndexOf(' ')));
							}
						}
					}
				}
				this.Dispatcher.Invoke(() => this.lblGeschwister.Content = currentPerson.Geschwister.Count);
				if (!cmbOpen)
				{
					this.Dispatcher.Invoke(() => this.cmbGeschwister.Items.Clear());
					foreach (Person geschwister in currentPerson.Geschwister)
					{
						this.Dispatcher.Invoke(() => this.cmbGeschwister.Items.Add(geschwister.Nummer.ToString().PadLeft(4, '0') + " " + geschwister.Vorname + " " + geschwister.Familienname));
					}

					for (int j = 0; j < this.cmbGeschwister.Items.Count; j++)
					{
						for (int i = this.cmbGeschwister.Items.Count - 1; i > 0; i--)
						{
							string tmp = this.cmbGeschwister.Items[i].ToString();
							int index1 = int.Parse(tmp.Substring(0, tmp.IndexOf(' ')));
							int index2 = int.Parse(this.cmbGeschwister.Items[i - 1].ToString().Substring(0, tmp.IndexOf(' ')));
							while (index1 < index2)
							{
								this.Dispatcher.Invoke(() => this.cmbGeschwister.Items[i] = this.cmbGeschwister.Items[i - 1]);
								this.Dispatcher.Invoke(() => this.cmbGeschwister.Items[i - 1] = tmp);
								index2 = int.Parse(this.cmbGeschwister.Items[i - 1].ToString().Substring(0, tmp.IndexOf(' ')));
							}
						}
					}
				}
			}
			this.BuildDataFinished?.Invoke(this, new EventArgs());
		}

		private void CurrentPerson_PersonChanged(object sender, Changed e)
		{
			if (!buildDataInThread)
			{
				bool dropOpen = this.cmbGeschwister.IsDropDownOpen || this.cmbKinder.IsDropDownOpen;
				Threads.AddThread("BuildData", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.BuildData(dropOpen))), true);
				buildDataInThread = true;
			}
		}

		private void cmbGeschwister_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.cmbGeschwister.Items.Count > 0)
			{
				foreach (object item in this.personen.Items)
				{
					if (this.cmbGeschwister.SelectedItem.Equals(item))
					{
						this.personen.SelectedItem = item;
						if (this.selectedItem != null && this.selectedItem != this.personen.SelectedItem)
							this.BuildData();
						break;
					}
				}
			}
		}
	}
}