using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Foundation;
using UIKit;

using D = System.Diagnostics.Debug;

using UtilityWebViews ;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(IOShybridWebViewRenderer))]
namespace UtilityWebViews
{
  /// <summary>
  /// IOS Renderer
  /// </summary>
  public class IOShybridWebViewRenderer : ViewRenderer<HybridWebView, UIWebView>, IHybridWebPage
  {
    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      HybridWebView hwv = (HybridWebView)sender;

      if (hwv.Html != null && e.PropertyName == "Html")
      {
        Control.LoadHtmlString(hwv.Html, new NSUrl(""));
        return;
      }

      if( hwv.Uri != null && e.PropertyName == "Uri" ) {
        Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
        return;
      }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
    {
      base.OnElementChanged(e);
      if (Control == null)
      {
        var nativeWebView = new UIWebView();

        /*
         * Set the delegate that will check if the requested url should be loaded
         */
        nativeWebView.ShouldStartLoad = shouldLoad;

        SetNativeControl(nativeWebView);
      }
      if (e.OldElement != null)
      {
        e.OldElement._setPageHandler(null);
      }
      if (e.NewElement != null)
      {
        e.NewElement._setPageHandler(this);
        if (Element.Html != null)
        {
          Control.LoadHtmlString(Element.Html, new NSUrl(""));
          return;
        }

        if (Element.Uri != null)
        {
          Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
        }
      }
    }

    private bool shouldLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
    {
      var uri = new Uri(request.Url.AbsoluteString);
      var linkClicked = navigationType == UIWebViewNavigationType.LinkClicked;
      var doLoad = Element.ShouldHandleUri(uri, linkClicked);
      if (doLoad && linkClicked )
        Element.Html = null; // otherwise we don't get a property change call when reloading.
      return doLoad;
    }

    public void Forward()
    {
      if( Control.CanGoForward )
        Control.GoForward();
    }

    public void Back()
    {
      if (Control.CanGoBack)
        Control.GoBack();
    }
  }
}
