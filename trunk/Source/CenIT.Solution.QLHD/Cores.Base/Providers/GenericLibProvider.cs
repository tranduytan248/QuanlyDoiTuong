using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using TSFramework.App.Processors;

namespace Cores.Base.Providers
{
    public static class GenericLibProvider<T>
    {
        public static T LoadLibByPath(string path, bool isAbsolutePath = false)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return default;
                if (!isAbsolutePath)
                    path = HostingEnvironment.MapPath(path);

                if (string.IsNullOrEmpty(path)) return default;

                var assembly = Assembly.Load(File.ReadAllBytes(path));
                var pluginType = typeof(T);
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                        if (type.IsInterface || type.IsAbstract)
                        {
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                var plugin = (T)Activator.CreateInstance(type);
                                return plugin;
                            }
                        }
                }

                return default;
            }
            catch (Exception ex)
            {
                AppProcessor.Logger.Error(ex);
                return default;
            }
        }

        public static ICollection<T> LoadLib(string path)
        {
            try
            {
                path = HostingEnvironment.MapPath("/" + path);
                if (Directory.Exists(path))
                {
                    var libFileNames = Directory.GetFiles(path, "*.dll");

                    ICollection<Assembly> assemblies = new List<Assembly>(libFileNames.Length);
                    foreach (var dllFile in libFileNames)
                    {
                        //AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        //Assembly assembly = Assembly.Load(an);
                        var assembly = Assembly.Load(File.ReadAllBytes(dllFile));
                        assemblies.Add(assembly);
                    }

                    var pluginType = typeof(T);
                    ICollection<Type> pluginTypes = new List<Type>();
                    foreach (var assembly in assemblies)
                        if (assembly != null)
                        {
                            var types = assembly.GetTypes();

                            foreach (var type in types)
                                if (type.IsInterface || type.IsAbstract)
                                {
                                }
                                else
                                {
                                    if (type.GetInterface(pluginType.FullName) != null) pluginTypes.Add(type);
                                }
                        }

                    ICollection<T> plugins = new List<T>(pluginTypes.Count);
                    foreach (var type in pluginTypes)
                    {
                        var plugin = (T)Activator.CreateInstance(type);
                        plugins.Add(plugin);
                    }

                    return plugins;
                }

                return null;
            }
            catch (Exception ex)
            {
                AppProcessor.Logger.Error(ex);
                return null;
            }
        }
    }
}