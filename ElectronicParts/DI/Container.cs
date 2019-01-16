<<<<<<< HEAD
﻿using ElectronicParts.Services;
using ElectronicParts.Services.Interfaces;
﻿using ElectronicParts.Services.Implementations;
using ElectronicParts.ViewModels;
=======
﻿using ElectronicParts.ViewModels;
>>>>>>> NodeTemplate
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

<<<<<<< HEAD
            /// Configuration
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            /// ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<PreferencesViewModel>();

            // Services
            services.AddSingleton<IAssemblyService, AssemblyService>();
            services.AddSingleton<IPinConnectorService, PinConnectorService>();
=======
            /// ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<PreferencesViewModel>();
>>>>>>> NodeTemplate

            provider = services.BuildServiceProvider();
        }


        public static TItem Resolve<TItem>()
        {
            return provider.GetService<TItem>();
        }
    }
}
