using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;

namespace Abc.Zebus.TinyHost
{
    public class AssemblyScanner  
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AssemblyScanner));
        private static readonly ConcurrentDictionary<string, List<Assembly>> _assemblyCache = new ConcurrentDictionary<string, List<Assembly>>();
         
        public static IEnumerable<Assembly> GetAssembliesInFolder(string path, Func<string, bool> assemblyNameFilter = null)
        {
            return _assemblyCache.GetOrAdd(path, p => LoadAssemblies(p, assemblyNameFilter));
        }

        private static List<Assembly> LoadAssemblies(string path, Func<string, bool> assemblyNameFilter = null)
        {
            var assemblies = new List<Assembly>();

            _logger.Info($"Scanning assemblies in : {path}");

            assemblies.AddRange(GetAssembliesInFolder(path, "*.exe", assemblyNameFilter));
            assemblies.AddRange(GetAssembliesInFolder(path, "*.dll", assemblyNameFilter));

            return assemblies;
        }

        [DebuggerStepThrough]
        private static IEnumerable<Assembly> GetAssembliesInFolder(string path, string searchPattern, Func<string, bool> assemblyNameFilter = null)
        {
            var assemblies = new List<Assembly>();
            foreach (var filePath in System.IO.Directory.GetFiles(path, searchPattern))
            {
                try
                {
                    var fileName = Path.GetFileName(filePath);
                    if (assemblyNameFilter != null && !assemblyNameFilter(fileName))
                    {
                        _logger.Info($"Skipping : {fileName}");
                        continue;
                    }

                    _logger.Info($"Loading : {fileName}");
                    assemblies.Add(Assembly.LoadFrom(filePath));
                }
                catch (ReflectionTypeLoadException ex)
                {
                    foreach (var loaderException in ex.LoaderExceptions)
                        _logger.Warn($"Problem with loading {filePath}", loaderException);
                }
                catch (BadImageFormatException)
                {
                    _logger.Warn($"The assembly is not in the correct format (x86/x64/AnyCpu) {filePath}");
                }
                catch (FileNotFoundException ex)
                {
                    _logger.Warn($"Unable to load {ex.FileName}");
                }
                catch (FileLoadException ex)
                {
                    _logger.Warn($"Unable to load managed assembly {filePath}: {ex.Message}");
                }
            }
            return assemblies;
        }
    }
}