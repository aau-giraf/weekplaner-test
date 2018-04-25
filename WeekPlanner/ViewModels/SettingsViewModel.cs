using System;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace WeekPlanner.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<UserNameDTO> _usernamedto;
        private readonly ISettingsService _settingsService;
        private readonly ILoginService _loginService;
        private readonly IRequestService _requestService;
        private readonly IDepartmentApi _departmentApi;

        public SettingsViewModel(ISettingsService settingsService, INavigationService navigationService, ILoginService loginService, IRequestService requestService, IDepartmentApi departmentApi) : base(navigationService)
        {
            _settingsService = settingsService;
            _loginService = loginService;
            _requestService = requestService;

        }

        //public ObservableCollection<UserNameDTO> userNameDTOs
        //{
        //    get => _usernamedto;
        //    set
        //    {
        //        _usernamedto = value;
        //        RaisePropertyChanged(() => _usernamedto);
        //    }
        //}

        //private async Task SetDTO()
        //{
        //    await _requestService.SendRequestAndThenAsync(this,
        //        requestAsync: async () => await _departmentApi.V1DepartmentByIdCitizensGetAsync
        //        (_settingsService.Department.Id), onSuccess: result => _usernamedto
        //        = new ObservableCollection<UserNameDTO>(result.Data));
        //}

        public override async Task InitializeAsync(object navigationData)
        {
            //userNameDTOs = new ObservableCollection<UserNameDTO>();
            //await _requestService.SendRequestAndThenAsync(this,
            //    requestAsync: async () => await _departmentApi.V1DepartmentByIdCitizensGetAsync
            //    (_settingsService.Department.Id), onSuccess: result => userNameDTOs
            //    = new ObservableCollection<UserNameDTO>(result.Data));

            //if (_settingsService.CitizenAuthToken == null && _settingsService.GuardianAuthToken == null)
            //{
                
            //    await _loginService.LoginAsync(UserType.Citizen, )
            //}
            //else
            //{
            //    throw new ArgumentException("Must be of type userNameDTO", nameof(navigationData));
            //}
        }
    }
}
