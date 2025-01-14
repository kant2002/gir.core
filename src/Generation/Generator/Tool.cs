﻿using System;
using System.IO;
using System.Linq;
using GirLoader;

namespace Generator
{
    public class Tool
    {
        // Example Usage: dotnet run -- ../gir-files/Gst-1.0.gir
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- file1 file2 file3 ...");
                return 0;
            }

            foreach (var arg in args)
            {
                if (!System.IO.File.Exists(arg))
                {
                    Log.Error($"Could not find file at '{arg}'. Make sure it is a valid path");
                    return -1;
                }
            }

            try
            {
                new Generator().Write(args.Select(x => new FileInfo(x)));
                return 0;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                Log.Error("An error occured during processing. Please save a copy of your log output and open an issue at: https://github.com/gircore/gir.core/issues/new");

                return -1;
            }
        }
    }
}
