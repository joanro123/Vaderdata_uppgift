using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using VäderdataCodeFirst.Models;

namespace VäderdataCodeFirst.Data
{
    public class EFContext : DbContext
    { 
           public const string connectionstring = @"Server=(localdb)\MsSqlLocalDb;Database=VäderdataCodeFirst;Trusted_Connection=True";
            public DbSet<WeatherInfo> WeatherInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionstring);
        }


        
    }
}
