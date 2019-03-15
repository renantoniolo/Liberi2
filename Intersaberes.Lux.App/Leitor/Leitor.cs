using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AppLeitor
{
    public static class Leitor
    {
        //Variaveis
        public static string caminho { get; set; }
        public static bool isAmostra { get; set; } = false;
        private static int nPastaHtml { get; set; }
        private static int nPastaImg { get; set; }
        private static int nPastaAudio { get; set; }
        private static int nPastaVideo { get; set; }
        private static int nPastaGrafico { get; set; }
        private static int nPastaTabela { get; set; }
        public static string Titulo { get; set; }

        //Listas
        public static List<string> pastas { get; set; }
        public static string htmlArqs { get; set; }
        public static List<string> imgArqs { get; set; }
        public static List<string> audioArqs { get; set; }
        public static List<string> videoArqs { get; set; }
        public static List<string> graficoArqs { get; set; }
        public static List<string> tabelaArqs { get; set; }

        public static void LerDiretorio(string caminho)
        {
            Leitor.caminho = caminho;
            Leitor.Titulo = caminho.Split('/').Last();

            pastas = new List<string>();
            var tempPastas = Directory.GetDirectories(caminho);
            foreach (var pasta in tempPastas) pastas.Add(pasta);

            if (isAmostra)
            {
                nPastaHtml = 0;
                nPastaImg = 3;
                nPastaTabela = 7;
            }
            else
            {
                nPastaAudio = 0;
                nPastaHtml = 1;
                nPastaGrafico = 3;
                nPastaImg = 5;
                nPastaTabela = 8;
                nPastaVideo = 9;
            }

            LerHtml();
            LerImg();
            LerAudio();
            LerVideo();
            LerGrafico();
            LerTabela();
        }


        private static void LerHtml()
        {

            try{

                var tempHtml = Directory.GetFiles(pastas[nPastaHtml]);

                for (int i = 0; i < tempHtml.Length; i++)
                    htmlArqs = tempHtml[i];

            }
            catch(Exception ex){
                
            }

        }
        private static void LerImg()
        {
            imgArqs = new List<string>();
            var tempImg = Directory.GetFiles(pastas[nPastaImg]);
            foreach (var image in tempImg) imgArqs.Add(image);
        }
        private static void LerAudio()
        {
            audioArqs = new List<string>();
            if (!isAmostra)
            {
                var tempAudio = Directory.GetFiles(pastas[nPastaAudio]);
                foreach (var audio in tempAudio) audioArqs.Add(audio);
            }
        }
        private static void LerVideo()
        {
            videoArqs = new List<string>();
            if (!isAmostra)
            {
                var tempVideo = Directory.GetFiles(pastas[nPastaVideo]);
                foreach (var video in tempVideo) videoArqs.Add(video);
            }
        }
        private static void LerGrafico()
        {
            graficoArqs = new List<string>();
            var tempGrafico= Directory.GetFiles(pastas[nPastaGrafico]);
            foreach (var grafico in tempGrafico) graficoArqs.Add(grafico);
        }
        private static void LerTabela()
        {
            tabelaArqs = new List<string>();
            var tempTabela = Directory.GetFiles(pastas[nPastaTabela]);
            foreach (var tabela in tempTabela) tabelaArqs.Add(tabela);
        }
    }
}
