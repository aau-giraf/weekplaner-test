using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IO.Swagger.Api;
using IO.Swagger.Model;
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.Services.Settings;
using WeekPlanner.ViewModels.Base;
using WeekPlanner.Views;
using Xamarin.Forms;
using static IO.Swagger.Model.WeekdayDTO;
using WeekPlanner.Services;

namespace WeekPlanner.ViewModels
{
    public class WeekPlannerViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IRequestService _requestService;
        private readonly IWeekApi _weekApi;
        private readonly IDialogService _dialogService;
        private readonly ISettingsService _settingsService;
        
        private bool _editModeEnabled;
        private WeekDTO _weekDto;
        private DayEnum _weekdayToAddPictogramTo;
        private ImageSource _userModeImage;

        public bool EditModeEnabled
        {
            get => _editModeEnabled;
            set
            {
                _editModeEnabled = value;
                RaisePropertyChanged(() => EditModeEnabled);
            }
        }

        public WeekDTO WeekDTO
        {
            get => _weekDto;
            set
            {
                _weekDto = value;
                RaisePropertyChanged(() => WeekDTO);
            }
        }

        public ImageSource UserModeImage
        {
            get => _userModeImage;
            set
            {
                _userModeImage = value;
                RaisePropertyChanged(() => UserModeImage);
            }
        }

        public ICommand ToggleEditModeCommand => new Command(async () => await SwitchUserModeAsync());

        public ICommand SaveCommand => new Command(async () => await SaveSchedule());

        public ICommand NavigateToPictoSearchCommand => new Command<DayEnum>(async weekday =>
        {
            _weekdayToAddPictogramTo = weekday;
            await NavigationService.NavigateToAsync<PictogramSearchViewModel>();
        });

        public ICommand PictoClickedCommand => new Command<ActivityDTO>(async activity =>
        {
            if (_editModeEnabled)
            {
                _selectedActivity = activity;
                await NavigationService.NavigateToAsync<ActivityViewModel>(activity);
            }
        });
                                                                    
