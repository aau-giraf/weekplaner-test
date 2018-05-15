﻿using System;
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

		public WeekdayColors WeekdayColors { get; }

		public IEnumerable<SettingDTO.ThemeEnum> Themes { get; } = new List<SettingDTO.ThemeEnum>
		{
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

		public IEnumerable<SettingDTO.OrientationEnum> Orientations { get; } = new List<SettingDTO.OrientationEnum>
		{
			SettingDTO.OrientationEnum.Landscape, SettingDTO.OrientationEnum.Portrait
		};

		public SettingDTO.OrientationEnum Orientation
		{
			get => _settingsService.CurrentCitizenSettingDTO.Orientation;
			set
			{
				Settings.Orientation = value;
				RaisePropertyChanged(() => Orientation);
				UpdateSettingsAsync();
				SetOrientation();
			}
		}

		public SettingDTO Settings => _settingsService.CurrentCitizenSettingDTO;

		private void SetThemeInSettingDTOAndUpdate(SettingDTO.ThemeEnum pickedTheme)
		{
			Settings.Theme = pickedTheme;
			UpdateSettingsAsync();
		}

		private async Task UpdateSettingsAsync()
		{
			await _requestService.SendRequest(_userApi.V1UserByIdSettingsPutAsync(_settingsService.CurrentCitizen.UserId, Settings));
		}

		public void SetOrientation()
		{
			if (!_settingsService.IsInGuardianMode)
			{
				LandscapeOrientation();
			}
			else if (_settingsService.CurrentCitizenSettingDTO.Orientation == SettingDTO.OrientationEnum.Portrait)
			{
				PortraitOrientation();
			}
			else
			{
				LandscapeOrientation();
			}
		}

		private void PortraitOrientation()
		{
			MessagingCenter.Send(this, "SetOrientation", "Portrait");
			MessagingCenter.Send(this, "ChangeView", "Portrait");
		}

		private void LandscapeOrientation()
		{
			MessagingCenter.Send(this, "SetOrientation", "Landscape");
			MessagingCenter.Send(this, "ChangeView", "Landscape");
		}
	}
}