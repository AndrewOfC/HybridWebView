using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Webkit;

using UtilityWebViews;


[assembly: ExportRenderer(typeof(HybridWebView), typeof(DroidHybridWebViewRenderer))]
namespace UtilityWebViews
{
  public class DroidHybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>, IHybridWebPage
  {

    protected class webClient : WebViewClient {

      private DroidHybridWebViewRenderer renderer;

      public webClient(DroidHybridWebViewRenderer renderer)
      {
        this.renderer = renderer;
      }



      public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
      {
        return !renderer.Element.ShouldHandleUri(new Uri(url), true) ;

      }
    }

    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      HybridWebView hwv = (HybridWebView)sender;

      if (hwv.Html != null && e.PropertyName == "Html")
      {
        Control.LoadData(hwv.Html, "text/html; charset=UTF-8", null);
        return;
      }


    }

    protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
    {
      base.OnElementChanged(e);

      if (Control == null)
      {
        var webView = new Android.Webkit.WebView(Forms.Context);
        webView.SetWebViewClient(new webClient(this));
        SetNativeControl(webView);
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
          Control.LoadData(Element.Html, "text/html; charset=UTF-8", null);
          return;
        }

        if (Element.Uri != null)
        {
          Control.LoadUrl(Element.Uri);
        }

      }
    }

    public void Back()
    {
      if (Control.CanGoBack())
        Control.GoBack();
    }

    public void Forward()
    {
      if (Control.CanGoForward())
        Control.GoForward();
    }
  }
}
