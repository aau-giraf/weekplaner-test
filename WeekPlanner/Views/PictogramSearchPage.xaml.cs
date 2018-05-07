﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WeekPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictogramSearchPage : ContentPage
    {
        public PictogramSearchPage()
        {
            InitializeComponent(); 
            NavigationPage.SetHasNavigationBar(this, false);
            searchField.Focus();
        }
    }
}
