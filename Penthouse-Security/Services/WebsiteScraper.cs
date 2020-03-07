using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    internal class WebsiteScraper
    {
        private const int totalCases = 0;
        private const int deaths = 1;
        private const int recovered = 2;

        private string siteUrl = "https://www.worldometers.info/coronavirus/";
        internal async Task<string> ScrapeWebsite()
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);

            if (response != null)
            {
                var activeCasesCounter = document.All.Where(x => x.ClassName == "number-table-main").ToArray();
                var counter = document.All.Where(x => x.ClassName == "maincounter-number").ToArray();

                var totalCount = Double.Parse(counter[totalCases].TextContent.Replace(",", "").Trim());
                var activeCount = Double.Parse(activeCasesCounter.First().TextContent.Replace(",", "").Trim());
                var deathsCount = Double.Parse(counter[deaths].TextContent.Replace(",", "").Trim());
                var recoveredCount = Double.Parse(counter[recovered].TextContent.Replace(",", "").Trim());

                int activePercentage = Convert.ToInt32(100 * activeCount / totalCount);
                int deathsPercentage = Convert.ToInt32(100 * deathsCount / totalCount);
                int recoveredPercentage = Convert.ToInt32(100 * recoveredCount / totalCount);

                Log.Debug(Convert.ToInt32((activePercentage * 100)).ToString());

                return "Total cases:"     + counter[totalCases].TextContent +
                       ", active cases: " + activeCasesCounter.First().TextContent + " `(" + activePercentage + "%)`" +
                       ", deaths: "       + counter[deaths].TextContent + " `(" + deathsPercentage + "%)`" +
                       ", recovered: "    + counter[recovered].TextContent + " `(" + recoveredPercentage + "%)`";
            }
            else
            {
                Log.Error("Invalid response.");
                return "Service unavailable.";
            }
        }
    }
}
