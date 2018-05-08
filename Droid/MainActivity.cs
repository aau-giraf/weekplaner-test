using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using FFImageLoading.Forms.Droid;
using WeekPlanner.ViewModels;
using Xamarin.Forms;
using WeekPlanner.Views;

namespace WeekPlanner.Droid
{

    [Activity(Label = "Giraf Ugeplan", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation /*, ScreenOrientation = ScreenOrientation.Landscape*/)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        ScreenOrientation targetOrientation= ScreenOrientation.Landscape;
       

        private void SetOrientation(WeekPlannerViewModel m, string s )
        {
            switch (s)
            {
                case "Landscape":
                    RequestedOrientation = ScreenOrientation.Landscape;
                    break;
                case "Portrait":
                   RequestedOrientation = ScreenOrientation.Portrait;
                    break;
                default:
                    targetOrientation = ScreenOrientation.Landscape;
                    break;

            }

        }

        protected override void OnCreate(Bundle bundle)
        {

            MessagingCenter.Subscribe<WeekPlannerViewModel,string>(this,"SetOrientation", SetOrientation);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            

			base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            // Load ffimageloading
            CachedImageRenderer.Init(enableFastRenderer: true);

            // Load Acr.UserDialogs
            UserDialogs.Init(this);

            LoadApplication(new App());
            
            // Does so the on-screen keyboard does not hide custom navigation bar
            Window.SetSoftInputMode(SoftInput.AdjustResize);
            
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
