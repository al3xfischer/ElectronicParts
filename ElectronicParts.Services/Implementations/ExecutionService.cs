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
    using Microsoft.Extensions.Logging;
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
        private long passedMiliseconds;

        private ILogger<ExecutionService> logger;

        public event EventHandler OnIsEnabledChanged;

        public ExecutionService(ILogger<ExecutionService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>true if this instance is enabled; otherwise, false.</value>
        public bool IsEnabled { get; private set; }

        public long MillisecondsPerLoop { get => this.passedMiliseconds; }

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
        public async Task StartExecutionLoop(IEnumerable<INode> nodes, Action callback)
        {
            if (!this.IsEnabled)
            {
                this.IsEnabled = true;
                this.FireOnIsEnabledChanged();
                Stopwatch watch = new Stopwatch();

                while (this.IsEnabled)
                {
                    watch.Reset();
                    watch.Start();
                    try
                    {
                        await this.ExecuteOnce(nodes);
                        callback?.Invoke();
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, $"Unexpected error in {nameof(callback)} ({nameof(this.StartExecutionLoop)}).");
                        Debug.WriteLine(e.Message);
                    }
                    watch.Stop();
                    var waitingTime = (1000 / this.FramesPerSecond) - watch.ElapsedMilliseconds;
                    this.passedMiliseconds = watch.ElapsedMilliseconds;
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
            if (this.IsEnabled)
            {
                this.IsEnabled = false;
                this.FireOnIsEnabledChanged();
            }
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
                    try
                    {
                        node.Execute();
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, $"An error occurred in an execution method of a node ({node.ToString()})!");
                    }
                });
            });
        }

        private void FireOnIsEnabledChanged()
        {
            this.OnIsEnabledChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
