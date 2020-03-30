﻿using System;
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

        public async Task<string> MiasmaTop10States()
        {
            var document = await websiteScraper.ScrapeWebsite(Config.vars["coronaSiteUrlStates"]);
            var countryTable = document.All.Where(x => x.Id == "usa_table_countries_today").First();
            var tableBody = countryTable.Children[1];

            var tableRows = tableBody.Children.ToArray();
            Array.Sort(tableRows, CompareCountryRowsByTotals);

            var result = new StringBuilder();

            result.AppendLine("__Quranovirus Top10 USA States__");
            result.AppendLine();

            for (int i = 0; i < 10; ++i)
            {
                var row = tableRows[i];
                var columns = row.Children;

                result.Append("#" + (i + 1).ToString());
                result.Append("  *");
                result.Append(columns[0].TextContent.Trim() + "*  (");
                if (columns[1].TextContent.Trim().Length != 0) result.Append(columns[1].TextContent.Trim() + ")");

                if (i == 0) result.Append(":first_place:");
                if (i == 1) result.Append(":second_place:");
                if (i == 2) result.Append(":third_place:");

                result.AppendLine();
            }
            return result.ToString();
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

            for (int i = 0; i < 10; ++i)
            {
                var row = tableRows[i];
                var columns = row.Children;

                result.Append("#" + (i + 1).ToString());
                result.Append("  *");
                result.Append(columns[0].TextContent.Trim() + "*  (");
                if (columns[1].TextContent.Trim().Length != 0) result.Append(columns[1].TextContent.Trim() + ")");

                if (i == 0) result.Append(":first_place:");
                if (i == 1) result.Append(":second_place:");
                if (i == 2) result.Append(":third_place:");

                result.AppendLine();
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
            var tableBody = countryTable.Children[1];

            var tableRows = tableBody.Children.ToArray();
            Array.Sort(tableRows, CompareCountryRowsByTotals);

            for (int i = 0; i < tableRows.Length; ++i)
            {
                var row = tableRows[i];
                var columns = row.Children;
                if(row.TextContent.ToLower().Contains(country.ToLower()))
                {
                    var result = new StringBuilder();
                    result.AppendLine("__Quranovirus stats for: **" + columns[0].TextContent.Trim() + "**__" + areYesterdaysStats);
                    result.AppendLine();
                    if (columns[1].TextContent.Trim().Length != 0) result.AppendLine("Total Cases: " + columns[1].TextContent.Trim());
                    if (columns[2].TextContent.Trim().Length != 0) result.AppendLine("New Cases: " + columns[2].TextContent.Trim());
                    if (columns[3].TextContent.Trim().Length != 0) result.AppendLine("Total Deaths: " + columns[3].TextContent.Trim());
                    if (columns[4].TextContent.Trim().Length != 0) result.AppendLine("New Deaths: " + columns[4].TextContent.Trim());
                    if (columns[5].TextContent.Trim().Length != 0) result.AppendLine("Total Recovered: " + columns[5].TextContent.Trim());
                    if (columns[6].TextContent.Trim().Length != 0) result.AppendLine("Active Cases: " + columns[6].TextContent.Trim());
                    result.AppendLine();
                    result.AppendLine("Rank: **#" + (i + 1).ToString() + "**");

                    return result.ToString();
                }                
            }
            return "Ale wpisz kurwa pajacu porzadnie to kauntry, z duzej jebanej litery i po angielsku jak pan bug przykazal.";            
        }

        public async Task<string> MiasmaRank(string rank)
        {
            int iRank = int.Parse(rank);
                        
            var document = await websiteScraper.ScrapeWebsite(Config.vars["coronaSiteUrl"]);
            var countryTable = document.All.Where(x => x.Id == "main_table_countries_today").First();
            var tableBody = countryTable.Children[1];

            var tableRows = tableBody.Children.ToArray();
            Array.Sort(tableRows, CompareCountryRowsByTotals);

            var result = new StringBuilder();

            result.AppendLine("__Quranovirus at rank: **#" + iRank + "**__");
            result.AppendLine();

            var row = tableRows[iRank - 1];
            var columns = row.Children;

            result.Append(columns[0].TextContent.Trim() + " (");
            if (columns[1].TextContent.Trim().Length != 0) result.Append(columns[1].TextContent.Trim() + " cases)");

            result.AppendLine();

            return result.ToString();
        }

        public int CompareCountryRowsByTotals(AngleSharp.Dom.IElement x, AngleSharp.Dom.IElement y)
        {
            var xColumns = x.Children;
            int xTotals = int.Parse(xColumns[1].TextContent.Replace(",", ""));

            var yColumns = y.Children;
            int yTotals = int.Parse(yColumns[1].TextContent.Replace(",", ""));

            if (xTotals > yTotals) { return -1; }
            else if (xTotals < yTotals) { return 1; }
            else return 0;
        }
    }
}
