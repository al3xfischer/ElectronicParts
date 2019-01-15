using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace ElectronicParts.Services.Assemblies
{
    public class AssemblyService : IAssemblyService
    {
        public List<IDisplayableNode> AvailableNodes { get; private set; }
        private readonly string assemblyPath;

        public AssemblyService()
        {
            // Generating the assembly path in the same folder as the exe.
            // C:\Programme\ElectronicParts\electronicParts.exe
            // C:\Programme\ElectronicParts\assemblies\????.dll
            this.assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies");
            this.AvailableNodes = new List<IDisplayableNode>();
            Directory.CreateDirectory(this.assemblyPath);
        }

        /// <summary>
        /// Loads the assemblies in the assembly paths.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task LoadAssemblies()
        {
            await Task.Run(() =>
            {
                IEnumerable<FileInfo> files;
                try
                {
                    // Finding all files in the assembly directory with .dll extension.
                    files = Directory.GetFiles(this.assemblyPath).Select(path => new FileInfo(path))
                    .Where(file => file.Extension == ".dll");
                }
                catch(Exception e)
                {
                    Debug.WriteLine("Directory not existing");
                    return;
                }
                
                

                // Iterating over every dll-file and finding dlls with types that implement IDisplayableNode.
                foreach (var file in files)
                {
                    try
                    {
                        // Loading Assembly.
                        var assembly = Assembly.LoadFrom(file.FullName);

                        // Getting all Types that implement IDisplayableNode interface.
                        var types = assembly.GetTypes();
                        var availableNodes = types
                            .Where(type => type.GetInterfaces()
                            .Contains(typeof(IDisplayableNode)));

                        // Iterating over every type and adding an instance to the AvailableNodeslist.
                        foreach (var node in availableNodes)
                        {
                            this.AvailableNodes.Add(Activator.CreateInstance(node) as IDisplayableNode);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Assembly loading error");
                    }
                }
            });
        }
    }
}
