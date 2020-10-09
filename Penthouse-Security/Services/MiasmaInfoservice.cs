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

            var document = await websiteScraper.ScrapeWebsite(Config.vars["coronaSiteUrl"]);

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
                "Total cases: " + counter[totalCases].TextContent.Trim() +
                ", active cases: " + activeCasesCounter.First().TextContent.Trim() + " `(" + activePercentage + "%)`" +
                ", deaths: " + counter[deaths].TextContent.Trim() + " `(" + deathsPercentage + "%)`" +
                ", recovered: " + counter[recovered].TextContent.Trim() + " `(" + recoveredPercentage + "%)`";
        }

        public async Task<string> MiasmaTop10()
        {   
            var document = await websiteScraper.ScrapeWebsite(Config.vars["coronaSiteUrl"]);
            var countryTable = document.All.Where(x => x.Id == "main_table_countries_today").First();
            var tableBody = countryTable.Children[1];

            var tableRows = tableBody.Children.ToArray();
            Array.Sort(tableRows, CompareCountryRowsByTotals);

            var result = new StringBuilder();

            result.AppendLine("__Quranovirus Top10__");
            result.AppendLine();

            int rank = 1;
            int i = 0;
            while (rank <= 10)
            {
                var row = tableRows[i];
                var columns = row.Children;

                // [fix] pomijaj dodatkowe totalsy dla kontynentow
                if (row.ClassName != null && row.ClassName.Contains("total"))
                {
                    i++;
                    continue;
                }
                
                result.Append("#" + rank.ToString());
                result.Append("  *");
                result.Append(columns[2].TextContent.Trim() + "*  (");
                if (columns[1].TextContent.Trim().Length != 0) result.Append(columns[1].TextContent.Trim() + ")");

                if (rank == 1) result.Append(":first_place:");
                if (rank == 2) result.Append(":second_place:");
                if (rank == 3) result.Append(":third_place:");

                result.AppendLine();
                rank++;
                i++;
            }
            return result.ToString();
        }

        public async Task<string> MiasmaByCountryAndDay(string country, string day)
        {
            const int countries = 1;
            string areYesterdaysStats = "";

            string table_html_element = "main_table_countries_today";
            if (day == "yesterday" || day == "yday")
            {
                table_html_element = "main_table_countries_yesterday";
                areYesterdaysStats = " ***(yesterday)***";
            }

            var document = await websiteScraper.ScrapeWebsite(Config.vars["coronaSiteUrl"]);
            var countryTable = document.All.Where(x => x.Id == table_html_element).First();
            var countryTableYday = document.All.Where(x => x.Id == "main_table_countries_yesterday").First();

            var tableBodyYday = countryTableYday.Children[1];
            var tableBody = countryTable.Children[1];

            var tableRows = tableBody.Children.ToArray();
            var tableRowsYday = tableBodyYday.Children.ToArray();
            Array.Sort(tableRows, CompareCountryRowsByTotals);
            Array.Sort(tableRowsYday, CompareCountryRowsByTotals);

            int savedYdayNewCases = 0;
            for (int i = 0; i < tableRowsYday.Length; ++i)
            {
                var row = tableRowsYday[i];
                var columns = row.Children;

                // [fix] pomijaj dodatkowe totalsy dla kontynentow
                if (row.ClassName != null && row.ClassName.Contains("total")) continue;

                if (row.TextContent.ToLower().Contains(country.ToLower()))
                {
                    try
                    {
                        var trimmed = columns[3].TextContent.Trim().Replace("+", "").Replace(",", "");
                        savedYdayNewCases = int.Parse(trimmed);
                    }
                    catch (Exception)
                    {
                        Log.Error("Wrong saved yday new cases format!");
                    }
                    break;
                }
            }

            int rank = 1;
            for (int i = 0; i < tableRows.Length; ++i)
            {
                var row = tableRows[i];
                var columns = row.Children;

                // [fix] pomijaj dodatkowe totalsy dla kontynentow
                if (row.ClassName != null && row.ClassName.Contains("total")) continue;

                if(row.TextContent.ToLower().Contains(country.ToLower()))
                {
                    var result = new StringBuilder();
                    result.AppendLine("__Quranovirus stats for: **" + columns[1].TextContent.Trim() + "**__" + areYesterdaysStats);
                    result.AppendLine();

                    for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                    {
                        var content = columns[columnIndex].TextContent.Trim();
                        if (content.Length == 0 || content == null)
                        {
                            content = "N/A";
                        }

                        switch (columnIndex)
                        {                            
                            case 2:
                                result.Append("Cases: " + content);
                                break;
                            case 3:

                                int delta = 0;
                                try
                                {
                                    delta = int.Parse(content.Replace("+", "").Replace(",", "")) - savedYdayNewCases;
                                }
                                catch(Exception)
                                {
                                    Log.Error("Delta cannot be parsed!");
                                }
                                
                                if(delta > 0)
                                {
                                    result.AppendLine(" (**" + content + "**)" + " delta: +" + delta + ":chart_with_upwards_trend:");
                                }
                                else if(delta < 0)
                                {
                                    result.AppendLine(" (**" + content + "**)" + " delta: " + delta + ":chart_with_downwards_trend:");
                                }
                                else
                                {
                                    result.AppendLine(" (**" + content + "**)" + " delta: " + delta);
                                }
                                
                                break;
                            case 4:
                                result.Append("Deaths: " + content);
                                break;
                            case 5:
                                result.AppendLine(" (" + content + ")");
                                break;
                            case 6:
                                result.AppendLine("Recovered: " + content);
                                break;
                            case 8:
                                result.AppendLine("Active: " + content);
                                break;
                            default:
                                break;
                        }
                    }  

                    result.AppendLine();
                    result.AppendLine("Rank: **#" + rank.ToString() + "**");
                    
                    return result.ToString();
                }
                rank++;
            }
            return "Ale wpisz kurwa pajacu porzadnie to kauntry, z duzej jebanej litery i po angielsku jak pan bug przykazal.";            
        }
        
        public int CompareCountryRowsByTotals(AngleSharp.Dom.IElement x, AngleSharp.Dom.IElement y)
        {
            var xColumns = x.Children;
            int xTotals = int.Parse(xColumns[2].TextContent.Replace(",", ""));

            var yColumns = y.Children;
            int yTotals = int.Parse(yColumns[2].TextContent.Replace(",", ""));

            if (xTotals > yTotals) { return -1; }
            else if (xTotals < yTotals) { return 1; }
            else return 0;
        }
    }
}
