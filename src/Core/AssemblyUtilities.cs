using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Semver;

namespace OllamaClient.Core;

public static class AssemblyUtilities {
    public static Assembly GetAssembly(string assembly){

        var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
                var paths = new List<string>(runtimeAssemblies);
                foreach (var path in runtimeAssemblies) {
                    var assemblyName = Path.GetFileNameWithoutExtension(path);
                    if (assemblyName == assembly) {
                        return Assembly.LoadFrom(path);
                    }
                }

                return null;
    }

    public static SemVersion GetAssemblyVersion(string assemblyName)
    {
        var runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        var paths = new List<string>(runtimeAssemblies);
        if (paths == null)
        {
            return SemVersion.Parse("1.0.0", SemVersionStyles.Strict);
        }

                    var resolver = new PathAssemblyResolver(paths);
            var mlc = new MetadataLoadContext(resolver);


         using (mlc)
            {
                var assembly = mlc.LoadFromAssemblyPath(assemblyName);
                var productVersion = "1.0.0";

                var attributes = assembly.CustomAttributes.ToList();
                for (var i = 0; i < attributes.Count; i++)
                {
                    var a = attributes[i];

                    try
                    {
                        var t = a.AttributeType.Name;


                        if (t == nameof(AssemblyInformationalVersionAttribute))
                        {
                            productVersion = a.ConstructorArguments.First().Value as string;
                            break;
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        // We are missing the required dependency assembly.
                        Console.WriteLine($"Error while getting attribute type: {ex.Message}");
                    }
                }

                var version = SemVersion.Parse(productVersion, SemVersionStyles.Strict);
                return version;
            }
    }
}