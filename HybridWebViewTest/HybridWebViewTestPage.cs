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
using System.Linq;

using Xamarin.Forms;

using UtilityViews;

using D = System.Diagnostics.Debug;

namespace UtilityViews.Test
{
  /// <summary>
  /// A Sample of using the HybridWebView to implement a white list
  /// of Uri's.  
  /// </summary>
  class WhiteListWebView : HybridWebView
  {
    private HashSet<Uri> _whiteList;
    private HashSet<Uri> _openers;

    /// <summary>
    /// Gets or sets the white list.
    /// </summary>
    /// <value>The white list.</value>
    public IEnumerable<Uri> whiteList {
      get => _whiteList;
      set => _whiteList = new HashSet<Uri>(value);
    }

    /// <summary>
    /// Gets or sets the Uri's that will be opened.
    /// </summary>
    /// <value>The openers.</value>
    public IEnumerable<Uri> openers
    {
      get => _openers;
      set => _openers = new HashSet<Uri>(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:UtilityViews.Test.WhiteListWebView"/> class.
    /// </summary>
    /// <param name="whites">Uri's to whitelist</param>
    public WhiteListWebView(IEnumerable<Uri> whites, IEnumerable<Uri> openers)
    {
      whiteList = new HashSet<System.Uri>(whites);
      this.openers = new HashSet<System.Uri>(openers);
    }

    /// <summary>
    /// Parameterless Ctor for Xaml preview for Visual Studio
    /// </summary>
    public WhiteListWebView()
    {
      whiteList = new HashSet<System.Uri>();
      openers = new HashSet<System.Uri>();
    }

    /// <summary>
    /// Handle the Uri IF it appears in our white list
    /// </summary>
    /// <returns><c>true</c>if the URI should be followed<c>false</c> otherwise.</returns>
    /// <param name="uri">URI.</param>
    /// <param name="linkClicked">If set to <c>true</c> link clicked.</param>
    public override bool ShouldHandleUri(Uri uri, bool linkClicked)
    {
      var baseResult = base.ShouldHandleUri(uri, linkClicked);

      if( openers.Contains(uri) ) {
        Device.OpenUri(uri);
        return false; // we're handling this
      }


      return baseResult || whiteList.Contains(uri) ;
    }


  }

  public class HybridWebViewTestPage : ContentPage
  {
    private Label statusLabel;

    public HybridWebViewTestPage(string html)
    {

      /* NOTE  two entries for google, if you hit google.com you will
       * get a 301 redirect which will invoke the ShouldHandleUri method again
       */
      var whites = new Uri[] { new Uri("https://google.com"), 
        new Uri("https://www.google.com") };
      var openers = new Uri[] { new Uri("http://weather.com") };
      

      var hybridWebView = new WhiteListWebView(whites, openers) { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Html = html };

      hybridWebView.RegisterCallbackForJS("iosconsole", (o) => Console.WriteLine("{0}", o)) ;


      statusLabel = new Label() { HorizontalOptions = LayoutOptions.FillAndExpand };

      hybridWebView.UriClicked += (view, uri) => statusLabel.Text = uri.AbsoluteUri;

      var back = new Button() { Text = "Back", HorizontalOptions = LayoutOptions.StartAndExpand };
      var forward = new Button() { Text = "Forward", HorizontalOptions = LayoutOptions.EndAndExpand };
      var reload = new Button() { Text = "Reload", HorizontalOptions = LayoutOptions.CenterAndExpand };

      back.Clicked += (sender, e) => hybridWebView.Back();
      reload.Clicked += (sender, e) => hybridWebView.Html = html;
      forward.Clicked += (sender, e) => hybridWebView.Forward();

      var buttons = new StackLayout()
      {
        Orientation = StackOrientation.Horizontal,
        HorizontalOptions = LayoutOptions.FillAndExpand,
        Children = { back, reload, forward }
      };

      Content = new StackLayout
      {
        Children = {
         buttons,
          statusLabel,
          hybridWebView

        }
      };
    }
  }
}

