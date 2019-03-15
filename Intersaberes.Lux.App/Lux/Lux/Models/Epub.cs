using Lux.ViewsModels;

namespace Lux.Models
{
    public class Epub : BaseViewModel
    {
        private string uriSource;
        public string UriSource
        {
            get { return uriSource; }
            set { SetProperty(ref uriSource, value); }
        }

        private int fontSize = 1;
        public int FontSize
        {
            get { return fontSize; }
            set { SetProperty(ref fontSize, value); }
        }

        private int fontType = 1;
        public int FontType
        {
            get { return fontType; }
            set { SetProperty(ref fontType, value); }
        }

        private int colorType = 3;
        public int ColorType
        {
            get { return colorType; }
            set { SetProperty(ref colorType, value); }
        }

        private bool searchVisible = false;
        public bool SearchVisible
        {
            get { return searchVisible; }
            set { SetProperty(ref searchVisible, value); }
        }


        private bool scrollPercenteReturn = false;
        public bool ScrollPercenteReturn
        {
            get { return scrollPercenteReturn; }
            set { SetProperty(ref scrollPercenteReturn, value); }
        }

    }
}
