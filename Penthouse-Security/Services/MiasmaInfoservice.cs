using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class MiasmaInfoservice
    {
        private WebsiteScraper websiteScraper;
        public MiasmaInfoservice(WebsiteScraper websiteScraper)
        {
            this.websiteScraper = websiteScraper ?? throw new ArgumentNullException("webSitescraper");
        }

        public async Task<string> MiasmaTotals()
        {
            const int totalCases = 0;
            const int deaths = 1;
            const int recovered = 2;

            var document = await websiteScraper.ScrapeWebsite("https://www.worldometers.info/coronavirus/");

            var activeCasesCounter = document.All.Where(x => x.ClassName == "number-table-main").ToArray();
            var counter = document.All.Where(x => x.ClassName == "maincounter-number").ToArray();            

            var totalCount = Double.Parse(counter[totalCases].TextContent.Replace(",", "").Trim());
            var activeCount = Double.Parse(activeCasesCounter.First().TextContent.Replace(",", "").Trim());
            var deathsCount = Double.Parse(counter[deaths].TextContent.Replace(",", "").Trim());
            var recoveredCount = Double.Parse(counter[recovered].TextContent.Replace(",", "").Trim());

            int activePercentage = Convert.ToInt32(100 * activeCount / totalCount);
            int deathsPercentage = Convert.ToInt32(100 * deathsCount / totalCount);
            int recoveredPercentage = Convert.ToInt32(100 * recoveredCount / totalCount);

            return
                "Total cases:" + counter[totalCases].TextContent +
                ", active cases: " + activeCasesCounter.First().TextContent + " `(" + activePercentage + "%)`" +
                ", deaths: " + counter[deaths].TextContent + " `(" + deathsPercentage + "%)`" +
                ", recovered: " + counter[recovered].TextContent + " `(" + recoveredPercentage + "%)`";
        }

        public async Task<string> MiasmaByCountry(string country)
        {
            //const int header = 0;
            const int countries = 1;

            var document = await websiteScraper.ScrapeWebsite("https://www.worldometers.info/coronavirus/");

            var countryTable = document.All.Where(x => x.Id == "main_table_countries").First();

            //var tableHeader = polsza.Children[header];
            var tableBody = countryTable.Children[1];

            for(int i = 0; i < tableBody.Children.Length; ++i)
            {
                var entry = tableBody.Children[i];
                var columns = entry.Children;
                if(entry.TextContent.ToLower().Contains(country.ToLower()))
                {

                    var result = new StringBuilder();
                    result.AppendLine("__Quranovirus stats for: **" + columns[0].TextContent.Trim() + "**__");
                    result.AppendLine();
                    if (columns[1].TextContent.Trim().Length != 0) result.AppendLine("Total Cases: " + columns[1].TextContent.Trim());
                    if (columns[2].TextContent.Trim().Length != 0) result.AppendLine("New Cases: " + columns[2].TextContent.Trim());
                    if (columns[3].TextContent.Trim().Length != 0) result.AppendLine("Total Deaths: " + columns[3].TextContent.Trim());
                    if (columns[4].TextContent.Trim().Length != 0) result.AppendLine("New Deaths: " + columns[4].TextContent.Trim());
                    if (columns[5].TextContent.Trim().Length != 0) result.AppendLine("Total Recovered: " + columns[5].TextContent.Trim());
                    if (columns[6].TextContent.Trim().Length != 0) result.AppendLine("Active Cases: " + columns[6].TextContent.Trim());

                    return result.ToString();
                }                
            }
            return "Ale wpisz kurwa pajacu porzadnie to kauntry, z duzej jebanej litery i po angielsku jak pan bug przykazal.";            
        }
    }
}
