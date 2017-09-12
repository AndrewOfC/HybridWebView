# Overview

A Xamarin Forms WebView that will allow you to intercept a URI so that it can be handled specially

# Structure

Implemented as a [ViewRenderer](https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/custom-renderer/view) supporting iOS and Android

# NuGet 

Intended to be distributed as a NuGet package

# Branches

* master 
* build Where we will build the latest release

# License

MIT License

Copyright 2017 Andrew E. Page

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


# Example
```csharp
using UtilityViews ;
 /// <summary>
  /// A Sample of using the HybridWebView to implement a white list
  /// of Uri's.  
  /// </summary>
  class WhiteListWebView : HybridWebView
  {
    private HashSet<Uri> whiteList;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:UtilityViews.Test.WhiteListWebView"/> class.
    /// </summary>
    /// <param name="whites">Uri's to whitelist</param>
    public WhiteListWebView(IEnumerable<Uri> whites)
    {
      whiteList = new HashSet<System.Uri>(whites);
    }

    /// <summary>
    /// Parameterless Ctor for Xaml preview for Visual Studio
    /// </summary>
    public WhiteListWebView()
    {
      whiteList = new HashSet<System.Uri>();
    }

    /// <summary>
    /// Handle the Uri IF it appears in our white list
    /// </summary>
    /// <returns><c>true</c>if the URI should be followed<c>false</c> otherwise.</returns>
    /// <param name="uri">URI.</param>
    /// <param name="linkClicked">If set to <c>true</c> link clicked.</param>
    public override bool ShouldHandleUri(Uri uri, bool linkClicked)
    {
      return base.ShouldHandleUri(uri, linkClicked) || whiteList.Contains(uri) ;
    }
  }

```

