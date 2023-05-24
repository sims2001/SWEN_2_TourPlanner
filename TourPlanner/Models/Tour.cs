using Faker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace TourPlanner.Models {
    public record Tour {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public string From { get; set; } = String.Empty;
        public string To { get; set; } = String.Empty;
        public TransportType TransportType { get; set; }
        public double Distance { get; set; }    
        public int Time { get; set; }
        public string PicturePath { get; set; } = String.Empty;
        public string FormatedTime { get => FormatTime(); } 
        public bool ChildFriendly { get => IsChildFriendly(); }

        public Popularity Popularity { get => AverageLogPopularity(); }
        public IEnumerable<TourLog>? TourLogs { get; set; } = Enumerable.Empty<TourLog>();
        //public IEnumerable<String?> ErrorMessages { get; set; }


        private int _logCount => TourLogs?.Count<TourLog>() ?? 0;
        private string FormatTime() {
            TimeSpan t = TimeSpan.FromSeconds(Time);
            return t.ToString(@"hh\:mm\:ss");
        }

        private bool IsChildFriendly() {
            if (Time > 2700) return false;

            if(AverageLogTime() > 2700)  return false;

            if (Distance > 5) return false;  

            if(AverageLogDifficulty().CompareTo(Difficulty.Medium) > 0) return false;

            return true;
        }

        private int AverageLogTime() {
            if(_logCount == 0)
                return Time;

            int sum = 0;
            foreach(var item in TourLogs) {
                sum += item.TotalTime;
            }

            return (int) sum/_logCount;
        }

        private Popularity AverageLogPopularity() {
            if (_logCount == 0)
                return Popularity.Good;

            int sum = 0;
            foreach(var item in TourLogs) {
                sum += (int) item.Rating;
            }

            return (Popularity)(sum / _logCount);
        }

        public Difficulty AverageLogDifficulty() {
            if (_logCount == 0)
                return Difficulty.Medium;

            int sum = 0;
            foreach(var item in TourLogs) {
                sum += (int)item.Difficulty;
            }

            return (Difficulty)(sum / _logCount);
        }

        public static Tour CreateExampleTour() {
            Random rnd = new Random();
            int num = rnd.Next(0, 100);
            string desc = string.Join(",", Faker.Lorem.Paragraphs(3) );
            string from = $"{Faker.Address.StreetAddress()}, {Faker.Address.City()}, {Faker.Address.Country()}"; 
            string to = $"{Faker.Address.StreetAddress()}, {Faker.Address.City()}, {Faker.Address.Country()}";
            TransportType trt = (TransportType) rnd.Next(0, System.Enum.GetNames(typeof(TransportType)).Length);
            return new Tour { Id = new Guid(), Name = $"Example Tour {num}", Description = desc, From = from, To = to, TransportType = trt, Distance = rnd.NextDouble() * rnd.Next(5, 100), Time = rnd.Next(0, 5000), PicturePath = "C:\\Users\\Simon\\Desktop\\Meme Shit\\alex_zaun.png" };
        }
    }
}
