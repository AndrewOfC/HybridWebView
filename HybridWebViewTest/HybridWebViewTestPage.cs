using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using UtilityWebViews;

using D = System.Diagnostics.Debug;

namespace HybridWebViewTest
{

  class WhiteListWebView : HybridWebView
  {
    private HashSet<Uri> whiteList;

    public WhiteListWebView(IEnumerable<Uri> whites)
    {
      whiteList = new HashSet<System.Uri>(whites);
    }

    public override bool ShouldHandleUri(Uri uri, bool linkClicked)
    {
      bool result = base.ShouldHandleUri(uri, linkClicked) || whiteList.Contains(uri) ;
      D.WriteLine("{0} uri = {1}", result, uri);
      return result;
    }
  }

  public class HybridWebViewTestPage : ContentPage
  {
    private Label statusLabel;

    public HybridWebViewTestPage(string html)
    {
      var whites = new Uri[] { new Uri("https://google.com"), new Uri("https://www.google.com") };

      var hybridWebView = new WhiteListWebView(whites) { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Html = html };

      statusLabel = new Label() { HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Blue };

      hybridWebView.UriClicked += (view, uri) => statusLabel.Text = uri.AbsoluteUri;

      var back = new Button() { Text = "Back", HorizontalOptions = LayoutOptions.StartAndExpand };
      var forward = new Button() { Text = "Forward", HorizontalOptions = LayoutOptions.EndAndExpand };
      var reload = new Button() { Text = "Reload", HorizontalOptions = LayoutOptions.CenterAndExpand };

      back.Clicked += (sender, e) => hybridWebView.Back();
      reload.Clicked += (sender, e) => hybridWebView.Html = html;
      forward.Clicked += (sender, e) => hybridWebView.Forward();

      var buttons = new StackLayout()
      {
        Orientation = StackOrientation.Horizontal,
        HorizontalOptions = LayoutOptions.FillAndExpand,
        Children = { back, reload, forward }
      };

      Content = new StackLayout
      {
        Children = {
         buttons,
          statusLabel,
          hybridWebView

        }
      };
    }
  }
}

