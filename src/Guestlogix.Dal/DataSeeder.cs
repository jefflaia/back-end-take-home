using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using Guestlogix.Dal.Domain;

namespace Guestlogix.Dal
{
    public class DataSeeder
    {

        public void SeedData(GlDbContext dbContext)
        {
            var routes = GetDataFromCsv<Route>($"data/routes.csv");
            var airports = GetDataFromCsv<Airport>($"data/airports.csv");
            var airlines = GetDataFromCsv<Airline>($"data/airlines.csv");

            dbContext.Airports.AddRange(airports);
            dbContext.Airlines.AddRange(airlines);
            dbContext.AddRange(routes);
            dbContext.SaveChanges();

        }


        private IEnumerable<T> GetDataFromCsv<T>(string fileName)
        {
            using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                using (var csv = new CsvReader(reader))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
        }

    }
}
