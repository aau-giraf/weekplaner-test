using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using WeekPlanner.Services;
using WeekPlanner.Services.Login;
using WeekPlanner.Services.Navigation;
using WeekPlanner.Services.Settings;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;

namespace WeekPlanner.ViewModels
{
	public class WeekplannerChangeColourViewModel : ViewModelBase
	{
		private readonly INavigationService _navigationService;
		private readonly ISettingsService _settingsService;

		public WeekplannerChangeColourViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
		{
			_navigationService = navigationService;
			_settingsService = settingsService;
		}
	}
}
