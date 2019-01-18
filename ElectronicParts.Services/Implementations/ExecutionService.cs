// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="ExecutionService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>

// <summary>Represents the ExecutionService class of the ElectronicParts.Services programm</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using ElectronicParts.Services.Interfaces;
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the <see cref="ExecutionService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Implementations.IExecutionService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Implementations.IExecutionService" />
    public class ExecutionService : IExecutionService
    {
        private int framesPerSecond;

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>true if this instance is enabled; otherwise, false.</value>
        public bool IsEnabled { get; private set; }

        public int FramesPerSecond
        {
            get => this.framesPerSecond;
            set
            {
                if (value > 0)
                {
                    this.framesPerSecond = value;
                }
            }
        }


        public ExecutionService()
        {
            this.FramesPerSecond = 60;
        }

        /// <summary>
        /// Starts the execution loop.
        /// </summary>
        /// <param name="nodes">The nodes to simulate.</param>
        /// <returns>Task.</returns>
        public async Task StartExecutionLoop(IEnumerable<INode> nodes, Func<Task> asyncCallback)
        {
            if (!this.IsEnabled)
            {
                this.IsEnabled = true;
                Stopwatch watch = new Stopwatch();

                while (this.IsEnabled)
                {
                    watch.Reset();
                    watch.Start();
                    try
                    {
                        await this.ExecuteOnce(nodes);
                        await asyncCallback?.Invoke();
                    }
                    catch (Exception e)
                    {
                        // TODO Proper Exception handeling
                        Debug.WriteLine(e.Message);
                    }
                    watch.Stop();
                    var waitingTime = (60000 / this.FramesPerSecond) - watch.ElapsedMilliseconds;
                    try
                    {
                        await Task.Delay(Math.Max((int)waitingTime, 1));
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Stops the execution loop.
        /// </summary>
        public void StopExecutionLoop()
        {
            this.IsEnabled = false;
        }

        /// <summary>
        /// Executes one step.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns>Task.</returns>
        public async Task ExecuteOnce(IEnumerable<INode> nodes)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(nodes, node =>
                {
                    node.Execute();
                });
            });
        }
    }
}
