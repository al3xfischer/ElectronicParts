// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="IConfigurationService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IConfigurationService interface of the ElectronicParts programm</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Interfaces
{
    using ElectronicParts.Models;

    /// <summary>
    /// Used for implementations of configuration services.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the configurations which include all configurations needed in other classes of the program.
        /// </summary>
        /// <value>The Configuration which contains all needed configurations.</value>
        Configuration Configuration { get; }

        /// <summary>
        /// Saves the configurations to a file.
        /// </summary>
        void SaveConfiguration();
    }
}
