using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.Stores {
    public class LanguageStore {
        private readonly ILoggerWrapper _logger;

        public LanguageStore(IServiceProvider serviceProvider) {
            _logger = LoggerFactory.GetLogger(serviceProvider.GetService<IConfiguration>());
        }

        private string? _currentLanguage;

        public string? CurrentLanguage {
            get => _currentLanguage;
            set {
                _currentLanguage = value;
                _logger.Info($"Set Language to {_currentLanguage}");
            }
        }
    }
}
