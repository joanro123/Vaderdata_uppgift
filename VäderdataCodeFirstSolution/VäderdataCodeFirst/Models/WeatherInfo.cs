using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace VäderdataCodeFirst.Models
{
    public class WeatherInfo
    {
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public string Plats { get; set; }
        public decimal Temp { get; set; }
        public int Luftfuktighet { get; set; }
    }
}
