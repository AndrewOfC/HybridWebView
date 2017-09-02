using System;

using Xamarin.Forms;

namespace HybridWebViewTest
{
  public class HybridWebViewTestPage : ContentPage
  {
    public HybridWebViewTestPage()
    {
      var w = new HybridWebView.HybridWebView() { Html = "<html><head><title>title</title></head><body><a href=\"https://google.com\">link</a></body><html>" };
      Content = w;
    }
  }
}

