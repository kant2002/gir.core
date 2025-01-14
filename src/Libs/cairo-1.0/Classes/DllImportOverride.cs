﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable enable

namespace cairo.Native
{
    // Manual override of the DllImport class so we can handle
    // cairo being in multiple shared libraries/dlls. See the
    // method 'TryGetOsDependentLibraryName' for an explanation
    // of what is happening.
    internal static class DllImportOverride
    {
        #region Fields

        // libcairo (for manually written code)
        private const string _windowsDllName = "libcairo-2.dll";
        private const string _linuxDllName = "libcairo.so.2";
        private const string _osxDllName = "libcairo.2.dylib";

        // libcairo-gobject (for autogenerated/introspection code)
        private const string _cgoWindowsDllName = "libcairo-gobject-2.dll";
        private const string _cgoLinuxDllName = "libcairo-gobject.so.2";
        private const string _cgoOsxDllName = "libcairo-gobject.2.dylib";

        private static readonly Dictionary<string, IntPtr> _cache = new();

        #endregion

        #region Methods

        public static void Initialize()
        {
            NativeLibrary.SetDllImportResolver(typeof(DllImportOverride).Assembly, ImportResolver);
        }

        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (_cache.TryGetValue(libraryName, out var cachedLibHandle))
                return cachedLibHandle;

            if (!TryGetOsDependentLibraryName(libraryName, out var osDependentLibraryName))
                return IntPtr.Zero;

            if (NativeLibrary.TryLoad(osDependentLibraryName, assembly, searchPath, out IntPtr libHandle))
            {
                _cache[libraryName] = libHandle;
                return libHandle;
            }

            // Fall back to default dll search mechanic
            return IntPtr.Zero;
        }

        private static bool TryGetOsDependentLibraryName(string libraryName, [NotNullWhen(true)] out string? osDependentLibraryName)
        {
            // Cairo, as used by GTK, is spread across two (maybe more?)
            // shared libraries. These are 'cairo' - the library itself,
            // and 'cairo-gobject' - interop code for using with GDK.

            // We map the string 'libraryName' to the correct shared library:
            //   * "cairo" -> "libcairo-gobject"
            //   * "cairo-graphics" -> "libcairo"

            // Confusingly, "cairo" refers to "cairo-gobject" as this is
            // the library specified in "cairo-1.0.gir".
            if (libraryName == "cairo")
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    osDependentLibraryName = _cgoWindowsDllName;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    osDependentLibraryName = _cgoOsxDllName;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    osDependentLibraryName = _cgoLinuxDllName;
                else
                    throw new Exception("Unknown platform");

                return true;
            }

            // This is the actual "libcairo" which defines the
            // functions, structs, etc that we want.
            if (libraryName == "cairo-graphics")
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    osDependentLibraryName = _windowsDllName;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    osDependentLibraryName = _osxDllName;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    osDependentLibraryName = _linuxDllName;
                else
                    throw new Exception("Unknown platform");

                return true;
            }

            // Library name not recognised, return
            osDependentLibraryName = null;
            return false;
        }

        #endregion
    }
}
