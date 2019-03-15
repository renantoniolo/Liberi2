using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Lux.Models;
using Lux.Views.Popup;
using Xamarin.Forms;
using AppLeitor;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Lux.Views;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Lux.Services;
using Lux.Helpers;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;

using Lux.Interfaces;
using HtmlAgilityPack;
using Android.Webkit;

namespace Lux.ViewsModels
{
    public class EPUBViewModel : BaseViewModel
    {
        #region PropSimple
        public int FontSize { get; set; } = 1;
        public int FontType { get; set; } = 2;
        public int ColorType { get; set; } = 3;
        public bool SearchVisible { get; set; } = false;
        private int displayWidth;
        private int displayHeigth;
        private string htmArquivo;
        #endregion

        private EPUBPage epubPage;
        private Livro livro;
        private IAlertService alertService;

        public Command LoadItemsCommand { get; set; }
        public Command ConfigLeitorCommand { get; set; }
        public Command ShareLeitorCommand { get; set; }
        public Command SearchLeitorCommand { get; set; }

        public EPUBViewModel(Livro livro, EPUBPage epubPage)
        {
            try
            {

                this.livro = livro;
                this.FontType = 2;
                this.ColorType = 3;
                this.FontSize = 1;
                this.SearchVisible = false;
                
                displayWidth = (int)App.Current.MainPage.Bounds.Width;
                displayHeigth = (int)App.Current.MainPage.Height - 80;

                this.epubPage = epubPage;

                this.alertService = DependencyService.Get<IAlertService>();

                EpubLivro = new Epub();
                LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());
                ConfigLeitorCommand = new Command(async () => await OpenPopup());
                ShareLeitorCommand = new Command(() => OpenShare());
                SearchLeitorCommand = new Command(() => OpenSerach());

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "EPUBViewModel", "Constructor()");
            }

        }

        public void PercentualLeitura()
        {

            if (EpubLivro.ScrollPercenteReturn) EpubLivro.ScrollPercenteReturn = false;
            else EpubLivro.ScrollPercenteReturn = true;

        }

        public void ChangeProperty(int fontSize = -1, int colorType = -1, int fontType = -1, bool SearchVisible = false)
        {

            if (fontSize != -1 && colorType != -1 && fontType != -1 )
            {
                this.FontSize = fontSize;
                this.FontType = fontType;
                this.ColorType = colorType;
                this.SearchVisible = SearchVisible;
            }

            try
            {
                EpubLivro.FontSize = FontSize;
                EpubLivro.FontType = FontType;
                EpubLivro.ColorType = ColorType;
                EpubLivro.SearchVisible = SearchVisible;

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "EPUBViewModel", "ChangeProperty()");
            }
        }


        private void OpenShare()
        {
            try
            {
                if (!CrossShare.IsSupported) return;

                CrossShare.Current.Share(new ShareMessage
                {
                    Title = "Editora InterSaberes",
                    Text = Leitor.Titulo,
                    Url = "https://www.livrariaintersaberes.com.br"
                });


            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "EPUBViewModel", "OpenShare()");
            }

        }

        private void OpenSerach()
        {
            if(SearchVisible) SearchVisible = false;
            else SearchVisible = true;

            EpubLivro.SearchVisible = SearchVisible;

        }


        async Task OpenPopup()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await App.Current.MainPage.Navigation.PushPopupAsync(new ConfigLeitorPop(FontSize, ColorType, FontType, this));
                IsBusy = false;
            }
        }

        async void ExecuteLoadItemsCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            htmArquivo = ResizeHtmlCss(Leitor.htmlArqs);

            try
            {
                if (!String.IsNullOrEmpty(htmArquivo))
                {
                    EpubLivro.UriSource = htmArquivo;
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "EPUBViewModel", "ExecuteLoadItemsCommand()");
            }
            finally
            {
                EpubLivro.SearchVisible = false;
                IsBusy = false;
            }
        }



        private Epub epubLivro;
        public Epub EpubLivro
        {
            get { return epubLivro; }
            set
            {
                epubLivro = value;
                this.Notify(nameof(EpubLivro));
            }
        }


        private string ResizeHtmlCss(string pageHTML)
        {
            string html = "";
            string url = pageHTML;

            WebClient wc = new WebClient();
            

            try
            {

                using (Stream st = wc.OpenRead(new Uri(string.Format("file://{0}", url))))
                {
                    using (StreamReader sr = new StreamReader(st, Encoding.UTF8))
                    {
                        // return html
                        html = sr.ReadToEnd();

                        // se existir isso o livro ja foi precessado
                        if (!html.Contains(".epubView"))
                        {
                            var doc = new HtmlDocument();
                            doc.LoadHtml(html);

                            var style = @"</title>
                                            <style>
                                                a.modallink-footer {
                                                display: contents;
                                                text-decoration: underline !important;
                                                }

                                                .epubView {
                                                    position: fixed;
                                                    top: 0;
                                                    left: 0;
                                                    width: 100%;
                                                    column-fill: auto;
                                                    -webkit-column-gap: 0em;
                                                    -moz-column-gap: 0em;
                                                    column-gap: 0em;
                                                    overflow: hidden;
                                                    margin: 0;
                                                }
                                                .epubView>section {
                                                    -webkit-column-break-before: always;
                                                    break-before: column;
                                                }
                                                .navbar{    
                                                    height: 50px;
                                                    width: 100%;
                                                    position: fixed;
                                                    bottom: 0;
                                                    left: 0;
                                                    border-top: 1px solid #e7e7e7;
                                                    background-color: #f6f6f6;
                                                    padding: 16pt;
                                                    z-index: 1000;
                                                    -webkit-box-sizing: border-box;
                                                    -moz-box-sizing: border-box;
                                                    box-sizing: border-box;
                                                }
                                                .nav-progress{
                                                    width: 100%;
                                                    height: 100%;
                                                    display: block;
                                                    position: relative;
                                                }
                                                .nav-slider{
                                                    -webkit-appearance: none;
                                                    outline: none;
                                                    width: 100%;
                                                    height: 3pt;
                                                    position: absolute;
                                                    top: 50%;
                                                    left: 0;
                                                    background: #C4C4C4;
                                                    -webkit-transform: translateY(-50%);
                                                    -ms-transform: translateY(-50%);
                                                    -o-transform: translateY(-50%);
                                                    transform: translateY(-50%);
                                                }
                                                .nav-slider::-webkit-slider-thumb {
                                                    -webkit-appearance: none;
                                                    appearance: none;
                                                    height: 30px;
                                                    width: 30px;
                                                    background: #f6f6f6;
                                                    border: 3px solid #2497A2;
                                                    border-radius: 50%;
                                                }
                                                .nav-slider::-webkit-range-progress {
                                                    -webkit-appearance: none;
                                                    appearance: none;
                                                    background: #2497A2;
                                                }
                                                .navprog-fill{
                                                    position: absolute;
                                                    top: 0;
                                                    left: 0;
                                                    height: 100%;
                                                    width: 50%;
                                                    background-color: #2497A2;
                                                }
                                                .navprog-bullet{
                                                    height: 40px;
                                                    width: 40px;
                                                    position: absolute;
                                                    top: 50%;
                                                    left: 50%;
                                                    background: transparent;
                                                    margin-top: -20px;
                                                    margin-left: -20px;
                                                    cursor: -webkit-grab;
                                                    cursor: grab;
                                                }
                                                .navprog-bullet:after{
                                                    content: '';
                                                    display: block;
                                                    left: 50%;
                                                    top: 50%;
                                                    width: 13px;
                                                    height: 13px;
                                                    margin-top: -8px;
                                                    margin-left: -8px;
                                                    position: absolute;
                                                    border-radius: 50%;
                                                    background-color: #fff;
                                                    border: 3px solid #C4C4C4;
                                                }

                                            </style>";


                            // scripts do search
                            var scriptSearch =
                                "<script src='../Scripts/mark.min.js' charset='UTF-8'></script>" +
                                "<script src='../Scripts/mark.js' charset='UTF-8'></script>" +
                                "<script src='../Scripts/mark.es6.js' charset='UTF-8'></script> " +
                                "<script src='../Scripts/mark.es6.min.js' charset='UTF-8'></script>" +
                                "<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js'></script>";
                              

                            // colco o css que separa por colunas
                            html = html.Replace("</title>",style + scriptSearch);


                            var divSearch = 
                                "<div id='SearchEpub' style='display:none;'>" +
                                    "<input type='text' name='keyword' placeholder='Pesquisar...'>" +
                                    "<input type='checkbox' name='opt[]' value='separateWordSearch' checked hidden='true'>" +
                                    "<input type='checkbox' name='opt[]' value='diacritics' checked hidden='true'> " +
                                "</div>";

                            // gero as paginas dos livros
                            html = html.Replace("<div>", divSearch + @"<p hidden id='myRetornoScroll'></p><p hidden id='myTotalScroll'></p>" +
                                                "<div id='xEpubView' class='epubView' onscroll='myFunctionScrolling()'>");


                            var navbarDiv = "<div class='navbar'>" +
                                                "<div class='nav-progress'>" +
                                                    "<input type='range' min='1' max='100' value='1' class='nav-slider'>" +
                                                "</div>" +
                                            "</div>";


                            var scriptSearchJs = "<script>" +
                                "var markInstance = new Mark(document.querySelector('.epubView'));" +
                                "var keywordInput = document.querySelector(\"input[name = 'keyword']\");" +
                                "var optionInputs = document.querySelectorAll(\"input[name = 'opt[]']\");" +
                                "function performMark() {" +
                                    "var keyword = keywordInput.value;" +

                                    "var options = {};" +
                                    "[].forEach.call(optionInputs, function(opt) {" +
                                    "options[opt.value] = opt.checked;" +
                                    "});" +

                                    "markInstance.unmark({" +
                                    "done: function(){" +
                                        "markInstance.mark(keyword, options);" +
                                    "}" +
                                    "});" +

                                "};" +

                                "keywordInput.addEventListener('input', performMark);" +
                                "for (var i = 0; i < optionInputs.length; i++) {" +
                                    "optionInputs[i].addEventListener('change', performMark);" +
                                "}" +

                                "</script>";

                          
                            var scriptSlider = @"<script src='../Scripts/plataforma.js' type='text/javascript'></script>
                                                 <script src='../Scripts/swipe_gesture.js' type='text/javascript'></script> " +
                                                @"<script>

                                                    function myFunctionScrolling() {
                                                        var elmnt = document.getElementById('xEpubView').scrollLeft;
                                                        document.getElementById('myRetornoScroll').innerHTML = elmnt;

                                                        var elmntTotal =   document.getElementById('xEpubView').scrollWidth;
                                                        document.getElementById('myTotalScroll').innerHTML = elmntTotal;

                                                    }

                                                </script>";

                            html = html.Replace("</body>", navbarDiv + scriptSlider + "</body>" + scriptSearchJs);

                            Debug.WriteLine(html);

                            // pagina modificada html
                            string paginaHTML = html;

                            // cria um novo arquivo ja redimencionado para o padrão desse device
                            File.Delete(url);
                            string path = Leitor.caminho + "/Content/Conteudo.html";
                            using (StreamWriter sw = File.CreateText(path))
                            {

                                sw.WriteLine(paginaHTML);
                            }

                            return path;
                        }

                        return url;
                    }
                }
            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "EPUBViewModel", "ResizeHtmlCss()");
                return null;
            }

        }

      
    }
}

