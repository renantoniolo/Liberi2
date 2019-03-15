using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using CoreGraphics;
using Foundation;
using Lux.Helpers;
using Lux.iOS.Custom;
using UIKit;
using WebKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(WebCustomView), typeof(WebCustom))]
namespace Lux.iOS.Custom
{
    public class WebCustom : ViewRenderer<WebCustomView, UIWebView>
    {
        private UIWebView webView;

        private string classeColor;
        private string fontFamily;
        private string fontSize;

        public WebCustom(){}


        protected override void OnElementChanged(ElementChangedEventArgs<WebCustomView> e)
        {
            try
            {
                if (Control == null)
                {
                    webView = new UIWebView(Frame);
                    webView.ScrollView.ScrollEnabled = false;
                    webView.ScrollView.Bounces = false;
                    webView.AutoresizingMask = UIViewAutoresizing.All;
                    SetNativeControl(webView);
                    var element = Element as WebCustomView;

                    webView.LoadRequest(new NSUrlRequest(new NSUrl(string.Format("file://{0}", element.Uri))));

                    webView.LoadFinished += (object senders, EventArgs a) =>
                    {
                        Console.WriteLine("WebView Load completed");
                        webView.ScrollView.ScrollEnabled = false;
                        webView.ScrollView.Bounces = false;
                        FontSize(element.FontSize);
                        ColorType(element.ColorType);
                        FontType(element.FontType);
                        ScrollPercenteReturn(element.ScrollPercenteReturn);

                    };
                }
            }
            catch(Exception exception){

                Function.AppCenterCrashes(exception, "WebCustom", "OnElementChanged()");
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);
                var element = Element as WebCustomView;
                webView.ScrollView.ScrollEnabled = false;
                webView.ScrollView.Bounces = false;

                if (e.PropertyName.Equals("Uri") && element != null)
                {
                    Control?.StopLoading();

                    this.webView.LoadRequest(new NSUrlRequest(new NSUrl(string.Format("file://{0}", element.Uri))));

                    webView.LoadFinished += (object senders, EventArgs a) =>
                    {
                        Console.WriteLine("WebView Load completed");
                        FontSize(element.FontSize);
                        ColorType(element.ColorType);
                        FontType(element.FontType);
                    };

                }
                else if (e.PropertyName.Equals("FontSize") && element != null)
                {
                    FontSize(element.FontSize);
                }
                else if (e.PropertyName.Equals("ColorType") && element != null)
                {
                    ColorType(element.ColorType);
                }
                else if (e.PropertyName.Equals("FontType") && element != null)
                {
                    FontType(element.FontType);
                }
                else if (e.PropertyName.Equals("SearchVisible") && element != null)
                {
                    SearchVisible(element.SearchVisible);
                }
                else if (e.PropertyName.Equals("ScrollPercenteReturn") && element != null){

                    ScrollPercenteReturn(true);
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "OnElementPropertyChanged()");
            }

        }

        private void SearchVisible(bool valor)
        {

            try
            {
                string visible = "none";
                if (valor) visible = "block";

                webView.EvaluateJavascript("javascript:document.getElementById('SearchEpub').style.display =\"" + visible + "\";");
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "SearchVisible()");
            }
        }


        public async void ScrollPercenteReturn(bool a)
        {
            string valorScrollAtual = webView.EvaluateJavascript("javascript: document.getElementById('myRetornoScroll').innerText");
            string ValorScrollTotal = webView.EvaluateJavascript("javascript: document.getElementById('myTotalScroll').innerText");

            if (!String.IsNullOrEmpty(ValorScrollTotal))
            {

                // grava o percentual do livro
                App.Current.Properties["PercentualLidoLivro"] = valorScrollAtual;
                App.Current.Properties["PercentualTotalLivro"] = ValorScrollTotal;
                App.Current.SavePropertiesAsync();

                await BookManagement.UpdateWebApiPercentualLeitura();
            }

        }


        private void FontSize(int fontsize)
        {
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

                ExecuteJavaScript();
            }
            catch(Exception exception){
                Function.AppCenterCrashes(exception, "WebCustom", "FontSize()");
            }
        }

        private void FontType(int fontTypes){

            try
            {
                fontFamily = string.Empty;

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

                ExecuteJavaScript();

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "FontType()");
            }

        }

        private void ColorType(int colorType)
        {
            try
            {
                classeColor = string.Empty;

                switch (colorType)
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

                ExecuteJavaScript();


            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "ColorType()");
            }

        }

        private void ExecuteJavaScript()
        {
            // controle de formato de tela
            var devicemode = "devicemode-mobile";
            var deviceorientation = "orientation-portrait";

            try
            {
                // se for tablet
                if ((int)App.Current.MainPage.Bounds.Width >= 767){
                    devicemode = "devicemode-tablet";
                    deviceorientation = "orientation-portrait";
                }

                // comando javascript
                webView.EvaluateJavascript("javascript:document.getElementById('bodyPage').className =\"" + devicemode + " " + deviceorientation + " " + 
                                           classeColor + " " + fontFamily + " " + fontSize + "\";");
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "WebCustom", "ExecuteJavaScript()");
            }
        }


    }
}
