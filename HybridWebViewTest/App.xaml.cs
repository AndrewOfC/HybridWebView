using System;

using Xamarin.Forms;

namespace HybridWebViewTest
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      MainPage = new HybridWebViewTestPage();
    }
  }
}
