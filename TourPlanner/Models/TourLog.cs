using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models {
    public class TourLog {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public int TotalTime { get; set; }
        public Popularity Rating { get; set; }
        [ForeignKey("TourId")]
        public Guid TourId { get; set; }
    }
}
