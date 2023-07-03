using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Stores {
    public class LanguageStore {

        private string? _currentLanguage;

        public string? CurrentLanguage {
            get => _currentLanguage;
            set {
                _currentLanguage = value;
            }
        }
    }
}
