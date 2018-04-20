using System;
using WeekPlanner.ViewModels;
using WeekPlanner.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeekPlanner.Views
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WeekPlannerPage : ContentPage
	{
		public WeekPlannerPage()
		{
			InitializeComponent();
		}

		/*
		* The following allows for specification of the orientation of the WeekPlannerPage. 
		* This, however, is dependent on the not yet implemented user story regarding citizen orientation setting
		* and should be set accordingly once this has been implemented. 
		*/
		//protected override void OnAppearing()
		//{
		//	base.OnAppearing();


		//	MessagingCenter.Send(this, "forcePortrait");
		//}
	}
}
