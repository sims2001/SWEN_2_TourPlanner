using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TourPlanner.Models;
using TourPlanner.Stores;

namespace TourPlanner.ViewModels
{
    public class TourViewModel : ViewModelBase
    {
        private readonly Tour _tour;
        public Guid Id => _tour.Id;
        public string Name => _tour.Name;
        public string? Description => _tour.Description;
        public string From => _tour.From;
        public string To => _tour.To;
        public double Distance => _tour.Distance;
        public string FormatedTime => _tour.FormatedTime;
        public string FormatedAverageTime => _tour.FormatedAverageTime;
        public string PicturePath => _tour.PicturePath;
        public TransportType TransportType => _tour.TransportType;

        public string ChildFriendly => _tour.ChildFriendly ? "This Tour is Child Friendly" : "This Tour is not Child Friendly";
        public bool IsChildFriendly => _tour.ChildFriendly;
        public Popularity Popularity => _tour.Popularity;
        public Difficulty Difficulty => _tour.Difficulty;
        public IEnumerable<TourLog> Logs => _tour.Logs;

        public string Searchstring => _tour.SearchString;

        private bool _visible = true;

        public bool Visible {
            get => _visible;
            set {
                _visible = value;
                OnPropertyChanged();
            }
        }

        public TourViewModel(Tour tour) {
            _tour = tour;
        }

    }
}
