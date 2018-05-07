using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IO.Swagger.Api;
using IO.Swagger.Model;
using WeekPlanner.Models;
using WeekPlanner.Services;
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.Services.Settings;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;

namespace WeekPlanner.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IRequestService _requestService;
        private readonly IUserApi _userApi;

        public WeekdayColors WeekdayColors { get; set; }

        private IEnumerable<SettingDTO.ThemeEnum> _themes = new List<SettingDTO.ThemeEnum>(){
            SettingDTO.ThemeEnum.AndroidBlue, SettingDTO.ThemeEnum.GirafGreen, SettingDTO.ThemeEnum.GirafRed, SettingDTO.ThemeEnum.GirafYellow
        };

        public SettingDTO.ThemeEnum ThemeSelected
        {
            get => _settingsService.CurrentCitizenSettingDTO.Theme;
            set
            {
                var currentTheme = Application.Current.Resources;
                switch (value)
                {
                    case SettingDTO.ThemeEnum.GirafRed:
                        currentTheme.MergedWith = typeof(Themes.RedTheme);
                        SetThemeInSettingDTOAndUpdate(value);
                        break;
                    case SettingDTO.ThemeEnum.GirafYellow:
                        currentTheme.MergedWith = typeof(Themes.OrangeTheme);
                        SetThemeInSettingDTOAndUpdate(value);
                        break;
                    case SettingDTO.ThemeEnum.AndroidBlue:
                        currentTheme.MergedWith = typeof(Themes.BlueTheme);
                        SetThemeInSettingDTOAndUpdate(value);
                        break;
                    case SettingDTO.ThemeEnum.GirafGreen:
                        currentTheme.MergedWith = typeof(Themes.GreenTheme);
                        SetThemeInSettingDTOAndUpdate(value);
                        break;
                    default:
                        break;
                }
                RaisePropertyChanged(() => ThemeSelected);
            }
        }

        private void SetThemeInSettingDTOAndUpdate(SettingDTO.ThemeEnum pickedTheme)
        {
            Settings.Theme = pickedTheme;
            UpdateSettingsAsync();
        }

        private bool _orientationSlider;

        public bool OrientationSwitch
        {
            get => _orientationSlider;
            set
            {
                if (Settings.Orientation == SettingDTO.OrientationEnum.Portrait)
                {
                    _orientationSlider = true;
                }
                else
                {
                    _orientationSlider = false;
                }

                RaisePropertyChanged(() => OrientationSwitch);
                UpdateSettingsAsync();
            }
        }

        public ICommand HandleSwitchChangedCommand => new Command(() =>
        {
            if (Settings.Orientation == SettingDTO.OrientationEnum.Portrait)
            {
                Settings.Orientation = SettingDTO.OrientationEnum.Landscape;
            }
            else
            {
                Settings.Orientation = SettingDTO.OrientationEnum.Portrait;
            }
        });


        private async Task UpdateSettingsAsync()
        {
            _settingsService.UseTokenFor(UserType.Citizen);
            await _requestService.SendRequest(_userApi.V1UserSettingsPatchAsync(Settings));
        }

        public SettingDTO Settings => _settingsService.CurrentCitizenSettingDTO;

        public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService, 
            IRequestService requestService, IUserApi userApi) : base(navigationService)
        {
            _settingsService = settingsService;
            _requestService = requestService;
            _userApi = userApi;

            WeekdayColors = new WeekdayColors(_settingsService.CurrentCitizenSettingDTO);
            // Update settings regardless of which property calls 'RaisePropertyChanged'
            WeekdayColors.PropertyChanged += (sender, e) => UpdateSettingsAsync();
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
                RaisePropertyChanged(() => LimitNumberofActivities);
                //Settings.NrOfDaysToDisplay = _shownDays; //Todo Find a way to add to the database without crashing
            }
        }
        public bool LimitNumberofActivities => (NumberOfShownDaysAtOnce == 1);
        private int _activitiesshown;
        public string ActivitiesShown
        {
            get => _activitiesshown.ToString();
            set
            {
                bool isdig = int.TryParse(value, out int temp);
                if (isdig)
                {

                    if (temp < 1)
                    {
                        _activitiesshown = 1;
                    }
                    else
                    {
                        _activitiesshown = temp;
                    }

                }
                else
                {
                    _activitiesshown = 30;
                }
                //Settings.ActivitiesCount = _activitiesshown; //Todo Find a way to add to the database without crashing
            }
        }
    }
}
