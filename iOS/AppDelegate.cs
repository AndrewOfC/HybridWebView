using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Foundation;
using UIKit;

namespace UtilityViews.Test.iOS
{
  [Register("AppDelegate")]
  public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
  {
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      var basepath = NSBundle.MainBundle.BundlePath;
      var docpath = NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
      var tmppath = System.IO.Path.GetTempPath();

      Console.WriteLine("Doc:  {0}", docpath);
      Console.WriteLine("Base: {0}", basepath);
      Console.WriteLine("Temp: {0}", tmppath);
      global::Xamarin.Forms.Forms.Init();

      var html = new StreamReader(new FileStream("testpage.html", FileMode.Open)).ReadToEnd() ;

      LoadApplication(new App(html));

      return base.FinishedLaunching(app, options);
    }
  }
}
