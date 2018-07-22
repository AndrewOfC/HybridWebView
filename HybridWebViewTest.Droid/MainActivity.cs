using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;

using UtilityViews.Test;

namespace HybridWebViewTest.Droid
{
  [Activity(Label = "HybridWebViewTest.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
  {
    int count = 1;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      var html = new StreamReader(Assets.Open("testpage.html")).ReadToEnd();

      global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

      LoadApplication(new App(html));
    }
  }
}

