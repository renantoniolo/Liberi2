using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace Lux.Helpers
{
    public static class Function
    {
        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }

        public static void AppCenterCrashes(Exception exception, string classe, string methodo_prop)//Gera os craches no Azure App Center
        {
            if (methodo_prop.Contains("()"))
            {
                var properties = new Dictionary<string, string> {
                    { "Classe", classe },
                    { "Methodo", methodo_prop }
                };
                Crashes.TrackError(exception, properties);
            }
            else
            {
                var properties = new Dictionary<string, string> {
                    { "Classe", classe },
                    { "Propriedade", methodo_prop }
                };
                Crashes.TrackError(exception, properties);
            }
        }
    }
}
