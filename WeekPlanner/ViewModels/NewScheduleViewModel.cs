using IO.Swagger.Api;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
<<<<<<< HEAD
=======
using WeekPlanner.Services;
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.Validations;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;

namespace WeekPlanner.ViewModels
{
    public class NewScheduleViewModel : Base.ViewModelBase
    {
        private WeekDTO _weekDTO = new WeekDTO();
<<<<<<< HEAD
        private IWeekApi _weekApi;
        private IPictogramApi _pictogramApi;
=======

        private readonly IWeekApi _weekApi;
        private readonly IPictogramApi _pictogramApi;
        private readonly IDialogService _dialogService;
        private readonly IRequestService _requestService;

>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
        private ValidatableObject<string> _scheduleName;
        public ValidatableObject<string> ScheduleName
        {
            get => _scheduleName;
            set
            {
                _scheduleName = value;
                RaisePropertyChanged(() => ScheduleName);
            }
        }
        private PictogramDTO _weekThumbNail;
        public PictogramDTO WeekThumbNail
        {
            get => _weekThumbNail;
            set
            {
                _weekThumbNail = value;
                RaisePropertyChanged(() => WeekThumbNail);
            }
        }

        public ICommand SaveWeekScheduleCommand => new Command(() => SaveWeekSchedule());
        public ICommand ChangePictogramCommand => new Command(() => ChangePictogram());

<<<<<<< HEAD
        public NewScheduleViewModel(INavigationService navigationService, IWeekApi weekApi, IPictogramApi pictogramApi) : base(navigationService)
        {
            _weekApi = weekApi;
            _pictogramApi = pictogramApi;
=======
        public NewScheduleViewModel(INavigationService navigationService, IWeekApi weekApi, IPictogramApi pictogramApi, IRequestService  requestService, IDialogService dialogService) : base(navigationService)
        {
            _weekApi = weekApi;
            _pictogramApi = pictogramApi;
            _requestService = requestService;
            _dialogService = dialogService;
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63

            _weekDTO.Thumbnail = _pictogramApi.V1PictogramByIdGet(2).Data;
            WeekThumbNail = _weekDTO.Thumbnail;

            _scheduleName = new ValidatableObject<string>(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Et navn er påkrævet." });
        }


        private void ChangePictogram()
        {
            NavigationService.NavigateToAsync<PictogramSearchViewModel>();
            MessagingCenter.Subscribe<PictogramSearchViewModel, PictogramDTO>(this, MessageKeys.PictoSearchChosenItem,
                InsertPicto);
        }

        private void InsertPicto(PictogramSearchViewModel arg1, PictogramDTO arg2)
        {
            WeekThumbNail = arg2;
        }

<<<<<<< HEAD
        private void SaveWeekSchedule()
=======
        private async void SaveWeekSchedule()
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
        {
            if (ValidateWeekScheduleName())
            {
                _weekDTO.Name = ScheduleName.Value;
<<<<<<< HEAD
                _weekDTO.Thumbnail = WeekThumbNail;

                _weekApi.V1WeekPostAsync(_weekDTO);

                NavigationService.PopAsync();
=======
                WeekThumbNail.AccessLevel = PictogramDTO.AccessLevelEnum.PUBLIC;
                _weekDTO.Thumbnail = WeekThumbNail;
                _weekDTO.Id = default(int);
                var list = new List<WeekdayDTO>();
                for (int i = 0; i < 7; i++)
                {
                    WeekdayDTO weekdayDTO = new WeekdayDTO();
                    weekdayDTO.Day = (WeekdayDTO.DayEnum)i;
                    weekdayDTO.Activities = new List<ActivityDTO>();
                    list.Add(weekdayDTO);
                }
                _weekDTO.Days = list;

                await _requestService.SendRequestAndThenAsync(this,
                requestAsync: async () => await _weekApi.V1WeekPostAsync(_weekDTO),
                onSuccess: async (R) => await _dialogService.ShowAlertAsync("Succes", "Ok", "Succes"));

                MessagingCenter.Send(this, "UpdateView");
                await NavigationService.PopAsync();
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
            }
        }

        public ICommand ValidateWeekNameCommand => new Command(() => _scheduleName.Validate());

        private bool ValidateWeekScheduleName()
        {
            var isWeekNameValid = _scheduleName.Validate();
            return isWeekNameValid;
        }
    }
}