        public WeekPlannerViewModel(
            INavigationService navigationService, 
            ILoginService loginService, 
            IRequestService requestService, 
            IWeekApi weekApi, 
            IDialogService dialogService, 
            ISettingsService settingsService) 
            : base(navigationService)
        {
            _requestService = requestService;
            _weekApi = weekApi;
            _dialogService = dialogService;
            _loginService = loginService;
            _settingsService = settingsService;

            UserModeImage = (FileImageSource)ImageSource.FromFile("icon_default_citizen.png");
            MessagingCenter.Subscribe<LoginViewModel>(this, MessageKeys.LoginSucceeded, (sender) => SetToGuardianMode());
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is UserNameDTO userNameDTO)
            {
                await _loginService.LoginAndThenAsync(GetWeekPlanForCitizenAsync, UserType.Citizen,
                    userNameDTO.UserName);
            }
            else if (navigationData is WeekDTO week)
            {
                WeekDTO = week;
                SetWeekdayPictos();
            }
            else
            {
                throw new ArgumentException("Must be of type userNameDTO", nameof(navigationData));
            }
        }

        // TODO: Handle situation where no days exist
        private async Task GetWeekPlanForCitizenAsync()
        {
            // TODO: Make dynamic regarding weekId
            await _requestService.SendRequestAndThenAsync(this,
                requestAsync: () => _weekApi.V1WeekByIdGetAsync(1),
                onSuccessAsync: async result =>
                {
                    WeekDTO = result.Data;
                    SetWeekdayPictos();
                },
                onExceptionAsync: async () => await NavigationService.PopAsync(),
                onRequestFailedAsync: async () => await NavigationService.PopAsync()
            );
        }

        private void InsertPicto(PictogramDTO pictogramDTO)
        {
            string imgSource =
                GlobalSettings.DefaultEndpoint + pictogramDTO.ImageUrl;
            WeekdayPictos[_weekdayToAddPictogramTo].Add(imgSource);

            var dayToAddTo = WeekDTO.Days.FirstOrDefault(d => d.Day == _weekdayToAddPictogramTo);
            if (dayToAddTo != null)
            {
                // Insert pictogram in the very bottom of the day
                var newOrderInBottom = dayToAddTo.Activities.Max(d => d.Order) + 1;
                dayToAddTo.Activities.Add(new ActivityDTO(pictogramDTO, newOrderInBottom));
            }
            // TODO: Fix
            RaisePropertyForDays();
        }


        private async Task SaveSchedule()
        {
            bool confirmed = await _dialogService.ConfirmAsync(
                title: "Gem ugeplan",
                message: "Vil du gemme ugeplanen?",
                okText: "Gem",
                cancelText: "Annuller");

            if (!confirmed)
            {
                return;
            }
            
            _settingsService.UseTokenFor(UserType.Citizen);
            
            if (WeekDTO.Id is null)
            {
                await SaveNewSchedule();
            }
            else
            {
                await UpdateExistingSchedule();
            }
        }

        private async Task SaveNewSchedule()
        {
            await _requestService.SendRequestAndThenAsync(this,
                async () => await _weekApi.V1WeekPostAsync(WeekDTO), result =>
                {
                    _dialogService.ShowAlertAsync(message: $"Ugeplanen '{result.Data.Name}' blev oprettet og gemt.");
                    EditModeEnabled = false;
                });
        }

        private async Task UpdateExistingSchedule()
        {
            if (WeekDTO.Id == null)
            {
                throw new InvalidDataException("WeekDTO should always have an Id when updating.");
            }

            await _requestService.SendRequestAndThenAsync(this,
                async () => await _weekApi.V1WeekByIdPutAsync((int)WeekDTO.Id, WeekDTO),
                result =>
                {
                    _dialogService.ShowAlertAsync(message: $"Ugeplanen '{result.Data.Name}' blev gemt.");
                    EditModeEnabled = false;
                });
        }

        private void SetWeekdayPictos()
        {
            var tempDict = new Dictionary<DayEnum, ObservableCollection<string>>();

            foreach (DayEnum day in Enum.GetValues(typeof(DayEnum)))
            {
                tempDict.Add(day, new ObservableCollection<string>());
            }

            foreach (WeekdayDTO dayDTO in WeekDTO.Days)
            {
                if (dayDTO.Day == null) continue;
                var weekday = dayDTO.Day.Value;
                ObservableCollection<string> pictos = new ObservableCollection<string>();
<<<<<<< HEAD
                foreach (var eleID in day.Elements)
                {
                    pictos.Add(
                        GlobalSettings.DefaultEndpoint + $"/v1/pictogram/{eleID.Id}/image/raw");
=======
                foreach (var activity in dayDTO.Activities.OrderBy(d => d.Order))
                {
                    pictos.Add(
                        GlobalSettings.DefaultEndpoint + activity.Pictogram.ImageUrl);
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
                }
                tempDict[weekday] = pictos;
            }

            WeekdayPictos = tempDict;
        }

        public override async Task PoppedAsync(object navigationData)
        {
            // Happens after choosing a pictogram in Pictosearch
            if (navigationData is PictogramDTO pictogramDTO)
            {
                InsertPicto(pictogramDTO);
            }

            // Happens when popping from ActivityViewModel
            if(navigationData is ActivityViewModel activityVM) {
                
                if(activityVM.Activity == null) {
                    // TODO this is dumb and it may remove duplicate elements since all elements are not unique
                    // we can refactor when ActivityDTO gets a unique ID
                    RemoveItemFromDay(DayEnum.Monday);
                    RemoveItemFromDay(DayEnum.Tuesday);
                    RemoveItemFromDay(DayEnum.Wednesday);
                    RemoveItemFromDay(DayEnum.Thursday);
                    RemoveItemFromDay(DayEnum.Friday);
                    RemoveItemFromDay(DayEnum.Saturday);
                    RemoveItemFromDay(DayEnum.Sunday);
                } else {
                    _selectedActivity = activityVM.Activity;
                }

                RaisePropertyForDays();
            }

            // Happens after logging in as guardian when switching to guardian mode
            if (navigationData is bool enterGuardianMode)
            {
                SetToGuardianMode();
            }
        }

        private bool RemoveItemFromDay(DayEnum day) {
            var a = WeekDTO.Days.First(x => x.Day == day).Activities;
            if (a.Count == 0) {
                return false;
            }
            return a.Remove(_selectedActivity);
        }

        private async Task SwitchUserModeAsync()
        {
            if (EditModeEnabled)
            {
                EditModeEnabled = false;
                UserModeImage = (FileImageSource)ImageSource.FromFile("icon_default_citizen.png");
            }
            else
            {
                await NavigationService.NavigateToAsync<LoginViewModel>(this);
            }
        }

        private void SetToGuardianMode()
        {
            EditModeEnabled = true;
            UserModeImage = (FileImageSource)ImageSource.FromFile("icon_default_guardian.png");
            
            // Needs to set pictos again due to? Perhaps garbage collection??
            SetWeekdayPictos();
        }

        private Dictionary<DayEnum, ObservableCollection<string>> _weekdayPictos =
            new Dictionary<DayEnum, ObservableCollection<string>>();
        private ActivityDTO _selectedActivity;

        public Dictionary<DayEnum, ObservableCollection<string>> WeekdayPictos
        {
            get => _weekdayPictos;
            set
            {
                _weekdayPictos = value;
                RaisePropertyForDays();
            }
        }

        public ObservableCollection<ActivityDTO> MondayPictos => GetPictosOrEmptyList(DayEnum.Monday);
        public ObservableCollection<ActivityDTO> TuesdayPictos => GetPictosOrEmptyList(DayEnum.Tuesday);
        public ObservableCollection<ActivityDTO> WednesdayPictos => GetPictosOrEmptyList(DayEnum.Wednesday);
        public ObservableCollection<ActivityDTO> ThursdayPictos => GetPictosOrEmptyList(DayEnum.Thursday);
        public ObservableCollection<ActivityDTO> FridayPictos => GetPictosOrEmptyList(DayEnum.Friday);
        public ObservableCollection<ActivityDTO> SaturdayPictos => GetPictosOrEmptyList(DayEnum.Saturday);
        public ObservableCollection<ActivityDTO> SundayPictos => GetPictosOrEmptyList(DayEnum.Sunday);

        private ObservableCollection<ActivityDTO> GetPictosOrEmptyList(DayEnum dayEnum)
        {
            var day = WeekDTO?.Days.FirstOrDefault(x => x.Day == dayEnum);
            if (day == null)
            {
                return new ObservableCollection<ActivityDTO>();
            }

            return day.Activities.ToObservableCollection();
        }

        private void SendRequestFailedMessage(
            ResponseWeekDTO.ErrorKeyEnum? errorKeyEnum = ResponseWeekDTO.ErrorKeyEnum.Error)
        {
            var friendlyErrorMessage = errorKeyEnum.ToFriendlyString();
            MessagingCenter.Send(this, MessageKeys.RequestFailed, friendlyErrorMessage);
        }

        private void RaisePropertyForDays() {
            RaisePropertyChanged(() => MondayPictos);
            RaisePropertyChanged(() => TuesdayPictos);
            RaisePropertyChanged(() => WednesdayPictos);
            RaisePropertyChanged(() => ThursdayPictos);
            RaisePropertyChanged(() => FridayPictos);
            RaisePropertyChanged(() => SaturdayPictos);
            RaisePropertyChanged(() => SundayPictos);
            RaisePropertyChanged(() => WeekdayPictos);
        }

    }

    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);

        }
    }
>>>>>>> release-2018S3R1
}
