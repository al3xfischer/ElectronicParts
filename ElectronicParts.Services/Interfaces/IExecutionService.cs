// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IExecutionService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IExecutionService interface of the ElectronicParts programm</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared;

    /// <summary>
    /// Represents the <see cref="IExecutionService"/> interface.
    /// </summary>
    public interface IExecutionService
    {
        /// <summary>
        /// Is invoked when the <see cref="IsEnabled"/> value changes.
        /// </summary>
        event EventHandler OnIsEnabledChanged;

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>True if this instance is enabled; otherwise, false.</value>
        bool IsEnabled { get; }        

        /// <summary>
        /// Gets or sets the amount of executions per second. 
        /// </summary>
        /// <value>The amount of executions per second. </value>
        int FramesPerSecond { get; set; }

        /// <summary>
        /// Gets the amount of time it took to complete a loop.
        /// </summary>
        /// <value>The amount of time it took to complete a loop.</value>
        long MillisecondsPerLoop { get; }

        /// <summary>
        /// Executes one step.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns>A await able task.</returns>
        Task ExecuteOnce(IEnumerable<INode> nodes);

        /// <summary>
        /// Starts the execution loop.
        /// </summary>
        /// <param name="nodes">The nodes to simulate.</param>
        /// <param name="callback">A callback method.</param>
        /// <returns>A await able task.</returns>
        Task StartExecutionLoop(IEnumerable<INode> nodes, Action callback);

        /// <summary>
        /// Stops the execution loop.
        /// </summary>
        void StopExecutionLoop();
    }
}