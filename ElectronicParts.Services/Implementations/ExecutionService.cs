// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="ExecutionService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ExecutionService class of the ElectronicParts programm</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using ElectronicParts.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Shared;

    /// <summary>
    /// Represents the <see cref="ExecutionService"/> class of the ElectronicParts application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.IExecutionService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.IExecutionService" />
    public class ExecutionService : IExecutionService
    {
        /// <summary>
        /// Represents the Logger.
        /// </summary>
        private readonly ILogger<ExecutionService> logger;

        /// <summary>
        /// Represents the Frames per second currently requested.
        /// </summary>
        private int framesPerSecond;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionService"/> class.
        /// </summary>
        public ExecutionService()
        {
            this.FramesPerSecond = 60;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ExecutionService(ILogger<ExecutionService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Is invoked when the <see cref="IsEnabled" /> value changes.
        /// </summary>
        public event EventHandler OnIsEnabledChanged;

        /// <summary>
        /// Gets or sets the amount of executions per second.
        /// </summary>
        /// <value>The amount of executions per second.</value>
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

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>True if this instance is enabled; otherwise, false.</value>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets the amount of time it took to complete a loop.
        /// </summary>
        /// <value>The amount of time it took to complete a loop.</value>
        public long MillisecondsPerLoop { get; private set; }

        /// <summary>
        /// Executes one step.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns>A task which can be awaited.</returns>
        public async Task ExecuteOnce(IEnumerable<INode> nodes)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(
                    nodes,
                    node =>
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

        /// <summary>
        /// Starts the execution loop.
        /// </summary>
        /// <param name="nodes">The nodes to simulate.</param>
        /// <param name="callback">A callback method.</param>
        /// <returns>A task which can be awaited.</returns>
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
                    this.MillisecondsPerLoop = watch.ElapsedMilliseconds;
                    await Task.Delay(Math.Max((int)waitingTime, 1));
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
        /// Fires the on is enabled changed event.
        /// </summary>
        private void FireOnIsEnabledChanged()
        {
            this.OnIsEnabledChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
