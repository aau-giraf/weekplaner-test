using System;
using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;
using WeekPlanner.Services.Settings;
using WeekPlanner.Views;
using WeekPlanner.Services;
using WeekPlanner.Helpers;
using Xamarin.Forms;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace WeekPlanner.ViewModels
{
    public class ChoiceBoardViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly IDialogService _dialogService;
        private readonly IWeekApi _weekApi;
        private readonly ILoginService _loginService;
        private readonly ISettingsService _settingsService;

        private ObservableCollection<PictogramDTO> _pictograms;

        public ChoiceBoardViewModel(INavigationService navigationService, IRequestService requestService,
            IDialogService dialogService, IWeekApi weekApi, ILoginService loginService,
            ISettingsService settingsService) : base(navigationService)
        {
            _requestService = requestService;
            _dialogService = dialogService;
            _weekApi = weekApi;
            _loginService = loginService;
            _settingsService = settingsService;
        }


        public ObservableCollection<PictogramDTO> Pictograms
        {
            get => _pictograms;
            set;
        }


    }
}
