using ElectronicParts.Services;
using ElectronicParts.Services.Interfaces;
﻿using ElectronicParts.Services.Implementations;
using ElectronicParts.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ElectronicParts.DI
{
    public static class Container
    {
        private static IServiceProvider provider;

        static Container()
        {
            IServiceCollection services = new ServiceCollection();

            /// Configuration
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            /// ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<PreferencesViewModel>();

            // Services
            services.AddSingleton<IAssemblyService, AssemblyService>();
            services.AddSingleton<IPinConnectorService, PinConnectorService>();
            services.AddSingleton<IExecutionService, ExecutionService>();
            services.AddSingleton<INodeSerializerService, NodeSerializerService>();
            services.AddSingleton<IAssemblyBinder, AssemblyBinder>();

            provider = services.BuildServiceProvider();
        }


        public static TItem Resolve<TItem>()
        {
            return provider.GetService<TItem>();
        }
    }
}
