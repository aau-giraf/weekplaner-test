﻿using System.Collections.ObjectModel;
using WeekPlanner.Services.Navigation;
using WeekPlanner.ViewModels.Base;

namespace WeekPlanner.ViewModels
{
    public class ChooseTemplateViewModel : ViewModelBase
    {
        public ObservableCollection<string> ButtonNames => new ObservableCollection<string>
        {
            "Lige Uge 1",
            "Lige Uge 2",
            "Ulige Uge 1",
            "Ulige Uge 2",
        };

        public ChooseTemplateViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
