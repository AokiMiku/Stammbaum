using System;
using System.Data;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using ApS;
using Stammbaum;

namespace Stammbaum
{
    class Program
    {
		private static StammbaumBusiness business;
		private static DispatcherTimer gameTime;
        static void Main(string[] args)
        {

			gameTime = new DispatcherTimer();
			gameTime.Tick += GameTime_Tick;
			gameTime.Interval = new TimeSpan(0, 0, 0, 0, 500);


			business = new StammbaumBusiness();
			business.Ausgabe += Business_Ausgabe;
            try
            {
				business.LoadPersons();

				//Person[] personen = new Person[StammbaumBusiness.WorkingList.Count];
				//StammbaumBusiness.WorkingList.CopyTo(personen);
				//foreach (var item in personen)
				//{
				//	if (item.Partner != 0)
				//		item.ZeugeKind();
				//}

				AlleDatenAusgeben();

				Console.ReadKey(true);
				//business.StarteSimulation();
				gameTime.Start();

                Console.ReadKey();
            }
            finally
            {
            }
        }

		private static void GameTime_Tick(object sender, EventArgs e)
		{
			Console.WriteLine("Tick vergangen!");
		}

		private static void Business_Ausgabe(object sender, Ausgabe e)
		{
			Console.WriteLine("{0}{1}{0}", Environment.NewLine, e.Message);
		}

		private static void AlleDatenAusgeben()
        {
			Queue<KeyValuePair<string, object>[]> ergebnis = business.OutputQueue;
			while (ergebnis.Count > 0)
			{
				Console.WriteLine("".PadRight(30, '-'));
				foreach (var item in ergebnis.Dequeue())
				{
					Console.WriteLine((item.Key + ": ").PadRight(20, ' ') + item.Value);
				}
			}
		}
    }
}