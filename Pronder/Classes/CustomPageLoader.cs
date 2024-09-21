using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pronder.Interfaces;
using Windows.Storage;

namespace Pronder.Classes
{
    class CustomPageLoader
    {
        private string PluginDirectory;

        public CustomPageLoader(string pluginDirectory)
        {
            GetCustomPagesDirectory();
        }

        async void GetCustomPagesDirectory()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            string projectFolderName = "Projects";
            StorageFolder projectFolder = await storageFolder.GetFolderAsync(projectFolderName);

            PluginDirectory =  projectFolder.Path.ToString();
        }

        // Load all assemblies and find types implementing ICustomePage
        public List<ICustomePage> LoadPlugins()
        {
            List<ICustomePage> plugins = new List<ICustomePage>();

            // Get all DLL files in the directory
            string[] dllFiles = Directory.GetFiles(PluginDirectory, "*.dll");

            foreach (var dll in dllFiles)
            {
                Assembly assembly = Assembly.LoadFrom(dll);

                // Find all types that implement IPlugin in the assembly
                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(ICustomePage).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in pluginTypes)
                {
                    // Create an instance of the plugin
                    ICustomePage plugin = (ICustomePage)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }
            }

            return plugins;
        }
    }
}
