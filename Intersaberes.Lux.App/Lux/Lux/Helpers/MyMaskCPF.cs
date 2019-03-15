using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Lux.Helpers
{
    public class MyMaskCPF : Entry
    {
        string text;

        public MyMaskCPF()
        {

            Keyboard = Keyboard.Numeric;
            TextColor = Color.Gray;
            TextChanged += Entry_TextChanged;

        }

        void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (((Entry)sender).Text.Length < 15)
                {
                    if (((Entry)sender).Text.Length == 3 || ((Entry)sender).Text.Length == 7)
                    {
                        text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
                        if (e.OldTextValue.Length == 2 || e.OldTextValue.Length == 6) ((Entry)sender).Text = text + ".";
                    }
                    else if (((Entry)sender).Text.Length == 11 && e.OldTextValue.Length == 10)
                    {
                        text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
                        ((Entry)sender).Text = text + "-";
                    }
                }
                else
                {
                    text = ((Entry)sender).Text.Remove(((Entry)sender).Text.Length - 1);
                    ((Entry)sender).Text = text;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao criar mask" + ex.Message);
            }

        }
    }
}
