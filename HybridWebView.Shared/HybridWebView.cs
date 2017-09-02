using System;

using Xamarin.Forms;

namespace UtilityWebViews
{
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

    private IHybridWebPage pageHandler;

    public void Forward()
    {
      if (pageHandler == null)
        return; // not ready
      pageHandler.Forward();
    }

    public void Back()
    {
      if (pageHandler == null)
        return;
      pageHandler.Back();
    }

    public void _setPageHandler(IHybridWebPage pageHandler)
    {
      this.pageHandler = pageHandler;
    }

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
