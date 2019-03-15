using System;
using Xamarin.Forms;

namespace Lux.Helpers
{
    public class WebCustomView : WebView
    {
        Action<string> action;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
          propertyName: "Uri",
          returnType: typeof(string),
          declaringType: typeof(WebCustomView),
          defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
         propertyName: "FontSize",
         returnType: typeof(int),
         declaringType: typeof(WebCustomView),
        defaultValue: default(int));

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty ColorTypeProperty = BindableProperty.Create(
            propertyName: "ColorType",
            returnType: typeof(int),
            declaringType: typeof(WebCustomView),
            defaultValue: default(int));

        public int ColorType
        {
            get { return (int)GetValue(ColorTypeProperty); }
            set { SetValue(ColorTypeProperty, value); }
        }

        public static readonly BindableProperty FontTypeProperty = BindableProperty.Create(
        propertyName: "FontType",
        returnType: typeof(int),
        declaringType: typeof(WebCustomView),
        defaultValue: default(int));

        public int FontType
        {
            get { return (int)GetValue(FontTypeProperty); }
            set { SetValue(FontTypeProperty, value); }
        }

        public static readonly BindableProperty SearchVisibleProperty = BindableProperty.Create(
        propertyName: "SearchVisible",
        returnType: typeof(bool),
        declaringType: typeof(WebCustomView),
        defaultValue: default(bool));

        public bool SearchVisible
        {
            get { return (bool)GetValue(SearchVisibleProperty); }
            set { SetValue(SearchVisibleProperty, value); }
        }


        public static readonly BindableProperty ScrollPercenteReturnProperty = BindableProperty.Create(
        propertyName: "ScrollPercenteReturn",
        returnType: typeof(bool),
        declaringType: typeof(WebCustomView),
        defaultValue: default(bool));

        public bool ScrollPercenteReturn
        {
            get { return (bool)GetValue(ScrollPercenteReturnProperty); }
            set { SetValue(ScrollPercenteReturnProperty,value); }
        }

        public void RegisterAction(Action<string> callback) => action = callback;

        public void Cleanup() => action = null;

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }
    }
}
