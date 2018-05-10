using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IO.Swagger.Api;
using IO.Swagger.Model;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.Services.Settings;
using WeekPlanner.ViewModels.Base;
using WeekPlanner.Views;
using Xamarin.Forms;
using static IO.Swagger.Model.WeekdayDTO;
using WeekPlanner.Services;
using WeekPlanner.Helpers;
using static IO.Swagger.Model.ActivityDTO;

namespace WeekPlanner.ViewModels
{
    public class WeekPlannerViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly IWeekApi _weekApi;
        private readonly IDialogService _dialogService;
        private readonly IPictogramApi _pictogramApi;
        public ISettingsService SettingsService { get; }
        private bool _isDirty = false;

        private ActivityDTO _selectedActivity;
        private WeekDTO _weekDto;
        private DayEnum _weekdayToAddPictogramTo;
        private ImageSource _toolbarButtonIcon;
        private WeekDTO _weekForChoiceBoards = new WeekDTO()
        {
            Days = new List<WeekdayDTO>()
            {
                new WeekdayDTO(DayEnum.Monday, new List<ActivityDTO>()),
                new WeekdayDTO(DayEnum.Tuesday, new List<ActivityDTO>()),
                new WeekdayDTO(DayEnum.Wednesday, new List<ActivityDTO>()),
                new WeekdayDTO(DayEnum.Thursday, new List<ActivityDTO>()),
                new WeekdayDTO(DayEnum.Friday, new List<ActivityDTO>()),
                new WeekdayDTO(DayEnum.Saturday, new List<ActivityDTO>()),
                new WeekdayDTO(DayEnum.Sunday, new List<ActivityDTO>())
            },
            
        };
        private WeekPictogramDTO _standardChoiceBoardPictoDTO;
        public WeekDTO WeekDTO
        {
            get => _weekDto;
            set
            {
                _weekDto = value;
                RaisePropertyChanged(() => WeekDTO);
                RaisePropertyForDays();
            }
        }

        public ImageSource ToolbarButtonIcon
        {
            get => _toolbarButtonIcon;
            set
            {
                _toolbarButtonIcon = value;
                RaisePropertyChanged(() => ToolbarButtonIcon);
            }
        }
        
        public double PictoSize { get; } = Device.Idiom == TargetIdiom.Phone ? 100 : 150;

        public bool ShowToolbarButton { get; set; }

        public ICommand ToolbarButtonCommand => new Command(async () => await SwitchUserModeAsync());

        public ICommand SaveCommand => new Command(async () => await SaveSchedule());
        public ICommand NavigateToPictoSearchCommand => new Command<DayEnum>(async weekday =>
        {
            if (IsBusy) return;
            IsBusy = true;
            _weekdayToAddPictogramTo = weekday;
            await NavigationService.NavigateToAsync<PictogramSearchViewModel>();
            IsBusy = false;
        });

        public ICommand PictoClickedCommand => new Command<ActivityDTO>(async activity =>
        {
            if (IsBusy) return;
            IsBusy = true;
            _selectedActivity = activity;
            if (_selectedActivity.IsChoiceBoard)
            {
                List<ActivityDTO> activityDTOs = GetActivitiesForChoiceBoard();
                await NavigationService.NavigateToAsync<ChoiceBoardViewModel>(activityDTOs);
            }
            else
            {
                await NavigationService.NavigateToAsync<ActivityViewModel>(activity);
            }
            
            IsBusy = false;
        });

