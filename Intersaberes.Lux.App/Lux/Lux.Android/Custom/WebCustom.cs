using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Lux.Helpers;
using Android.Content;
using System.ComponentModel;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(WebCustomView), typeof(Lux.Droid.Custom.WebCustom))]
namespace Lux.Droid.Custom
{
    public class WebCustom : WebViewRenderer
    {

        WebCustomView webView;
        public WebCustom(Context context) : base(context) { }

        public WebCustom() : base(null) => throw new Exception("This constructor should not actually ever be used");

        private string classeColor;
        private string fontFamily;
        private string fontSize;

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            webView = Element as WebCustomView;
            //SetNativeControl(Control);
            CustomElements();
            //SetWebView(element);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
             webView = Element as WebCustomView;
            if (e.PropertyName.Equals("Uri") && webView != null)
            {
                Control?.StopLoading();
                Control.LoadUrl(string.Format("file://{0}", webView.Uri));
            }
            else if (e.PropertyName.Equals("FontSize") && webView != null)
            {
                FontSize(webView.FontSize);

            }
            else if (e.PropertyName.Equals("ColorType") && webView != null)
            {
                ColorType(webView.ColorType);
            }
            else if (e.PropertyName.Equals("FontType") && webView != null)
            {
                FontType(webView.FontType);
            }
            else if(e.PropertyName.Equals("SearchVisible") && webView != null)
            {
                SearchVisible(webView.SearchVisible);
            }
            else if (e.PropertyName.Equals("ScrollPercenteReturn") && webView != null)
            {
                ScrollPercenteReturn(true);
            }
        }

        private void SearchVisible(bool valor){

            try
            {
                string visible = "none";
                if (valor) visible = "block";

                Control.EvaluateJavascript("javascript:document.getElementById('SearchEpub').style.display =\"" + visible + "\";", null);

            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "SearchVisible()");
            }
        }

        private void ColorType(int color)
        {

            try
            {
                classeColor = string.Empty;
                switch (color)
                {
                    case 1:

                        classeColor = "colormode-1";
                        break;
                    case 2:

                        classeColor = "colormode-2";
                        break;
                    case 3:

                        classeColor = "colormode-default";
                        break;
                }

                this.ExecuteJavaScript();

            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "ColorType()");

            }

        }

        private void FontType(int fontTypes)
        {

            try
            {
                this.fontFamily = string.Empty;

                switch (fontTypes)
                {
                    case 1:
                        fontFamily = "fonttype-serif";
                        break;

                    case 2:
                        fontFamily = "fonttype-sans";
                        break;

                    default:
                        fontFamily = "fonttype-serif";
                        break;

                }

                this.ExecuteJavaScript();

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "FontType()");
            }

        }

        private void FontSize(int fontsize){

            try
            {
                fontSize = string.Empty;
                switch (fontsize)
                {
                    case 0:

                        fontSize = "fontsize-small";
                        break;
                    case 1:

                        fontSize = "fontsize-normal";
                        break;
                    case 2:

                        fontSize = "fontsize-large";
                        break;
                }

                this.ExecuteJavaScript();

            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "FontSize()");
            }
        }

        public void ScrollPercenteReturn(bool a)
        {

            Control.EvaluateJavascript("javascript: document.getElementById('myRetornoScroll').innerText", new JavaScriptResult() );
            Control.EvaluateJavascript("javascript: document.getElementById('myTotalScroll').innerText", new JavaScriptResultTotal());

        }

        private void ExecuteJavaScript(){

            // controle de formato de tela
            var devicemode = "devicemode-mobile";
            var deviceorientation = "orientation-portrait";

            try
            {
                // se for tablet
                if ((int)App.Current.MainPage.Bounds.Width >= 767)
                {
                    devicemode = "devicemode-tablet";
                    deviceorientation = "orientation-portrait";
                }

                Control.EvaluateJavascript("javascript:document.getElementById('bodyPage').className =\"" + devicemode + " " + deviceorientation + " " + 
                                           classeColor + " " + fontFamily + " " + fontSize + "\";", null);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "ExecuteJavaScript()");
            }
        }

        private void CustomElements()
        {
            //Color
            //Control.SetBackgroundColor(Color.Blue.ToAndroid());

            //Fonts
            //Control.Settings.SerifFontFamily = "sans-serif-condensed-light";
            //Control.Settings.StandardFontFamily = "sans-serif-condensed-light";

            //JavaScript
            Control.Settings.JavaScriptEnabled = true;

            //ZOOM
            Control.Settings.SetSupportZoom(true);
            Control.Settings.BuiltInZoomControls = true;
            //Control.Settings.UseWideViewPort = true;
            //Control.Settings.DisplayZoomControls = true;
            //Control.ZoomOut();

            //Control.Settings.SetLayoutAlgorithm(Android.Webkit.WebSettings.LayoutAlgorithm.TextAutosizing);
            Control.Settings.LoadWithOverviewMode = true;

            //Eventos
            //Control.KeyPress += (object sender, KeyEventArgs e) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Tecla Apertada" + e.Handled + "Key " + e.KeyCode);
            //};

            //Control.CapturedPointer += (object sender, CapturedPointerEventArgs e) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Pointer Apertado");
            //};

            //Control.Touch += (object sender, TouchEventArgs e) => 
            //{
            //    System.Diagnostics.Debug.WriteLine("Touch Apertado " + e.Handled);
            //};
        }
    }

    class JavaScriptResult : Java.Lang.Object, Android.Webkit.IValueCallback
    {

        public async void OnReceiveValue(Java.Lang.Object result)
        {
            Java.Lang.String valorScrollAtual = (Java.Lang.String)result;
            Debug.WriteLine(valorScrollAtual);

            // grava o percentual do livro
            App.Current.Properties["PercentualLidoLivro"] = valorScrollAtual.Replace('"', ' ');
            App.Current.SavePropertiesAsync();

        }
    }

    class JavaScriptResultTotal : Java.Lang.Object, Android.Webkit.IValueCallback
    {

        public async void OnReceiveValue(Java.Lang.Object result)
        {
            Java.Lang.String valorTotalScroll = (Java.Lang.String)result;
            Debug.WriteLine(valorTotalScroll);

            // grava o percentual total do livro
            App.Current.Properties["PercentualTotalLivro"] = valorTotalScroll.Replace('"', ' '); ;
            App.Current.SavePropertiesAsync();

            await BookManagement.UpdateWebApiPercentualLeitura();
        }
    }
}