using System;
using System.Collections.ObjectModel;
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
		private readonly IRequestService _requestService;
		private readonly IUserApi _userApi;

		private GirafUserDTO _girafCitizen;
		public GirafUserDTO GirafCitizen
		{
			get => _girafCitizen;
			set
			{
				_girafCitizen = value;
				RaisePropertyChanged(() => GirafCitizen);
			}
		}

		private bool _orientationSlider;
		public bool OrientationSwitch
		{
			get	=> _orientationSlider; 
			set
			{
				if (OrientationSetting == LauncherOptionsDTO.OrientationEnum.Portrait)
				{
					_orientationSlider = true;
				}
				else
				{
					_orientationSlider = false;
				}
				RaisePropertyChanged(() => OrientationSwitch);
			}
		}

		public ICommand HandleSwitchChangedCommand => new Command(() =>
		{
			if (OrientationSetting == LauncherOptionsDTO.OrientationEnum.Portrait)
			{
				OrientationSetting = LauncherOptionsDTO.OrientationEnum.Landscape;
			}
			else
			{
				OrientationSetting = LauncherOptionsDTO.OrientationEnum.Portrait;
			}
		});


		private LauncherOptionsDTO.OrientationEnum _orientationSetting;
		public LauncherOptionsDTO.OrientationEnum OrientationSetting
		{
			get => _orientationSetting;
			set
			{
				_orientationSetting = value;
				RaisePropertyChanged(() => OrientationSetting);
			}
		}



		public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService, ILoginService loginService, IRequestService requestService, IUserApi userApi) : base(navigationService)
        {
            _settingsService = settingsService;
            _loginService = loginService;
			_requestService = requestService;
			_userApi = userApi;

        }

        public override async Task InitializeAsync(object navigationData)
        {
			if (navigationData is UserNameDTO userNameDTO)
			{
				await _loginService.LoginAndThenAsync(InitializeCitizen, UserType.Citizen,
					userNameDTO.UserName);
			}
			else
			{
				throw new ArgumentException("Must be of type userNameDTO", nameof(navigationData));
			}
		}

		private async Task InitializeCitizen()
		{
			await _requestService.SendRequestAndThenAsync(this,
				requestAsync: async () => await _userApi.V1UserGetAsync(),
				onSuccess: result => 
				{
					GirafCitizen = result.Data;
					InitalizeSettings();
				},
				onExceptionAsync: async () => await NavigationService.PopAsync(),
				onRequestFailedAsync: async () => await NavigationService.PopAsync());
		}

		private async Task InitalizeSettings()
		{
			await _requestService.SendRequestAndThenAsync(this,
				requestAsync: async () => await _userApi.V1UserSettingsGetAsync(),
				onSuccess: result =>
				{
					SetSettings(result);
				},
				onExceptionAsync: async () => await NavigationService.PopAsync(),
				onRequestFailedAsync: async () => await NavigationService.PopAsync());
		}

		private void SetSettings(ResponseLauncherOptionsDTO result)
		{
			OrientationSetting = result.Data.Orientation;

			// Set all the other settings here.
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
    }
        }
            }
                }
                {
                else
                }
                    RaisePropertyChanged(() => Activitiesshown);
                    MessagingCenter.Send(this, "Bogstaver og specialtegn genkendes ikke, indtast venligst et tal");
}
