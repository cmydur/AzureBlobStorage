using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Azure.Mobile;

namespace AzureBlobStorage.Droid
{
    [Activity(Label = "My Picks", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
          
            global::Xamarin.Forms.Forms.Init(this, bundle);
            MobileCenter.Configure("4444398b-307e-4c28-8693-669d1ade242c");
            LoadApplication(new AzureBlobStorage.App());
        }
    }
}

