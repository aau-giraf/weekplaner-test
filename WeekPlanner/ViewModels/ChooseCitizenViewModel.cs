using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using IO.Swagger.Api;
using IO.Swagger.Model;
using WeekPlanner.Helpers;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Request;
using WeekPlanner.Services.Settings;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;

namespace WeekPlanner.ViewModels
{
    public class ChooseCitizenViewModel : ViewModelBase
    {
        private ObservableCollection<UserNameDTO> _citizenNames;
        private readonly IDepartmentApi _departmentApi;
	    private readonly IRequestService _requestService;
	    private readonly ISettingsService _settingsService;
	    
	    
	    public ObservableCollection<UserNameDTO> CitizenNames {
		    get => _citizenNames;
		    set {
			    _citizenNames = value;
			    RaisePropertyChanged(() => CitizenNames);
		    }
	    }

        public ChooseCitizenViewModel(INavigationService navigationService, IDepartmentApi departmentApi, 
	        IRequestService requestService, ISettingsService settingsService) : base(navigationService)
        {
	        _departmentApi = departmentApi;
	        _requestService = requestService;
	        _settingsService = settingsService;
        }

	    public ICommand ChooseCitizenCommand => new Command<UserNameDTO>(async usernameDTO =>
		    await UseGuardianTokenAndNavigateToWeekPlan(usernameDTO));

	    private async Task UseGuardianTokenAndNavigateToWeekPlan(UserNameDTO usernameDTO)
	    {
<<<<<<< HEAD
		    _settingsService.UseTokenFor(UserType.Department);
=======
		    _settingsService.UseTokenFor(UserType.Guardian);
>>>>>>> 0573501efca6c73f1bcc6c6c18b0304a6a4fba63
		    await NavigationService.NavigateToAsync<CitizenSchedulesViewModel>(usernameDTO);
	    }

	    private async Task GetAndSetCitizenNamesAsync()
	    {
		    // Always use the departmentToken when coming to this view.
		    // It might have been changed to using the citizenToken
            _settingsService.UseTokenFor(UserType.Guardian);

		    await _requestService.SendRequestAndThenAsync(this,
			    requestAsync: async () => await _departmentApi.V1DepartmentByIdCitizensGetAsync(_settingsService.Department.Id),
			    onSuccess: result => CitizenNames = new ObservableCollection<UserNameDTO>(result.Data));
	    }

	    public override async Task InitializeAsync(object navigationData)
	    {
		    await GetAndSetCitizenNamesAsync();
	    }
    }

}
