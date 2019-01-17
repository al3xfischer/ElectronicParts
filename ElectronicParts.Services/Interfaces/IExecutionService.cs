// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IExecutionService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>

// <summary>Represents the IExecutionService class of the ElectronicParts.Services programm</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    /// <summary>
    /// Represents the <see cref="IExecutionService"/> interface.
    /// </summary>
    public interface IExecutionService
    {
        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>true if this instance is enabled; otherwise, false.</value>
        bool IsEnabled { get; }

        int FramesPerSecond { get; set; }

        /// <summary>
        /// Executes one step.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns>Task.</returns>
        Task ExecuteOnce(IEnumerable<INode> nodes);

        /// <summary>
        /// Starts the execution loop.
        /// </summary>
        /// <param name="nodes">The nodes to simulate</param>
        /// <returns>Task.</returns>
        Task StartExecutionLoop(IEnumerable<INode> nodes);

        /// <summary>
        /// Stops the execution loop.
        /// </summary>
        void StopExecutionLoop();
    }
}