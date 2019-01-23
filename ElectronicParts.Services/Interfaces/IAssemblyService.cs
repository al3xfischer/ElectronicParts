// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IAssemblyService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IAssemblyService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    /// <summary>
    /// A interface used to implement classes which allow to load assemblies.
    /// </summary>
    public interface IAssemblyService
    {
        /// <summary>
        /// Gets a collection of all <see cref="IDisplayableNode"/> instances saved in the loaded assemblies.
        /// </summary>
        /// <value>A collection of all <see cref="IDisplayableNode"/> instances saved in the loaded assemblies.</value>
        IEnumerable<IDisplayableNode> AvailableNodes { get; }

        /// <summary>
        /// Loads all assemblies.
        /// </summary>
        /// <returns>A await able task.</returns>
        Task LoadAssemblies();
    }
}