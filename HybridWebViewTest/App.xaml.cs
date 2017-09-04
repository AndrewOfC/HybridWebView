using System;

using Xamarin.Forms;

namespace UtilityViews.Test
{
  public partial class App : Application
  {


    public App()
    {
      InitializeComponent();
      var html = "<html><head></head><body>foo</body></html>";
      MainPage = new HybridWebViewTestPage2(html);
    }

    public App(string html)
    {
      InitializeComponent();

      MainPage = new HybridWebViewTestPage2(html);
    }
  }
}
