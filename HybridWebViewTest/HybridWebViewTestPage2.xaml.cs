using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace UtilityViews.Test
{
  public partial class HybridWebViewTestPage2 : TabbedPage
  {
    public HybridWebViewTestPage2()
    {
      InitializeComponent();
    }

    public HybridWebViewTestPage2(string html)
    {
      InitializeComponent();
      webView.Html = html;

      var whites = new Uri[] { new Uri("https://google.com"), 
        new Uri("https://www.google.com") };
      var openers = new Uri[] { new Uri("http://weather.com") };

      webView.whiteList = whites;
      webView.openers = openers;


    }
  }
}
