using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;



namespace UtilityViews.Test.Droid
{
  [Activity(Label = "HybridWebViewTest.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      var html = new StreamReader(Assets.Open("testpage.html")).ReadToEnd();

      global::Xamarin.Forms.Forms.Init(this, bundle);

      LoadApplication(new App(html));
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
      base.OnActivityResult(requestCode, resultCode, data);
    }
  }
}
