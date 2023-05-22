using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.ViewModels {
    class TourListViewModel : ViewModelBase {

        public Tour? _tour;
        public event EventHandler<Tour> SelectedTourChanged;
        public ObservableCollection<Tour> AllTours { get; set;  } = new();

        public Tour? Tour {
            get => _tour;
            set {
                _tour = value;
                OnPropertyChanged();
                OnSelectedTourChanged();
            }
        }

        private void OnSelectedTourChanged() {
            SelectedTourChanged?.Invoke(this, Tour);
        }

        public void GenerateTestTours() {
            //AllTours.Clear();
            ObservableCollection<Tour> touren = new ObservableCollection<Tour>();

            touren.Add(new Tour { Name = "Wienerwald", Description = "Example Beschreibung mit schönem Wetter" });
            touren.Add(new Tour { Name = "Mistelbacher Wald", Description = "Example Beschreibung mit schönem Wetter" });
            touren.Add(new Tour { Name = "Peters Gastgarten", Description = "Example Beschreibung mit schönem Wetter" });
            touren.Add(new Tour { Name = "Unsere DSGVO wurde aktualisi", Description = "Example Beschreibung mit schönem Wetter" });
            touren.Add(new Tour { Name = "jfdkasljlklödasfjljkadsflöjk", Description = "Example Beschreibung mit schönem Wetter" });

            AllTours = touren;
        }
    }
}
