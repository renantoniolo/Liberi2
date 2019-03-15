using Android.OS;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;

using Xamarin.Forms;
using Plugin.Media;
using Plugin.CurrentActivity;
using Plugin.DeviceOrientation;

using RoundedBoxView.Forms.Plugin.Droid;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Droid;
using FFImageLoading;
using Lottie.Forms.Droid;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Plugin.MediaManager.Forms.Android;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Permissions;
using System;
using Lux.Interfaces;

namespace Lux.Droid
{
    [Activity(Label = "Liberi", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
           
            base.OnCreate(bundle);

            CrossCurrentActivity.Current.Activity = this;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;

            CachedImageRenderer.Init(true);

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false
            };
            ImageService.Instance.Initialize(config);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CrossMedia.Current.Initialize();
            RoundedBoxViewRenderer.Init();
            CarouselViewRenderer.Init();
 
            AnimationViewRenderer.Init();
            VideoViewRenderer.Init();
            Acr.UserDialogs.UserDialogs.Init(() => (Activity)Forms.Context);

           
            Push.SetSenderId("794810249174");
        
            AppCenter.Start("4d26fa6b-7182-4c24-9a81-79f0face2afe", typeof(Analytics),typeof(Crashes),typeof(Push));

            LoadApplication(new App());
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // ao dar crach exception sempre exclui as pastas
            DependencyService.Get<IConfig>().DirectoryDeleteAll();
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


    }
}

