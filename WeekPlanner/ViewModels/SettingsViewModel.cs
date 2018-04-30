using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using WeekPlanner.Helpers;
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.Services.Settings;
using WeekPlanner.Validations;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;

namespace WeekPlanner.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ILoginService _loginService;

        public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService, ILoginService loginService) : base(navigationService)
        {
            _settingsService = settingsService;
            _loginService = loginService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is UserNameDTO userNameDTO)
            {
                if(_settingsService.CitizenAuthToken == null && _settingsService.GuardianAuthToken == null)
                {
                    await _loginService.LoginAsync(UserType.Citizen,
                    userNameDTO.UserName);
                }
            }
            else
            {
                throw new ArgumentException("Must be of type userNameDTO", nameof(navigationData));
            }
        }

        private int _shownDays = 7;
        public int NumberOfShownDaysAtOnce
        {
            get => _shownDays;
            set
            {
                if (value < 1)
                {
                    _shownDays = 1;
                }
                else if (value > 7)
                {
                    _shownDays = 7;
                }
                else
                {
                    _shownDays = value;
                }
                _settingsService.UserOptions.AppGridSizeColumns = _shownDays;
                RaisePropertyChanged(() => NumberOfShownDaysAtOnce);
            }
        }
        private int _activitiesshown;
        public string Activitiesshown
        {
            get => _activitiesshown.ToString();
            set
            {
                string pat = @"/\d/g";
                if (Regex.IsMatch(value, pat, RegexOptions.IgnoreCase))
                {
                    int.TryParse(value, out int temp);
                    if (temp < 1)
                    {
                        _activitiesshown = 1;
                    }
                    else
                    {
                        _activitiesshown = temp;
                    }
                    _settingsService.UserOptions.ActivitiesCount = _activitiesshown;
                    RaisePropertyChanged(() => Activitiesshown);
                }
                else
                {
                    MessagingCenter.Send(this, "Bogstaver og specialtegn genkendes ikke, indtast venligst et tal");
                }
            }
        }
    }
}
