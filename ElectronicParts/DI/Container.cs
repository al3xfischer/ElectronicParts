// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="Container.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Container class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.DI
{
    using System;
    using System.IO;
    using ElectronicParts.Services.Implementations;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.ViewModels;
    using GuiLabs.Undo;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Includes all services used in the application.
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// The provider for all services.
        /// </summary>
        private static IServiceProvider provider;

        /// <summary>
        /// Initializes static members of the <see cref="Container"/> class.
        /// </summary>
        static Container()
        {
            IServiceCollection services = new ServiceCollection();

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<PreferencesViewModel>();

            // Services
            services.AddSingleton<IAssemblyService, AssemblyService>();
            services.AddSingleton<IPinConnectorService, PinConnectorService>();
            services.AddSingleton<IExecutionService, ExecutionService>();
            services.AddSingleton<INodeSerializerService, NodeSerializerService>();
            services.AddSingleton<AssemblyBinder>();
            services.AddSingleton<IGenericTypeComparerService, GenericTypeComparerService>();
            services.AddSingleton<IAssemblyNameExtractorService, AssemblyNameExtractorService>();
            services.AddTransient<INodeValidationService, NodeValidationService>();
            services.AddSingleton<INodeCopyService, NodeCopyService>();
            services.AddSingleton<ActionManager>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddTransient<IPinCreatorService, PinCreatorService>();
            services.AddSingleton<IConnectorHelperService, ConnectorHelperService>();

            // Logging
            services.AddLogging();

            provider = services.BuildServiceProvider();

            // Loggin configuration
            provider.GetService<ILoggerFactory>().AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logs/electronicparts.log"), minimumLevel: LogLevel.Trace, fileSizeLimitBytes: 1024 * 1024 * 3);
        }

        /// <summary>
        /// Gets an item out of the service provider.
        /// </summary>
        /// <typeparam name="TItem">The type of the wanted item.</typeparam>
        /// <returns>The wanted item.</returns>
        public static TItem Resolve<TItem>()
        {
            return provider.GetService<TItem>();
        }
    }
}
