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

using D = System.Diagnostics.Debug;

using Xamarin.Forms;

namespace UtilityViews.Test
{
  public partial class HybridWebViewTestPage2 : TabbedPage
  {
    public HybridWebViewTestPage2()
    {
      InitializeComponent();
      webView.RegisterCallbackForJS("iosconsole", (o) => Console.WriteLine("{0}", o));

    }

    public HybridWebViewTestPage2(string html)
    {
      InitializeComponent();
      webView.Html = html;

      var whites = new Uri[] { new Uri("https://google.com"), 
        new Uri("https://www.google.com") };
      var openers = new Uri[] { new Uri("http://weather.com") };

      webView.whiteList = whites;
      webView.openers = openers;
      webView.RegisterCallbackForJS("iosconsole", (o) => Console.WriteLine("{0}", o));
    }

    private void onClick(object sender, EventArgs args)
    {
      webView.EvaluateJS("[]", (o) => { D.WriteLine("4 = {0}", o); });
    }
  }
}
