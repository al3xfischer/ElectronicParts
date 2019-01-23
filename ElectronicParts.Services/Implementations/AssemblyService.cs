// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="AssemblyService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the AssemblyService class of the ElectronicParts.Services project</summary>
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
    using Microsoft.Extensions.Logging;
    using Shared;

    /// <summary>
    /// Represents the <see cref="AssemblyService" /> class of the ElectronicParts application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.IAssemblyService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.IAssemblyService" />
    public class AssemblyService : IAssemblyService
    {
        /// <summary>
        /// Represents the Assembly path.
        /// </summary>
        private readonly string assemblyPath;

        /// <summary>
        /// Contains the logger.
        /// </summary>
        private readonly ILogger<AssemblyService> logger;

        /// <summary>
        /// The validation service.
        /// </summary>
        private readonly INodeValidationService validationService;

        /// <summary>
        /// Represents the list of available nodes.
        /// </summary>
        private List<IDisplayableNode> nodeList;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyService" /> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="validationService">The validation service used for validating a given assembly is loadable.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if either the injected logger or the <see cref="NodeValidationService"/> is null.
        /// </exception>
        public AssemblyService(ILogger<AssemblyService> logger, INodeValidationService validationService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

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
                this.logger.LogError(e, $"Error while getting files from {this.assemblyPath}");
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

                        // Loading file into mainmemory and loading assembly. If there is a pdb file load that too
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
                            .Contains(typeof(IDisplayableNode)) &&
                            type.IsDefined(typeof(SerializableAttribute)));

                        // Iterating over every type and adding an instance to the AvailableNodeslist.
                        foreach (var node in availableNodes)
                        {
                            var nodeInstance = Activator.CreateInstance(node) as IDisplayableNode;
                            if (this.validationService.Validate(nodeInstance))
                            {
                                loadedNodes.Add(nodeInstance);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, $"Error while retrieving {nameof(IDisplayableNode)} types of .dll files");
                        Debug.WriteLine($"{e.Message}");
                    }
                }

                // Writing loadedNodes into availableNodes immutableList.
                this.nodeList = loadedNodes;
            });
        }
    }
}
