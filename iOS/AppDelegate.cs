﻿using FFImageLoading.Forms.Touch;
using Foundation;
using ObjCRuntime;
using UIKit;
using WeekPlanner.ViewModels;
using Xamarin.Forms;

namespace WeekPlanner.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
		
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, [Transient] UIWindow forWindow)
		{
			//MessagingCenter.Subscribe<WeekPlannerViewModel>(this, "forcePortrait", callback => 
			//{
			//	UIInterfaceOrientationMask.Portrait;
			//});
			return UIInterfaceOrientationMask.LandscapeRight;
		}

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }


    }
}
