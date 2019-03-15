using System;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading;
using Foundation;
using MaterialIcons.FormsPlugin.iOS;
using Rg.Plugins.Popup.IOS;
using RoundedBoxView.Forms.Plugin.iOSUnified;
using UIKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Distribute;
using Lottie.Forms.iOS.Renderers;
using Microsoft.AppCenter.Push;
using Syncfusion.SfRating.XForms.iOS;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.MediaManager.Forms.iOS;
using Xamarin.Forms;
using Lux.Interfaces;

namespace Lux.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Plugin.Iconize.Iconize.With(new Lux.Helpers.MaterialDesignIconsWebFont());
            VideoViewRenderer.Init();
            global::Xamarin.Forms.Forms.Init();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;;

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,

            };
            ImageService.Instance.Initialize(config);
            RoundedBoxViewRenderer.Init();

            MaterialIconControls.Init();
            CarouselViewRenderer.Init();
            AnimationViewRenderer.Init();
            Distribute.DontCheckForUpdatesInDebug();

            new SfRatingRenderer();


            AppCenter.Start("a25d705f-e8a0-4913-b8c0-7e374edefb6a", typeof(Analytics), typeof(Crashes), typeof(Push));

            LoadApplication(new Lux.App());
            return base.FinishedLaunching(app, options);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // ao dar crach exception sempre exclui as pastas
            DependencyService.Get<IConfig>().DirectoryDeleteAll();
        }


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Push.RegisteredForRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Push.FailedToRegisterForRemoteNotifications(error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            var result = Push.DidReceiveRemoteNotification(userInfo);
            if (result)
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NewData);
            }
            else
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NoData);
            }
        }


        // Details: https://github.com/wcoder/Xamarin.Plugin.DeviceOrientation#xamarinforms-ios
        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, IntPtr forWindow)
        {
            return Plugin.DeviceOrientation.DeviceOrientationImplementation.SupportedInterfaceOrientations;
        }

    }
}
