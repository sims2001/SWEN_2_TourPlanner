using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services {
    public static class PersonalMapper {

        public static Tour TourFromDTO(TourDTO t) {
            var ll = new List<TourLog>();

            foreach (var l in t.LogDTOs) {
                ll.Add(PersonalMapper.LogFromDTO(l));
            }

            var tour = new Tour() {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                From = t.From,
                To = t.To,
                TransportType = t.TransportType,
                Distance = t.Distance,
                Time = t.Time,
                PicturePath = t.PicturePath,
                Logs = ll
            };

            foreach (var l in tour.Logs) {
                l.Tour = tour;
            }

            return tour;
        }

        public static TourLog LogFromDTO(LogDTO logDto) {
            return new TourLog() {
                Id = logDto.Id,
                Comment = logDto.Comment,
                Date = logDto.Date,
                Difficulty = logDto.Difficulty,
                TotalTime = logDto.TotalTime,
                Rating = logDto.Rating,
                TourId = logDto.TourId,
            };
        }

        public static TourDTO DTOFromTour(Tour t) {
            var ll = new List<LogDTO>();

            foreach (var l in t.Logs) {
                ll.Add(PersonalMapper.DTOFromLog(l));
            }

            var dto = new TourDTO() {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                From = t.From,
                To = t.To,
                TransportType = t.TransportType,
                Distance = t.Distance,
                Time = t.Time,
                PicturePath = t.PicturePath,
                LogDTOs = ll
            };

            foreach (var l in dto.LogDTOs) {
                l.TourDTO = dto;
            }

            return dto;
        }

        public static LogDTO DTOFromLog(TourLog tourLog) {
            return new LogDTO() {
                Id = tourLog.Id,
                Comment = tourLog.Comment,
                Date = tourLog.Date,
                Difficulty = tourLog.Difficulty,
                TotalTime = tourLog.TotalTime,
                Rating = tourLog.Rating,
                TourId = tourLog.TourId,
            };
        }
    }
}
