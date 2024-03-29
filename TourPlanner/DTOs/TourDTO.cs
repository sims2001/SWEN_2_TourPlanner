﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DTOs {
    public class TourDTO {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public TransportType TransportType { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public string PicturePath { get; set; }
        public virtual ICollection<LogDTO> LogDTOs { get; set; }
        
    }
}
