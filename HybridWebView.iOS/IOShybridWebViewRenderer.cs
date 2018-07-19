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
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Foundation;
using UIKit;

using D = System.Diagnostics.Debug;

using UtilityViews;
using WebKit;
using CoreGraphics;
using ObjCRuntime;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(IOShybridWebViewRenderer))]
namespace UtilityViews
{


  internal class NavDel : WKNavigationDelegate {

    private Func<NSUrlRequest, bool, bool> doAllow;

    internal NavDel(Func<NSUrlRequest, bool, bool> doAllow)
    {
      this.doAllow = doAllow;
    }

    public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
    {
      if (navigationAction.NavigationType == WKNavigationType.Other)
      {
        decisionHandler(WKNavigationActionPolicy.Allow);
        return;
      }
      decisionHandler(doAllow(navigationAction.Request, navigationAction.NavigationType == WKNavigationType.LinkActivated) ? WKNavigationActionPolicy.Allow: WKNavigationActionPolicy.Cancel);
    }
  }


  /// <summary>
  /// IOS Renderer
  /// </summary>
  public class IOShybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler, IWKUIDelegate, IHybridWebPage
  {
    WKUserContentController userController;

    private Dictionary<string, Action<object> > callbacks = new Dictionary<string, Action<object> >();

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
        userController = new WKUserContentController();

        WKPreferences preferences = new WKPreferences() { JavaScriptEnabled = true };

        var config = new WKWebViewConfiguration { UserContentController = userController, Preferences = preferences  };

        var webView = new WKWebView(Frame, config);
        webView.UIDelegate = this;

        webView.NavigationDelegate = new NavDel(shouldLoad);

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
          Control.LoadHtmlString(Element.Html, new NSUrl(""));
          return;
        }
        if (Element.Uri != null)
        {
          Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
        }
      }
    }

    private bool shouldLoad(NSUrlRequest request, bool linkClicked)
    {
      var uri = new Uri(request.Url.AbsoluteString);
      var doLoad = Element.ShouldHandleUri(uri, linkClicked);
      if ( doLoad && linkClicked )
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

    public void RegisterCallbackForJS(string name, Action<object> cb)
    {
      if( cb == null ) {
        callbacks.Remove(name);
        return;
      }

      string JavaScriptFunction = string.Format("function {0}(data){{window.webkit.messageHandlers.{0}.postMessage(data);}}", name) ;
    
      var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentStart, false);
      userController.AddUserScript(script);
      userController.AddScriptMessageHandler(this, name);

      callbacks[name] = cb;
    }

    public void EvaluateJS(string javscript)
    {
      Control.EvaluateJavaScript(new NSString(javscript), HandleWKJavascriptEvaluationResult);
    }

    void HandleWKJavascriptEvaluationResult(NSObject result, NSError error)
    {
      Console.WriteLine("JS Result = {0}  error = {1}", result, error);
    }


    [Export("webView:runJavaScriptAlertPanelWithMessage:initiatedByFrame:completionHandler:")]
    public void RunJavaScriptAlertPanel(WKWebView webView, string message, WKFrameInfo frame, Action completionHandler)
    {
      Console.WriteLine("JS Alert:  " + message);
      completionHandler();
    }

    public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
    {
      Console.WriteLine("message:  {0} {1}", message.Name, message.Body);
      Action<object> cb;
      if( ! callbacks.TryGetValue(message.Name, out cb) ) {
        Console.Error.WriteLine("callback \"{0}\" not found", message.Name);
        return;
      }
      cb(message.Body);
    }
  }
}
