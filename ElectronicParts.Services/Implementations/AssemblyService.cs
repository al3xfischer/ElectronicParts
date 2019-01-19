// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="PinConnectorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>

// <summary>Represents the PinConnectorService class of the ElectronicParts programm</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using ElectronicParts.Services.Interfaces;
    using Shared;

    /// <summary>
    /// Represents the <see cref="AssemblyService"/> class of the ElectronicParts application.
    /// Implements the <see cref="ElectronicParts.Services.Assemblies.IAssemblyService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Assemblies.IAssemblyService" />
    public class AssemblyService : IAssemblyService
    {
        /// <summary>
        /// Represents the Assembly path.
        /// </summary>
        private readonly string assemblyPath;

        /// <summary>
        /// The <list type="IDisplayableNode"/> 
        /// </summary>
        private List<IDisplayableNode> nodeList;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyService"/> class.
        /// </summary>
        public AssemblyService()
        {
            // Generating the assembly path in the same folder as the exe.
            // C:\Programme\ElectronicParts\electronicParts.exe
            // C:\Programme\ElectronicParts\assemblies\????.dll
            this.assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies");
            Directory.CreateDirectory(this.assemblyPath);
        }

        /// <summary>
        /// Gets the available nodes.
        /// </summary>
        /// <value>The List of available lists.</value>
        public IEnumerable<IDisplayableNode> AvailableNodes
        {
            get => this.nodeList.AsEnumerable();
        }

        /// <summary>
        /// Loads the assemblies in the assembly paths.
        /// </summary>
        /// <returns>A Task for awaiting this operation.</returns>
        public async Task LoadAssemblies()
        {
            await Task.Run(() =>
            {
            // Creating temporary nodelist.
            List<IDisplayableNode> loadedNodes = new List<IDisplayableNode>();
            IEnumerable<FileInfo> files;
            try
            {
                // Finding all files in the assembly directory with .dll extension.
                files = Directory.GetFiles(this.assemblyPath).Select(path => new FileInfo(path))
                .Where(file => file.Extension == ".dll");
            }
            catch (Exception e)
            {
                // TODO proper exception-handeling
                Debug.WriteLine($"{e.Message}");
                return;
            }

            // Iterating over every dll-file and finding dlls with types that implement IDisplayableNode.
            foreach (var file in files)
            {
                try
                {
                    Assembly assembly;
                    var x = Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(file.Name) + ".pdb");
                        // Loading file into mainmemory and loading assembly.
                        if (File.Exists(Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(file.Name) + ".pdb")))
                        {
                            assembly = Assembly.Load(File.ReadAllBytes(file.FullName), File.ReadAllBytes(Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(file.Name) + ".pdb")));
                        }
                        else
                        {
                            assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
                        }

                        

                        // Getting all Types that implement IDisplayableNode interface.
                        var types = assembly.GetTypes();
                        var availableNodes = types
                            .Where(type => type.GetInterfaces()
                            .Contains(typeof(IDisplayableNode)));

                        // Iterating over every type and adding an instance to the AvailableNodeslist.
                        foreach (var node in availableNodes)
                        {
                            loadedNodes.Add(Activator.CreateInstance(node) as IDisplayableNode);
                        }
                    }
                    catch (Exception e)
                    {
                        // TODO proper exception-handeling
                        Debug.WriteLine($"{e.Message}");
                    }
                }

                // writing loadedNodes into availableNodes immutableList.
                this.nodeList = loadedNodes;
            });
        }
    }
}
