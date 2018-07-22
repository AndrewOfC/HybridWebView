/*
 * Copyright 2017 Andrew E. Page
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Webkit;

using UtilityViews;
using Android.Content;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(DroidHybridWebViewRenderer))]
namespace UtilityViews
{
  public class DroidHybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>, IHybridWebPage
  {
    public DroidHybridWebViewRenderer(Context context) : base(context)
    {
    }

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
        e.OldElement._setPageRenderer(null);
      }
      if (e.NewElement != null)
      {
        e.NewElement._setPageRenderer(this);
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

    public void RegisterCallbackForJS(string name, Action<object> cb)
    {
      // throw new NotImplementedException();
    }

    public void EvaluateJS(string javascript)
    {
      throw new NotImplementedException();
    }
  }
}
