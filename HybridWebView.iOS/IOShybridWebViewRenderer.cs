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
using Xamarin.Forms.Platform.iOS;

using Foundation;
using UIKit;

using D = System.Diagnostics.Debug;

using UtilityViews ;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(IOShybridWebViewRenderer))]
namespace UtilityViews
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
        e.OldElement._setPageRenderer(null);
      }
      if (e.NewElement != null)
      {
        e.NewElement._setPageRenderer(this);
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
