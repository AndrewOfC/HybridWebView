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

namespace UtilityViews
{
  /// <summary>
  /// A WebView that allows you to check a Uri before loading to handle.
  /// it differently.  
  /// </summary>
  public class HybridWebView : View, IHybridWebPage
  {
    public event Action<HybridWebView, Uri> UriClicked;

    /// <summary>
    /// Backing store for the Uri property
    /// </summary>
    public static readonly BindableProperty UriProperty = BindableProperty.Create(
      propertyName: "Uri",
      returnType: typeof(Uri),
      declaringType: typeof(HybridWebView),
      defaultValue: default(Uri));

    /// <summary>
    /// Backing store for the Html property
    /// </summary>
    public static readonly BindableProperty HtmlProperty = BindableProperty.Create(
      propertyName: "Html",
      returnType: typeof(string),
      declaringType: typeof(HybridWebView),
      defaultValue: default(string));

    /// <summary>
    /// Gets or sets the URI.
    /// </summary>
    /// <value>The URI.</value>
    public string Uri { get => (string)GetValue(UriProperty); set => SetValue(UriProperty, value); }

    /// <summary>
    /// Gets or sets the html content of the page.
    /// </summary>
    /// <value>The html.</value>
    public string Html { get => (string)GetValue(HtmlProperty); set => SetValue(HtmlProperty, value); }

    private IHybridWebPage pageRenderer;

    /// <summary>
    /// Go forward 1 step in the WebView's hisory
    /// </summary>
    public void Forward()
    {
      if (pageRenderer == null)
        return; // not ready
      pageRenderer.Forward();
    }

    /// <summary>
    /// Go back 1 step in the WebView's hisory
    /// </summary>
    public void Back()
    {
      if (pageRenderer == null)
        return;
      pageRenderer.Back();
    }

    /// <summary>
    /// Called by the renderer so the HybridWebView may invoke
    /// platform specific services.
    /// </summary>
    /// <param name="pageRenderer">Page renderer.</param>
    public void _setPageRenderer(IHybridWebPage pageRenderer)
    {
      this.pageRenderer = pageRenderer;
    }

    /// <summary>
    /// Check the Uri.  
    /// </summary>
    /// 
    /// the base class should be called as it provides the service of
    /// firing the UriClicked event.  <see cref="HybridWebView.UriClicked"/>
    /// 
    /// <code>
    /// 
    /// base.ShouldHandleUri(uri, linkClicked) || myTest(uri) ;
    /// 
    /// </code>
    /// 
    /// 
    /// <returns><c>true</c>if the uri is to be followed<c>false</c> otherwise.</returns>
    /// <param name="uri">URI.</param>
    /// <param name="linkClicked">If set to <c>true</c> we arrived here as a result of user clicking a link</param>
    public virtual bool ShouldHandleUri(Uri uri, bool linkClicked)
    {
      if (uri.Scheme == "file")
        return true;
      if (linkClicked)
        UriClicked?.Invoke(this, uri);
      return false ;
    }

  }
}
