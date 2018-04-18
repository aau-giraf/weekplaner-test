using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeekPlanner.Views
{
    public partial class WeekTemplatePage : ContentPage
    {
        public WeekTemplatePage()
        {
            InitializeComponent();
            /*MessagingCenter.Subscribe<ChooseCitizenViewModel, string>(this, MessageKeys.CitizenListRetrievalFailed,
             *   async (sender, message) =>
             *       await DisplayAlert("Fejl", message, "Luk"));
             */
        }


    }
}
