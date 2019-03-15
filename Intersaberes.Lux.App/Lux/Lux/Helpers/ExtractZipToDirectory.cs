using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Lux.Interfaces;
using Xamarin.Forms;

namespace Lux.Helpers
{
    public static class ExtractZipToDirectory
    {
       
        public static void ExtractToDirectoryFile(string zipPath, string extractPath){

            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "ExtractZipToDirectory", "ExtractToDirectoryFile()");
            }

        }

    }
}
