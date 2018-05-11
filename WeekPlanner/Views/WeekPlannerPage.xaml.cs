using WeekPlanner.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeekPlanner.ViewModels;

namespace WeekPlanner.Views
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WeekPlannerPage : PageBase
	{
		public WeekPlannerPage()
		{
			InitializeComponent();

			MessagingCenter.Subscribe<WeekPlannerViewModel, string>(this, "ChangeView", ChangeView);
		}

		private void ChangeView(WeekPlannerViewModel m, string s)
		{
			if (s == "Portrait")
			{
				//MultidayView.IsVisible = true;
				OneDayView.IsVisible = false;
			}
			else
			{
				OneDayView.IsVisible = false;
				//MultidayView.IsVisible = true;
			}
		}
	}
}
