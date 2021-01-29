using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using VäderdataCodeFirst.Data;
using VäderdataCodeFirst.Models;
namespace VäderdataCodeFirst
{
    class Window
    {
        public static void FileImport()
        {
            using (EFContext context = new EFContext())
            {

                string line;
                List<WeatherInfo> lista = new List<WeatherInfo>();
                using (StreamReader file = new StreamReader(@"C:\Users\jonas\source\repos\VäderdataCodeFirstSolution\VäderdataCodeFirst\Data\TemperaturData - Blad1.csv"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');

                        try
                        {

                            lista.Add(new WeatherInfo { Datum = Convert.ToDateTime(fields[0]), Plats = fields[1].ToString(), Temp = Convert.ToDecimal(fields[2], CultureInfo.InvariantCulture), Luftfuktighet = Convert.ToInt32(fields[3]) });

                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }



                }

                foreach (var item in lista)
                {
                    context.WeatherInfo.Add(item);
                }

                context.SaveChanges();

            }
        }

        public static void MainWindow()
        {
            bool running = true;
            string svar;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkCyan;

            Console.Clear();

            while (running)
            {
                Console.WriteLine("Här hittar du temperaturinformation.");
                Console.WriteLine();
                Console.WriteLine("Välj utomhus eller inomhus[1-2]:");
                Console.WriteLine();
                Console.WriteLine("1. Utomhus");
                Console.WriteLine("2. Inomhus");
                Console.WriteLine();
                svar = Console.ReadLine();

                if (svar == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Vill du veta:");
                    Console.WriteLine();
                    Console.WriteLine("1. Medeltemperatur för vald datum?");
                    Console.WriteLine("2. Sortering av varmast datum till kallaste dagen enligt medeltemperatur per dag?");
                    Console.WriteLine("3. Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag");
                    Console.WriteLine("4. Sortering av minst till störst risk för mögel?");
                    Console.WriteLine("5. Datum för meteorologisk höst?");
                    Console.WriteLine("6. Datum för meteorologisk vinter?");
                    Console.WriteLine();
                    svar = Console.ReadLine();

                    switch (svar)
                    {
                        case "1":
                            Console.WriteLine("Välj datum (ÅÅÅÅ-MM-DD)");
                            string datum = Console.ReadLine();
                            ToolsOutside.AverageTempOut(datum);
                            break;
                        case "2":
                            Console.WriteLine();
                            ToolsOutside.WarmestDayOut();
                            break;
                        case "3":
                            Console.WriteLine();
                            ToolsOutside.DriestDayOut();
                            break;
                        case "4":
                            Console.WriteLine();
                            ToolsOutside.RiskForMold();
                            break;
                        case "5":
                            Console.WriteLine();
                            ToolsOutside.MeteorologiskHöst();
                            break;
                        case "6":
                            Console.WriteLine();
                            ToolsOutside.MeteorologiskVinter();
                            break;
                        default:
                            Console.WriteLine("Välj mellan 1-6");
                            break;
                    }

                }
                else if (svar == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Vill du veta:");
                    Console.WriteLine();
                    Console.WriteLine("1. Medeltemperatur för valt datum?");
                    Console.WriteLine("2. Sortering av varmast till kallaste dagen enligt medeltemperatur per dag?");
                    Console.WriteLine("3. Sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag?");
                    Console.WriteLine("4. Sortering av minst till störst risk för mögel?");
                    Console.WriteLine();
                    svar = Console.ReadLine();
                    switch (svar)
                    {
                        case "1":
                            Console.WriteLine("Välj datum (ÅÅÅÅ-MM-DD)");
                            string datum = Console.ReadLine();
                            ToolsInside.AverageTempIn(datum);
                            break;
                        case "2":
                            Console.WriteLine();
                            ToolsInside.WarmestDayIn();
                            break;
                        case "3":
                            Console.WriteLine();
                            ToolsInside.DriestDayIn();
                            break;
                        case "4":
                            Console.WriteLine();
                            ToolsInside.RiskForMoldInside();
                            break;
                        default:
                            Console.WriteLine("Välj mellan 1-4");
                            break;
                    }


                }
                else if (svar == "a")
                {
                    running = false;
                }

                else if (svar != "1" || svar != "2")
                {
                    Console.WriteLine("Välj 1 eller 2 eller avsluta programmet med A");
                }
                Console.WriteLine();
            }
        }
    }
}
