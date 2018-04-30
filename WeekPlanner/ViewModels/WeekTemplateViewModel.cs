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
                //fail
                return;
            }

            if (result.Success == true && result.Data.Count > 0)
            {
                WeekNameDtos = result.Data;
            }
            else
            {
                //send besked om at der ikke kan findes noget data
            }
        }

        private async Task NavigateToTemplate(WeekNameDTO dto)
        {
            await NavigationService.NavigateToAsync<WeekPlannerViewModel>(dto);
        }

        public ICommand ChooseTemplateCommand => new Command<WeekNameDTO>(async dto => await NavigateToTemplate(dto));
    }
}