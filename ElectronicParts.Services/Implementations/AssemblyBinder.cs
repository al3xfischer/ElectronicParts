// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="AssemblyBinder.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the AssemblyBinder class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="AssemblyBinder" />
    /// </summary>
    public class AssemblyBinder : SerializationBinder
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<AssemblyBinder> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBinder"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{AssemblyBinder}"/></param>
        public AssemblyBinder(ILogger<AssemblyBinder> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Represents a method which is used to find the assembly within the currently loaded assemblies and gets the requested type.
        /// </summary>
        /// <param name="fullAssemblyString">The full assembly string.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The required type.</returns>
        public override Type BindToType(string fullAssemblyString, string typeName)
        {
            Type wantedType = null;
            try
            {
                string assemblyName = fullAssemblyString.Split(',')[0];
                Assembly[] currentlyLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse().ToArray();

                foreach (Assembly assembly in currentlyLoadedAssemblies)
                {
                    if (assembly.FullName.Split(',')[0] == assemblyName)
                    {
                        wantedType = assembly.GetType(typeName);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Unexpected error in {this}");
                Debug.WriteLine(e.Message);
            }

            return wantedType;
        }
    }
}
