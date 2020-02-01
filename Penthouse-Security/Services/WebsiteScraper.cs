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
                var counter = document.All.Where(x => x.ClassName == "maincounter-number");                
                return "Infected: " + counter.First().TextContent + ", deaths:" + counter.ToArray()[1].TextContent;
            }
            else
            {
                Log.Error("Invalid response.");
                return "Service unavailable.";
            }
        }
    }
}
