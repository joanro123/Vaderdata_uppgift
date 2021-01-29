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
    class ToolsInside
    {
        public static void AverageTempIn(string date)
        {
            List<decimal?> lista = new List<decimal?>();
            try
            {

                using (var context = new EFContext())
                {
                    var temp = context.WeatherInfo.Where(t => t.Plats == "Inne" && t.Datum.ToString().Contains(date));

                    foreach (var item in temp)
                    {
                        lista.Add(item.Temp);
                    }

                }
                var average = Math.Round((double)lista.Average(), 1);

                Console.WriteLine($"Medeltemperatur inomhus för {date} var {average} grader");
            }
            catch
            {
                Console.WriteLine("Datum kunde inte hittas");
            }
        }

        public static void WarmestDayIn()
        {

            using (var context = new EFContext())
            {

                var q = context.WeatherInfo.Where(r => r.Plats == "Inne")
                    .GroupBy(s => s.Datum.Date)
                    .Select(t => new { Datum = (DateTime)t.Key, Avg = t.Average(a => a.Temp) })
                    .OrderByDescending(o => o.Avg)
                    .Take(5);

                foreach (var item in q)
                {
                    Console.WriteLine(item.Datum.ToShortDateString());
                    Console.WriteLine(Math.Round((double)item.Avg, 1) + " grader");
                    Console.WriteLine();
                }

            }


        }

        public static void DriestDayIn()
        {

            using (var context = new EFContext())
            {
                var q = context.WeatherInfo.Where(s => s.Plats == "Inne")
                    .GroupBy(t => t.Datum.Date)
                    .Select(u => new { Datum = (DateTime)u.Key, Avg = u.Average(a => a.Luftfuktighet) })
                    .OrderBy(o => o.Avg);

                foreach (var item in q)
                {
                    Console.WriteLine(item.Datum.ToShortDateString());
                    Console.WriteLine("Luftfuktighet " + Math.Round((double)item.Avg, 0));
                    Console.WriteLine();
                }
            }

        }

        public static void RiskForMoldInside()
        {
            using (var context = new EFContext())
            {
                var q = context.WeatherInfo.Where(s => s.Plats == "Inne")
                    .GroupBy(t => t.Datum.Date)
                    .Select(u => new { Datum = (DateTime)u.Key, Fukt = u.Average(a => a.Luftfuktighet), Värme = u.Average(v => v.Temp) });


                List<Mold> molds = new List<Mold>();

                foreach (var item in q)
                {
                    double mögelrisk = (((int)item.Fukt - 78) * ((double)item.Värme / 15)) / 0.22;

                    molds.Add(new Mold() { Datum = item.Datum, Fukt = item.Fukt, Värme = item.Värme, Mögel = mögelrisk });

                }

                var order = molds.OrderByDescending(o => o.Mögel)
                    .Take(10);

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
    }
}