        public WeekPlannerViewModel(
            INavigationService navigationService,
            IRequestService requestService,
            IWeekApi weekApi,
            IDialogService dialogService,
            ISettingsService settingsService,
            IPictogramApi pictogramApi)
            : base(navigationService)
        {
            _requestService = requestService;
            _weekApi = weekApi;
            _dialogService = dialogService;
            _requestService = requestService;
            _pictogramApi = pictogramApi;
            SettingsService = settingsService;

            OnBackButtonPressedCommand = new Command(async () => await BackButtonPressed());
            ShowToolbarButton = true;
            ToolbarButtonIcon = (FileImageSource)ImageSource.FromFile("icon_default_guardian.png");
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is Tuple<int?, int?> weekYearAndNumber)
            {
                _standardChoiceBoardPictoDTO = PictoToWeekPictoDtoHelper.Convert(_pictogramApi.V1PictogramByIdGet(2).Data);
                await GetWeekPlanForCitizenAsync(weekYearAndNumber);
            }
            else
            {
                throw new ArgumentException("Must be of type userNameDTO", nameof(navigationData));
            }
        }

        // TODO: Handle situation where no days exist
        private async Task GetWeekPlanForCitizenAsync(Tuple<int?, int?> weekYearAndNumber)
        {
            SettingsService.UseTokenFor(UserType.Citizen);

            await _requestService.SendRequestAndThenAsync(
                requestAsync: () =>
                    _weekApi.V1WeekByWeekYearByWeekNumberGetAsync(weekYearAndNumber.Item1, weekYearAndNumber.Item2),
                onSuccess: result => { 
                    WeekDTO = result.Data;
                }
            );

            foreach (var days in WeekDTO.Days)
            {
                DayEnum? day = days.Day;
                List<int?> orderOfChoiceBoards = new List<int?>();

                foreach (var a in days.Activities.GroupBy(d => d.Order, d => d))
                {
                    if (a.Count() > 1)
                    {
                        foreach (var item in a)
                        {
                            _weekForChoiceBoards.Days.First(d => d.Day == day).Activities.Add(item);
                        }
                        orderOfChoiceBoards.Add(a.Key);
                    }
                }

                foreach (var item in orderOfChoiceBoards)
                {
                    days.Activities.RemoveAll(d => d.Order == item);
                    days.Activities.Add(new ActivityDTO(_standardChoiceBoardPictoDTO, item, StateEnum.Normal) { IsChoiceBoard = true});
                }
            }

            foreach (var days in WeekDTO.Days)
            {
                days.Activities = days.Activities.OrderBy(a => a.Order).ToList();
            }
        }

        private void InsertPicto(WeekPictogramDTO pictogramDTO)
        {
            var dayToAddTo = WeekDTO.Days.FirstOrDefault(d => d.Day == _weekdayToAddPictogramTo);
            if (dayToAddTo != null)
            {
                // Insert pictogram in the very bottom of the day
                var newOrderInBottom = dayToAddTo.Activities.Max(d => d.Order) + 1;
                dayToAddTo.Activities.Add(new ActivityDTO(pictogramDTO, newOrderInBottom, StateEnum.Normal));
                _isDirty = true;
                RaisePropertyForDays();
            }
        }

        private List<ActivityDTO> GetActivitiesForChoiceBoard()
        {
            List<ActivityDTO> activities = new List<ActivityDTO>();

            DayEnum? day = WeekDTO.Days.First(d => d.Activities.Contains(_selectedActivity)).Day;

            foreach (var item in _weekForChoiceBoards.Days.Single(d => d.Day == day).Activities.Where(a => a.Order == _selectedActivity.Order))
            {
                activities.Add(item);
            }

            return activities;
        }

        private void DeleteChoiceBoardAsync()
        {
            DayEnum? day = WeekDTO.Days.First(d => d.Activities.Contains(_selectedActivity)).Day;
            int? order = _selectedActivity.Order;

            _weekForChoiceBoards.Days.Single(d => d.Day == day).Activities.RemoveAll(a => a.Order == order);
        }


        private async Task SaveSchedule()
        {
            if (IsBusy) return;
            if(!_isDirty) return;

            IsBusy = true;
            bool confirmed = await _dialogService.ConfirmAsync(
                title: "Gem ugeplan",
                message: "Vil du gemme ugeplanen?",
                okText: "Gem",
                cancelText: "Annuller");

            if (!confirmed)
            {
                IsBusy = false;
                return;
            }

            SettingsService.UseTokenFor(UserType.Citizen);

            await SaveOrUpdateSchedule();

            IsBusy = false;
        }

        private async Task SaveNewSchedule()
        {
            await _requestService.SendRequestAndThenAsync(
                () => _weekApi.V1WeekByWeekYearByWeekNumberPutAsync(weekYear: WeekDTO.WeekYear,
                    weekNumber: WeekDTO.WeekNumber, newWeek: WeekDTO),
                result =>
                {
                    _dialogService.ShowAlertAsync(message: $"Ugeplanen '{result.Data.Name}' blev oprettet og gemt.");
                    _isDirty = false;
                });
        }

        private async Task UpdateExistingSchedule()
        {
            if (WeekDTO.WeekNumber == null)
            {
                throw new InvalidDataException("WeekDTO should always have an Id when updating.");
            }

            await _requestService.SendRequestAndThenAsync(
                () => _weekApi.V1WeekByWeekYearByWeekNumberPutAsync(WeekDTO.WeekYear, WeekDTO.WeekNumber, WeekDTO),
                result => { 
                    _dialogService.ShowAlertAsync(message: $"Ugeplanen '{result.Data.Name}' blev gemt.");
                    _isDirty = false;
                 });
        }

        public override async Task PoppedAsync(object navigationData)
        {
            // Happens after choosing a pictogram in Pictosearch
            if (navigationData is PictogramDTO pictogramDTO)
            {
                WeekPictogramDTO weekPictogramDTO = PictoToWeekPictoDtoHelper.Convert(pictogramDTO);
                InsertPicto(weekPictogramDTO);
            }

            // Happens when popping from ActivityViewModel
            if (navigationData == null)
            {
                WeekDTO.Days.First(d => d.Activities.Contains(_selectedActivity))
                    .Activities
                    .Remove(_selectedActivity);
                _isDirty = true;
                RaisePropertyForDays();
                if (!SettingsService.IsInGuardianMode)
                {
                    await UpdateExistingSchedule();
                }
            }
            else if (navigationData is ActivityDTO activity)
            {
                _selectedActivity = activity;
                _isDirty = true;
                RaisePropertyForDays();
                if (!SettingsService.IsInGuardianMode)
                {
                    await UpdateExistingSchedule();
                }
            } else if (navigationData is ObservableCollection<ActivityDTO> activities)
            {
                await HandleChoiceBoardAsync(activities);
            }
            // Happens after logging in as guardian when switching to guardian mode
            if (navigationData is bool enterGuardianMode)
            {
                SetToGuardianMode();
            }
        }

        private async Task HandleChoiceBoardAsync(ObservableCollection<ActivityDTO> activities)
        {
            if (activities.Count == 0)
            {
                DeleteChoiceBoardAsync();
                await InitializeAsync(null);
            }
            else if (activities.Count == 1)
            {
                DeleteChoiceBoardAsync();
                await InitializeAsync(activities.First());
            }
            else
            {
                int? order = activities.First().Order;
                DayEnum? dayEnum = WeekDTO.Days.First(d => d.Activities.Contains(_selectedActivity)).Day;

                _weekForChoiceBoards.Days.Single(d => d.Day == dayEnum).Activities.RemoveAll(a => a.Order == order);

                foreach (var item in activities)
                {
                    _weekForChoiceBoards.Days.Single(d => d.Day == dayEnum).Activities.Add(item);
                }
                WeekDTO.Days.First(d => d.Activities.Contains(_selectedActivity)).Activities.Remove(_selectedActivity);
                WeekDTO.Days.Single(d => d.Day == dayEnum).Activities.Add(new ActivityDTO(_standardChoiceBoardPictoDTO, order, StateEnum.Normal) { IsChoiceBoard = true});
                
                _isDirty = true;
                RaisePropertyForDays();
            }
        }

        private async Task SwitchUserModeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            if (SettingsService.IsInGuardianMode)
            {
                if(!_isDirty) {
                    SetToCitizenMode();
                    IsBusy = false;
                    return;
                }
                var result = await _dialogService.ActionSheetAsync("Der er ændringer der ikke er gemt. Vil du gemme?",
                    "Annuller", null, "Gem ændringer", "Gem ikke");

                switch (result)
                {
                    case "Annuller":
                        break;

                    case "Gem ændringer":
                        await SaveOrUpdateSchedule();
                        SetToCitizenMode();
                        break;

                    case "Gem ikke":
                        if (WeekDTO.WeekNumber != null)
                            await GetWeekPlanForCitizenAsync(new Tuple<int?, int?>(WeekDTO.WeekYear, WeekDTO.WeekNumber));
                        SetToCitizenMode();
                        break;
                }
            }
            else
            {
                await NavigationService.NavigateToAsync<LoginViewModel>(this);
            }

            IsBusy = false;
        }

        public int Height
        {
            get
            {
                int minimumHeight = 1500;
                int elementHeight = 250;

                if (WeekDTO == null)
                {
                    return minimumHeight;
                }

                int dynamicHeight = WeekDTO.Days.Max(d => d.Activities.Count) * elementHeight;

                return dynamicHeight > minimumHeight ? dynamicHeight : minimumHeight;
            }
        }

        private void SetToCitizenMode()
        {
            ShowBackButton = false;
            SettingsService.IsInGuardianMode = false;
            ToolbarButtonIcon = (FileImageSource)ImageSource.FromFile("icon_default_citizen.png");
        }

        private void SetToGuardianMode()
        {
            ShowBackButton = true;
            SettingsService.IsInGuardianMode = true;
            ToolbarButtonIcon = (FileImageSource)ImageSource.FromFile("icon_default_guardian.png");
        }

        private async Task BackButtonPressed()
        {
            if (IsBusy) return;
            if(!_isDirty) {
                await NavigationService.PopAsync();
                return;
            }
            if (!SettingsService.IsInGuardianMode){
                return;
            }
            IsBusy = true;
            var result = await _dialogService.ActionSheetAsync("Der er ændringer der ikke er gemt. Vil du gemme?",
                "Annuller", null, "Gem ændringer", "Gem ikke");

            switch (result)
            {
                case "Annuller":
                    break;
                case "Gem ændringer":
                    await SaveOrUpdateSchedule();
                    await NavigationService.PopAsync();
                    break;
                case "Gem ikke":
                    await NavigationService.PopAsync();
                    break;
            }

            IsBusy = false;
        }

        private DayEnum GetCurrentDay()
        {
            var today = DateTime.Today.DayOfWeek;
            switch (today)
            {
                case DayOfWeek.Monday:
                    return DayEnum.Monday;
                case DayOfWeek.Tuesday:
                    return DayEnum.Tuesday;
                case DayOfWeek.Wednesday:
                    return DayEnum.Wednesday;
                case DayOfWeek.Thursday:
                    return DayEnum.Thursday;
                case DayOfWeek.Friday:
                    return DayEnum.Friday;
                case DayOfWeek.Saturday:
                    return DayEnum.Saturday;
                case DayOfWeek.Sunday:
                    return DayEnum.Sunday;
                default:
                    throw new NotSupportedException("DayEnum out of bounds");
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

            return new ObservableCollection<ActivityDTO>(day.Activities);
        }


        private void RaisePropertyForDays()
        {
            SetActiveActivity();
            RaisePropertyChanged(() => MondayPictos);
            RaisePropertyChanged(() => TuesdayPictos);
            RaisePropertyChanged(() => WednesdayPictos);
            RaisePropertyChanged(() => ThursdayPictos);
            RaisePropertyChanged(() => FridayPictos);
            RaisePropertyChanged(() => SaturdayPictos);
            RaisePropertyChanged(() => SundayPictos);
            RaisePropertyChanged(() => Height);
        }

        private void SetActiveActivity()
        {
            var today = GetCurrentDay();
            var todaysActivities = WeekDTO.Days.First(d => d.Day == today).Activities;

            foreach (var activity in todaysActivities)
            {
                if (activity.State == StateEnum.Normal)
                {
                    activity.State = StateEnum.Active;
                    return;
                }
            }
        }

        private async Task SaveOrUpdateSchedule()
        {
            PutChoiceActivitiesBackIntoSchedule();
            if (WeekDTO.WeekNumber is null)
            {
                await SaveNewSchedule();
            }
            else
            {
                await UpdateExistingSchedule();
            }
        }

        private void PutChoiceActivitiesBackIntoSchedule()
        {
            foreach (var day in _weekForChoiceBoards.Days)
            {
                foreach (var item in day.Activities)
                {
                    WeekDTO.Days.Single(d => d.Day == day.Day).Activities.Add(item);
                }
            }

            foreach (var day in WeekDTO.Days)
            {
                day.Activities.RemoveAll(a => a.IsChoiceBoard);
            }
        }
    }
}