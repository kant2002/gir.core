﻿using System.IO;
using GirLoader;

namespace Generator
{
    internal static class IncludeResolver
    {
        private const string CacheDir = "../../../ext/gir-files";

        public static GirLoader.Input.Repository? Resolve(GirLoader.Output.Include include)
        {
            var fileName = $"{include.Name}-{include.Version}.gir";

            var path = Path.Combine(CacheDir, fileName);
            return File.Exists(path)
                ? new FileInfo(path).OpenRead().DeserializeGirInputModel()
                : null;
        }
    }
}
