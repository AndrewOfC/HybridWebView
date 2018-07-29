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
using D = System.Diagnostics.Debug;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Interop;

using Android.Webkit;

using UtilityViews;
using Android.Content;
using Java.Lang;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(DroidHybridWebViewRenderer))]
namespace UtilityViews
{
  public class JSBridge : Java.Lang.Object
  {
    readonly WeakReference<DroidHybridWebViewRenderer> hybridWebViewRenderer;
    private Action<object> callback ;

    public JSBridge(DroidHybridWebViewRenderer hybridRenderer, Action<object> callback)
    {
      this.callback = callback;
      hybridWebViewRenderer = new WeakReference<DroidHybridWebViewRenderer>(hybridRenderer);
    }

    [Export("invokeAction")]
    [JavascriptInterface]
    public void InvokeAction(Java.Lang.String data)
    {
      D.WriteLine("invoking \"{0}\"", data);
      DroidHybridWebViewRenderer hybridRenderer;

      if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
      {
        callback(data);
      }
    }

    [Export("invokeAction")]
    [JavascriptInterface]
    public void InvokeAction(Java.Lang data)
    {
      D.WriteLine("invoking \"{0}\"", data);
      DroidHybridWebViewRenderer hybridRenderer;

      if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
      {
        callback(data);
      }
    }
  }

  public class DroidHybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>, IHybridWebPage
  {
    private Context context;
    public DroidHybridWebViewRenderer(Context context) : base(context)
    {
      this.context = context;
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
        var webView = new Android.Webkit.WebView(context);
        webView.Settings.JavaScriptEnabled = true;
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
      Control.AddJavascriptInterface(new JSBridge(this, cb), name + "_jsBridge");
      string JavaScriptInvokeFunction = string.Format("function {0}(data){{  {0}_jsBridge.invokeAction(data);}}", name) ;
      D.WriteLine("javascript enabled {0}", Control.Settings.JavaScriptEnabled);

      EvaluateJS(JavaScriptInvokeFunction, (o) => { 
        D.WriteLine("reg result = {0}", o); 
      });


    }

    internal class JavaScriptCallback : Java.Lang.Object, IValueCallback
    {
      // public IntPtr Handle => throw new NotImplementedException();

      Action<object> action;

      public JavaScriptCallback(Action<object> action)
      {
        this.action = action;
      }

      public void Dispose()
      {
        this.action = null;
      }

      public void OnReceiveValue(Java.Lang.Object value)
      {
        action?.Invoke(value);
      }
    }

    public void EvaluateJS(string javascript, Action<object> handleJavaScriptReturnValue)
    {
      Control.EvaluateJavascript(javascript, handleJavaScriptReturnValue == null ? null : new JavaScriptCallback(handleJavaScriptReturnValue));
    }
  }
}
