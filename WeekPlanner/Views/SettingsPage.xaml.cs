using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeekPlanner.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

		public void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			DisplayAlert("Hey", "Med", "Dig");
		}

        private void DaysPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayAlert("Settings", "Antal af dage er blevet ændret", "Ok");
        }
        private void Picker_SelectedIndexChangedActivity(object sender, EventArgs e)
        {
            DisplayAlert("Indstillinger", $"Antal viste aktiviteter er blevet sat", "OK");
        }
    }
}
