﻿using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class ViewExtensions
    {
        public static UIView ToUIView( this View view, CGRect size )
        {
            var renderer = Platform.CreateRenderer( view );

            renderer.NativeView.Frame = size;

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            renderer.Element.Layout( size.ToRectangle() );

            var nativeView = renderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;
        }
    }
}
