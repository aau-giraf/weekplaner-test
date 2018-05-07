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
<<<<<<< HEAD
=======
using WeekPlanner.Services;
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63

namespace WeekPlanner.ViewModels
{
    public class CitizenSchedulesViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
<<<<<<< HEAD
        private readonly IWeekApi _weekApi;
        private readonly ILoginService _loginService;
=======
        private readonly IDialogService _dialogService;
        private readonly IWeekApi _weekApi;
        private readonly ILoginService _loginService;
        private readonly ISettingsService _settingsService;
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63

        public ICommand WeekTappedCommand => new Command((tappedItem) => ListViewItemTapped((WeekDTO)tappedItem));
        public ICommand WeekDeletedCommand => new Command((x) => WeekDeletedTapped(x));
        public ICommand AddWeekScheduleCommand => new Command(() => AddWeekSchedule());

<<<<<<< HEAD
        public CitizenSchedulesViewModel(INavigationService navigationService, IRequestService requestService, IWeekApi weekApi, ILoginService loginService) : base(navigationService)
        {
            _requestService = requestService;
            _weekApi = weekApi;
            _loginService = loginService;
        }

        private ObservableCollection<WeekNameDTO> _namesAndID;
=======
        public CitizenSchedulesViewModel(INavigationService navigationService, IRequestService requestService, IDialogService dialogService, IWeekApi weekApi, ILoginService loginService, ISettingsService settingsService) : base(navigationService)
        {
            _requestService = requestService;
            _dialogService = dialogService;
            _weekApi = weekApi;
            _loginService = loginService;
            _settingsService = settingsService;

            MessagingCenter.Subscribe<NewScheduleViewModel>(this, "UpdateView", (sender) => UpdateWeekViewAsync());
        }

        private ObservableCollection<WeekNameDTO> _namesAndID = new ObservableCollection<WeekNameDTO>();
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
        public ObservableCollection<WeekNameDTO> NamesAndID
        {
            get => _namesAndID;
            set
            {
                _namesAndID = value;
                RaisePropertyChanged(() => NamesAndID);
            }
        }

        private ObservableCollection<WeekDTO> _weeks = new ObservableCollection<WeekDTO>();
        public ObservableCollection<WeekDTO> Weeks
        {
            get => _weeks;
            set
            {
                _weeks = value;
                RaisePropertyChanged(() => Weeks);
            }
        }

        private ObservableCollection<PictogramDTO> _weekImage;
        public ObservableCollection<PictogramDTO> WeekImage
        {
            get => _weekImage;
            set
            {
                _weekImage = value;
                RaisePropertyChanged(() => WeekImage);
            }
        }
<<<<<<< HEAD

=======
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
        private async void ListViewItemTapped(WeekDTO tappedItem)
        {
            await NavigationService.NavigateToAsync<WeekPlannerViewModel>(tappedItem);
        }

        public async Task InitializeWeekSchedules()
        {
            await _requestService.SendRequestAndThenAsync(this,
                requestAsync: async () => await _weekApi.V1WeekGetAsync(),
<<<<<<< HEAD
                onSuccess: result => { NamesAndID = new ObservableCollection<WeekNameDTO>(result.Data); },
                onExceptionAsync: async () => await NavigationService.PopAsync(),
                onRequestFailedAsync: async () => await NavigationService.PopAsync());

            foreach (var item in NamesAndID)
            {
                Weeks.Add(_weekApi.V1WeekByIdGet(item.Id).Data);
            }
        }

        private void WeekDeletedTapped(Object week)
        {
            if (week is WeekDTO weekDTO)
            {
                MessagingCenter.Send(this, "DeleteWeekAlert");

                //MessagingCenter.Subscribe<CitizenSchedulesPage>(this, "DeleteWeek", (sender) => DeleteWeek(sender, weekDTO));
            }
        }

        private void DeleteWeek(CitizenSchedulesPage sender, WeekDTO week)
        {
            _weekApi.V1WeekByIdDelete(week.Id);
=======
                onSuccess: result => { NamesAndID = new ObservableCollection<WeekNameDTO>(result.Data); });

            foreach (var item in NamesAndID)
            {
                await _requestService.SendRequestAndThenAsync(this,
                    async () => await _weekApi.V1WeekByIdGetAsync(item.Id), (res) => Weeks.Add(res.Data));
            }
        }
        private async Task WeekDeletedTapped(Object week)
        {
            if (week is WeekDTO weekDTO)
            { 
                await DeleteWeek(weekDTO);
            }
        }

        private async Task DeleteWeek(WeekDTO w)
        {
            var answer = await _dialogService.ConfirmAsync($"Vil du slette {w.Name}?", "Slet Ugeplan");
            if (answer)
            {
                await _requestService.SendRequestAndThenAsync(this,
                    requestAsync: () => _weekApi.V1WeekByIdDeleteAsync(w.Id), onSuccess: (r) => Weeks.Remove(w));
            }
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
        }

        private async void AddWeekSchedule()
        {
            await NavigationService.NavigateToAsync<NewScheduleViewModel>();
        }

<<<<<<< HEAD
=======
        private async void UpdateWeekViewAsync()
        {
            Weeks.Clear();
            await InitializeWeekSchedules();
        }
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is UserNameDTO userNameDTO)
            {
                await _loginService.LoginAndThenAsync(InitializeWeekSchedules, UserType.Citizen, userNameDTO.UserName);
            }
            else
            {
                throw new ArgumentException("Must be of type userNameDTO", nameof(navigationData));
            }
        }
    }
}
