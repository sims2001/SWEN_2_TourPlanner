using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TourPlanner.Models {
    internal class OnlineRoute {

        public string From { get; }
        public string To { get; }
        public string Transport { get; }
        public int Time { get; set; }
        public double Distance { get; set; }


        public OnlineRoute(string from,  string to, string tt) {
            From = from;
            To = to;
            Transport = tt;

            Time = 1500;
            Distance = 3.50;
            //getData(); 
        }

        private async Task getData() {
            var builder = new UriBuilder("https://www.mapquestapi.com/directions/v2/route");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = "By1wHXsGAwuUXAsjCqHgg5yTAxAq1KfW";
            query["from"] = From;
            query["to"] = To;
            query["outFormat"] = "json";
            query["ambiguities"] = "ignore";
            query["routeType"] = "fastest";
            builder.Query = query.ToString();
            string url = builder.ToString();
            using var client = new HttpClient();
            var content = await client.GetStringAsync(url);
            var response = JObject.Parse(content);
            var info = (JToken)response["info"];
            var status = (int)info.FirstOrDefault().Last;

            if ( status != 0 ) {
                throw new Exception();
            }

            Console.WriteLine(response);
        }
    }
}
