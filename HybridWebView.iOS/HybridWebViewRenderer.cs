using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Foundation;
using UIKit;

using D = System.Diagnostics.Debug;

using HybridWebViewNS ;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace HybridWebView
{
  /// <summary>
  /// IOS Renderer
  /// </summary>
  public class HybridWebViewRenderer : ViewRenderer<HybridWebView, UIWebView>
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
        // var hybridWebView = e.OldElement as HybridWebView; // any clean up?
      }
      if (e.NewElement != null)
      {
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
      return Element._ShouldHandleUri(new Uri(request.Url.ToString()));
    }

  }
}
