using System;

using Xamarin.Forms;

namespace HybridWebView
{
  public class HybridWebView : View
  {
    public HybridWebView()
    {
      Console.WriteLine("Loaded");
    }

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

    /// <summary>
    /// Override as necessary to filter uris.  By default it will allow file uris to be loaded
    /// to allow loading of the page.   Needs to be public so the renderer can call it but we mark
    /// it with a python-esque private marker.
    /// 
    /// https://docs.python.org/3/tutorial/classes.html#private-variables
    /// </summary>
    /// <returns><c>true</c>, if handle URI was shoulded, <c>false</c> otherwise.</returns>
    /// <param name="uri">URI.</param>
    public virtual bool _ShouldHandleUri(Uri uri)
    {
      if (uri.Scheme == "file")
        return true;
      UriClicked?.Invoke(this, uri);

      return false;
    }

  }

  class HWV2 : HybridWebView {
    
  }
}
