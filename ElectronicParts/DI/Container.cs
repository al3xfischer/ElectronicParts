using ElectronicParts.Services;
using ElectronicParts.Services.Interfaces;
﻿using ElectronicParts.Services.Assemblies;
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

            /// ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<PreferencesViewModel>();

            // Services
            services.AddSingleton<IAssemblyService, AssemblyService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            provider = services.BuildServiceProvider();
        }


        public static TItem Resolve<TItem>()
        {
            return provider.GetService<TItem>();
        }
    }
}
