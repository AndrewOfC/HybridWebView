using System;

using Xamarin.Forms;

namespace HybridWebViewTest
{
  public partial class App : Application
  {
    public App(string html)
    {
      InitializeComponent();

      MainPage = new HybridWebViewTestPage(html);
    }
  }
}
