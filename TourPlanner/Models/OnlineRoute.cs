using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web;
using TourPlanner.Exceptions;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TourPlanner.Models {
    internal class OnlineRoute {
        private readonly IConfiguration _configuration;
        public string From { get; protected set; }
        public string To { get; protected set; }
        public string Transport { get; protected set; }
        public int Time { get; protected set; }
        public double Distance { get; protected set; }
        public string PicPath { get; protected set; } = string.Empty;
        public byte[]? mapArray { get; protected set; }

        public OnlineRoute(string from, string to, string tt, IServiceProvider serviceProvider) {
            From = from;
            To = to;
            Transport = tt;
            _configuration = serviceProvider.GetService<IConfiguration>();
        }

        private async Task getData() {
            //Build Uri String -> Auslagern in Config
            var builder = new UriBuilder("https://www.mapquestapi.com/directions/v2/route");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = _configuration["MapQuestAPI:key"];
            query["from"] = From;
            query["to"] = To;
            query["outFormat"] = "json";
            query["ambiguities"] = "ignore";
            query["routeType"] = Transport;
            query["doReverseGeocode"] = "false";
            query["enhancedNarrative"] = "false";
            query["avoidTimedConditions"] = "false";
            builder.Query = query.ToString();
            string url = builder.ToString();


            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var content = JObject.Parse(response);
            var info = content.SelectToken("info");//(JToken)content["info"]; 
            var status = info.Value<int>("statuscode");


            if ( status != 0 ) throw new RouteNotFoundException();

            var route = content.SelectToken("route");
            Console.WriteLine(route);

            Distance = route.Value<double>("distance");
            Time = route.Value<int>("time");

            await setMap();
        }

        private async Task setMap() {
            var builder = new UriBuilder("https://www.mapquestapi.com/staticmap/v5/map");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["start"] = From;
            query["end"] = To;
            query["size"] = "1600,900";
            query["format"] = "png";
            query["key"] = _configuration["MapQuestAPI:key"];
            builder.Query = query.ToString();
            string url = builder.ToString();

            using var client = new HttpClient();
            var map = await client.GetByteArrayAsync(url);
            mapArray = map;

            PicPath = SaveImage();
        } 

        private string SaveImage() {
            string picDir = "C:\\Users\\Simon\\Desktop\\WEB_PROJEKTE\\TourPlanner\\MapImages";
            Directory.CreateDirectory(picDir);
            var path = $"{picDir}\\{DateTime.Now.ToString("dd'-'MM'-'yyyy'T'HH'_'mm'_'ss")}.png";
            var fs = File.Create(path);
            fs.Write(mapArray);
            fs.Close();

            return path;
        }

        public static async Task<OnlineRoute> GetOnlineRoute(string from, string to, string tt, IServiceProvider serviceProvider) {
            var newRoute = new OnlineRoute(from, to, tt, serviceProvider);
            await newRoute.getData();
            return newRoute;
        }
        
    }
}
