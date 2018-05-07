using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using WeekPlanner.Services.Navigation;
using WeekPlanner.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using WeekPlanner.Helpers;
using System.Collections.ObjectModel;

namespace WeekPlanner.ViewModels
{
    public class WeekTemplateViewModel : ViewModelBase
    {
        private readonly IWeekTemplateApi _weekTemplateApi;

        public WeekTemplateViewModel(INavigationService navigationService,
            IWeekTemplateApi weekTemplateApi) : base(navigationService)
        {
            _weekTemplateApi = weekTemplateApi;
        }

        private IEnumerable<WeekNameDTO> _weekNameDtos;

        public IEnumerable<WeekNameDTO> WeekNameDtos
        {
            get => _weekNameDtos;
            set
            {
                _weekNameDtos = value;
                RaisePropertyChanged(() => WeekNameDtos);
            }
        }

        public async override Task InitializeAsync(object navigationData)
        {
            ResponseIEnumerableWeekNameDTO result;
            try
            {
                result = await _weekTemplateApi.V1WeekTemplateGetAsync();
            }
            catch (ApiException)
            {
                var friendlyErrorMessage = ErrorCodeHelper.ToFriendlyString(ResponseString.ErrorKeyEnum.Error);
                MessagingCenter.Send(this, MessageKeys.WeekTemplatesNotFound, friendlyErrorMessage);
                return;
            }

            if (result.Success == true && result.Data.Count > 0)
            {
                WeekNameDtos = result.Data;
            }
            else
            {
                return;
            }
        }

        public ICommand ItemTappedCommand => new Command<WeekNameDTO>(dto => ItemTapped(dto));

        async void ItemTapped(WeekNameDTO dto) 
        {
            if (IsBusy) return;
            IsBusy = true;
            await NavigationService.PopAsync(dto);
            IsBusy = false;
        }

        private async Task NavigateToTemplate(WeekNameDTO dto)
        {
            await NavigationService.NavigateToAsync<WeekPlannerViewModel>(dto);
        }

        public ICommand ChooseTemplateCommand => new Command<WeekNameDTO>(async dto => await NavigateToTemplate(dto));
    }
}