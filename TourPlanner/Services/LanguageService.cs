using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TourPlanner.Stores;

namespace TourPlanner.Services {
    public class LanguageService {

        private Dictionary<string, Dictionary<string, string>> languageDictionary;
        private readonly LanguageStore _languageStore;

        public LanguageService(IServiceProvider serviceProvider, IConfiguration config) {
            _languageStore = serviceProvider.GetRequiredService<LanguageStore>();

            var lang = Serialize(config.GetSection("Languages"));

            Console.WriteLine(lang);

            languageDictionary = new Dictionary<string, Dictionary<string, string>>();

            foreach (var v in lang) {
                languageDictionary.Add(v.Path, new Dictionary<string, string>());

                var l = languageDictionary[v.Path];

                foreach(var w in v.Children().Children()) {
                    var key = w.Path.Substring(v.Path.Length + 1);

                    l.Add(key, w.First().ToString());
                }
            }
            
        }

        public string[] getLanguages() {
            return languageDictionary.Keys.ToArray();
        }

        public string getVariable(string key) {
            var v = languageDictionary[_languageStore.CurrentLanguage];
            return v[key];
        }

        //Working with Languages
        private JToken Serialize(IConfiguration config) {
            JObject obj = new JObject();
            foreach (var child in config.GetChildren()) {
                obj.Add(child.Key, Serialize(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
                return new JValue(section.Value);

            return obj;
        }
    }
}
