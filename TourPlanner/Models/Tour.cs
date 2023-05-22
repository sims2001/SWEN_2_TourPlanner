using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TourPlanner.Models {
    public class Tour {
        //[JsonIgnore]
        //public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        //public string From { get; set; } = String.Empty;
        //public string To { get; set; } = String.Empty;
        //public TransportType TransportType { get; set; }
        //public double Distance { get; set; }    
        //public int Time { get; set; }
        //public string? FormatedTime { get; set; }
        //public string PicturePath { get; set; } = String.Empty;
        //public bool ChildFriendly { get; set; }
        //public IEnumerable<TourLog>? TourLogs { get; set; }
        //public IEnumerable<String?> ErrorMessages { get; set; }

    }
}
