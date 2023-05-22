using Faker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TourPlanner.Models {
    public record Tour {
        //[JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public string From { get; set; } = String.Empty;
        public string To { get; set; } = String.Empty;
        public TransportType TransportType { get; set; }
        public double Distance { get; set; }    
        public int Time { get; set; }
        public string FormatedTime { get => FormatTime(); } 
        ////public string PicturePath { get; set; } = String.Empty;
        public bool ChildFriendly { get; set; }
        //public IEnumerable<TourLog>? TourLogs { get; set; }
        //public IEnumerable<String?> ErrorMessages { get; set; }

        private string FormatTime() {
            TimeSpan t = TimeSpan.FromSeconds(Time);
            return t.ToString(@"hh\:mm\:ss");
        }
        public static Tour CreateExampleTour() {
            Random rnd = new Random();
            int num = rnd.Next(0, 100);
            string desc = string.Join(",", Faker.Lorem.Paragraphs(3) );
            string from = $"{Faker.Address.StreetAddress()}, {Faker.Address.City()}, {Faker.Address.Country()}"; 
            string to = $"{Faker.Address.StreetAddress()}, {Faker.Address.City()}, {Faker.Address.Country()}";
            TransportType trt = (TransportType) rnd.Next(0, System.Enum.GetNames(typeof(TransportType)).Length);
            return new Tour { Id = new Guid(), Name = $"Example Tour {num}", Description = desc, From = from, To = to, TransportType = trt, ChildFriendly = true, Distance = rnd.NextDouble() * rnd.Next(5, 100), Time = rnd.Next(0, 5000) };
        }
    }
}
