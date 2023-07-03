using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Commands;
using TourPlanner.Services;

namespace TourPlanner.ViewModels {
    public class BackToMenuViewModel : ViewModelBase {

        private readonly LanguageService _languageService;

        public BackToMenuViewModel(IServiceProvider serviceProvider) {
            _languageService = serviceProvider.GetRequiredService<LanguageService>();

            ToOverViewCommand = new NavigateCommand<TourOverViewModel>(
                serviceProvider.GetService<INavigationService<TourOverViewModel>>(),
                serviceProvider
            );
        }

        public ICommand ToOverViewCommand { get; }

        public string ToMenuLabel => _languageService.getVariable("button_back_to_overview");
    }
}
