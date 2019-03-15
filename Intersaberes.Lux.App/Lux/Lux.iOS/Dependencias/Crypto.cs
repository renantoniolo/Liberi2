using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Lux.iOS.Dependencias
{
    public class Crypto
    {
        public void Decript(string input, string output)
        {
            Config config = new Config();

            FileStream inStream, outStream;
            CryptoStream CryStream;
            TripleDESCryptoServiceProvider TDC = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Byte[] byteHash, byteTexto;

            inStream = new FileStream(input, FileMode.Open, FileAccess.Read);
            outStream = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);

            byteHash = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(config.DeviceID));
            byteTexto = File.ReadAllBytes(input);

            md5.Clear();

            TDC.Key = byteHash;
            TDC.Mode = CipherMode.ECB;

            CryStream = new CryptoStream(outStream, TDC.CreateDecryptor(), CryptoStreamMode.Write);

            int bytesRead;
            int position = 0;
            long length = inStream.Length;

            while (position < length)
            {
                bytesRead = inStream.Read(byteTexto, 0, byteTexto.Length);
                position += bytesRead;

                CryStream.Write(byteTexto, 0, bytesRead);
            }

            inStream.Close();
            outStream.Close();
        }

        public string DecriptString(string Message)
        {
            Config config = new Config();

            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(config.DeviceID));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }
    }
}
