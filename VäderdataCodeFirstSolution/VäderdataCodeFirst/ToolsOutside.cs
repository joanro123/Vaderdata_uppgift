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
    class ToolsOutside
    {
        public static void AverageTempOut(string date)
        {
            List<decimal?> lista = new List<decimal?>();
            try
            {

                using (var context = new EFContext())
                {
                    var temp = context.WeatherInfo.Where(t => t.Plats == "Ute" && t.Datum.ToString().Contains(date));

                    foreach (var item in temp)
                    {
                        lista.Add(item.Temp);
                    }

                }
                var average = Math.Round((double)lista.Average(), 1);

                Console.WriteLine($"Medeltemperatur utomhus för {date} var {average} grader");
            }
            catch
            {
                Console.WriteLine("Datum kunde inte hittas");
            }
        }

        public static void WarmestDayOut()
        {
            using (var context = new EFContext())
            {

                var q = context.WeatherInfo.Where(r => r.Plats == "Ute")
                    .GroupBy(s => s.Datum.Date)
                    .Select(t => new { Datum = (DateTime)t.Key, Avg = t.Average(a => a.Temp) })
                    .OrderByDescending(o => o.Avg)
                    .Take(10);

                foreach (var item in q)
                {
                    Console.WriteLine(item.Datum.ToShortDateString());
                    Console.WriteLine(Math.Round((double)item.Avg, 1) + " grader");
                    Console.WriteLine();
                }

            }

        }

        public static void DriestDayOut()
        {

            using (var context = new EFContext())
            {
                var q = context.WeatherInfo.Where(s => s.Plats == "Ute")
                    .GroupBy(t => t.Datum.Date)
                    .Select(u => new { Datum = (DateTime)u.Key, Avg = u.Average(a => a.Luftfuktighet) })
                    .OrderBy(o => o.Avg)
                    .Take(10);

                foreach (var item in q)
                {
                    Console.WriteLine(item.Datum.ToShortDateString());
                    Console.WriteLine("Luftfuktighet " + Math.Round((double)item.Avg, 0));
                    Console.WriteLine();
                }
            }

        }

        public static void RiskForMold()
        {
            using (var context = new EFContext())
            {
                var q = context.WeatherInfo.Where(s => s.Plats == "Ute")
                    .GroupBy(t => t.Datum.Date)
                    .Select(u => new { Datum = (DateTime)u.Key, Fukt = u.Average(a => a.Luftfuktighet), Värme = u.Average(v => v.Temp) });


                List<Mold> molds = new List<Mold>();

                foreach (var item in q)
                {
                    double mögelrisk = (((int)item.Fukt - 78) * ((double)item.Värme / 15)) / 0.22;

                    molds.Add(new Mold() { Datum = item.Datum, Fukt = item.Fukt, Värme = item.Värme, Mögel = mögelrisk });

                }

                var order = molds.OrderByDescending(o => o.Mögel)
                    .Take(20);

                foreach (var i in order)
                {

                    if (i.Mögel > 0)
                    {
                        Console.WriteLine(i.Datum.ToShortDateString());
                        Console.WriteLine("Värme: " + Math.Round((double)i.Värme, 1));
                        Console.WriteLine("Fuktighet: " + (int)i.Fukt);
                        Console.WriteLine("Mögelrisken är " + Math.Round(i.Mögel, 1) + " %");
                    }
                    else
                    {
                        Console.WriteLine(i.Datum.ToShortDateString());
                        Console.WriteLine("Värme: " + Math.Round((double)i.Värme, 1));
                        Console.WriteLine("Fuktighet: " + (int)i.Fukt);
                        Console.WriteLine("inte risk för mögel");
                    }
                    Console.WriteLine();
                }

            }

        }

        public static void MeteorologiskHöst()
        {
            int counter = 0;
            List<DateTime> lista = new List<DateTime>();

            using (var context = new EFContext())
            {
                var q = context.WeatherInfo.Where(s => s.Plats == "Ute")
                    .GroupBy(t => t.Datum.Date)
                    .Select(u => new { Datum = (DateTime)u.Key, Temperatur = u.Average(a => a.Temp) })
                    .OrderBy(o => o.Datum);

                foreach (var item in q)
                {
                    if (item.Temperatur < 10)
                    {
                        //   Console.WriteLine(item.Datum);
                        // Console.WriteLine(Math.Round((double)item.Temperatur, 1));
                        //  Console.WriteLine();
                        counter++;
                        lista.Add(item.Datum);
                        {
                            if (counter == 5)
                            {
                                //  Console.WriteLine("5 dagar under 10 grader i rad.");
                                break;
                            }
                        }

                    }
                    else
                    {
                        counter = 0;
                        lista.Clear();
                    }

                }

                DateTime första = lista[0];

                Console.WriteLine("Hösten anlände: " + första.ToShortDateString());

            }
        }

        public static void MeteorologiskVinter()
        {
            int counter = 0;
            List<DateTime> lista = new List<DateTime>();

            using (var context = new EFContext())
            {
                var q = context.WeatherInfo.Where(s => s.Plats == "Ute")
                    .GroupBy(t => t.Datum.Date)
                    .Select(u => new { Datum = (DateTime)u.Key, Temperatur = u.Average(a => a.Temp) })
                    .OrderBy(o => o.Datum);

                foreach (var item in q)
                {
                    if (item.Temperatur <= 0)
                    {
                        Console.WriteLine(item.Datum.ToShortDateString());
                        Console.WriteLine(Math.Round((double)item.Temperatur, 1));
                        Console.WriteLine();
                        counter++;
                        lista.Add(item.Datum);
                        {
                            if (counter == 5)
                            {
                                Console.WriteLine("5 dagar under 0 grader i rad.");
                                break;
                            }
                        }

                    }
                    else
                    {
                        counter = 0;
                        lista.Clear();
                    }

                }

                //   DateTime första = lista[0];

                //  Console.WriteLine("Vintern anlände: " + första);

                if (counter < 5)
                {
                    Console.WriteLine("Ingen meteorologisk vinter.");
                }

            }

        }




    }
    class Mold
    {
        public DateTime Datum { get; set; }
        public double Fukt { get; set; }
        public decimal? Värme { get; set; }
        public double Mögel { get; set; }
    }
}

