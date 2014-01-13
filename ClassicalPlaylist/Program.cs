using System;
using System.Runtime.InteropServices;

using iTunesLib;

namespace ClassicalPlaylist
{
    class Program
    {
        /*
        ** License:
        ** You may use this software freely for personal use.
        ** This software is provided AS IS with no warrenty of any kind, implied or otherwise.         
        */
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            if (args.Length == 0)
            {
                string programName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                Console.WriteLine("Usage: {0} <playlist full path name>", programName);
                return;
            }

            string playlistFullPath = Environment.ExpandEnvironmentVariables(args[0]);
            var iTunesApp = new iTunesAppClass();
            try
            {
                IITLibraryPlaylist mainLibrary = iTunesApp.LibraryPlaylist;
                var classicalLibrary = new ClassicalLibrary(mainLibrary);
                classicalLibrary.CreatePlaylist(playlistFullPath);
            }
            finally
            {
                Marshal.ReleaseComObject(iTunesApp);
            }
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Console.Error.WriteLine("Exception caught: {0}", unhandledExceptionEventArgs.ExceptionObject);
        }
    }
}
