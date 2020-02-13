namespace StammbaumVisual
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

	using ApS;
	using Stammbaum;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		StammbaumBusiness business = new StammbaumBusiness();
		DispatcherTimer gameTime = new DispatcherTimer();
		int[,] control = null;

		public MainWindow()
		{
			InitializeComponent();

			business.Ausgabe += Business_Ausgabe;

			gameTime.Interval = new TimeSpan(0, 0, 0, 0, 10);
			gameTime.Tick += GameTime_Tick;
		}

		private void GameTime_Tick(object sender, EventArgs e)
		{
			gameTime.Stop();
			Dispatcher.Invoke(() => datum.Content = StammbaumBusiness.AktuellerTagInSimulation.ToShortDateString());
			business.Simulation();

			Random rnd = new Random();
			int x = 0;
			int y = 0;
			foreach (Person pers in StammbaumBusiness.WorkingList)
			{
				if (!control.Contains(pers.Nummer))
				{
					pers.PersonChanged += Pers_PersonChanged;
					do
					{
						x = rnd.Next(control.GetLength(0));
						y = rnd.Next(control.GetLength(1));
					} while (control[x, y] != 0);
					control[x, y] = pers.Nummer;

					Label lbl = new Label();
					if (pers.Geschlecht == 'M')
						lbl.Background = Brushes.Blue;
					else
						lbl.Background = Brushes.Red;
					lbl.Content = pers.Vorname.Substring(0, 2) + pers.Familienname.Substring(0, 2);
					lbl.Tag = pers.Nummer;
					lbl.FontSize = 8;
					Grid.SetColumn(lbl, x);
					Grid.SetRow(lbl, y);
					gameGrid.Children.Add(lbl);
				}
			}

			gameTime.Start();
		}

		private void Pers_PersonChanged(object sender, Changed e)
		{
			if (e.Property == Changed.ChangedProperty.Lebend && !e.Person.Lebend)
			{
				for (int i = 0; i < gameGrid.Children.Count; i++)
				{
					if (((Label)gameGrid.Children[i]).Tag?.ToInt() == e.Person.Nummer)
					{
						int x = Grid.GetColumn(gameGrid.Children[i]);
						int y = Grid.GetRow(gameGrid.Children[i]);
						control[x, y] = 0;
						gameGrid.Children.RemoveAt(i);
						break;
					}
				}
			}
		}

		private void Business_Ausgabe(object sender, Ausgabe e)
		{
			this.Dispatcher.Invoke(() => this.output.Content = e.Message);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < 80; i++)
			{
				gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
				gameGrid.RowDefinitions.Add(new RowDefinition());
			}
			//Grid.SetColumn(datum, 0);
			//Grid.SetRow(datum, gameGrid.RowDefinitions.Count - 2);
			//Grid.SetColumnSpan(datum, gameGrid.ColumnDefinitions.Count);
			//Grid.SetColumn(output, 0);
			//Grid.SetRow(output, gameGrid.RowDefinitions.Count - 1);
			//Grid.SetColumnSpan(output, gameGrid.ColumnDefinitions.Count);
			control = new int[gameGrid.ColumnDefinitions.Count, gameGrid.RowDefinitions.Count - 2];

			Threads.AddThread("LoadAll", new System.Threading.Thread(new System.Threading.ThreadStart(() => this.business.LoadAll())), true);
			this.business.LoadAllFinished += Business_LoadAllFinished;
		}

		private void Business_LoadAllFinished(object sender, EventArgs e)
		{
			Threads.RemoveThread("LoadAll", false);
			gameTime.Start();
		}

		private void Window_Initialized(object sender, EventArgs e)
		{

		}
	}
}