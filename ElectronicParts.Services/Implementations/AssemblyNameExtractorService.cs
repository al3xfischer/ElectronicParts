// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="AssemblyNameExtractorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the AssemblyNameExtractorService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Text.RegularExpressions;
    using ElectronicParts.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the <see cref="AssemblyNameExtractorService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.IAssemblyNameExtractorService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.IAssemblyNameExtractorService" />
    public class AssemblyNameExtractorService : IAssemblyNameExtractorService
    {
        /// <summary>
        /// Represents the logger instance.
        /// </summary>
        private readonly ILogger<AssemblyNameExtractorService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNameExtractorService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <exception cref="ArgumentNullException">Throws if the injected logger instance is null.</exception>
        public AssemblyNameExtractorService(ILogger<AssemblyNameExtractorService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Extracts the assembly name out of an exception message.
        /// </summary>
        /// <param name="exception">The thrown exception.</param>
        /// <returns>The name of the assembly.</returns>
        public string ExtractAssemblyNameFromErrorMessage(Exception exception)
        {
            string result = "Unknown";

            if (exception == null)
            {
                return result;
            }

            Regex pattern = new Regex("\"[A-Za-z0-9_.]*,");
            var match = pattern.Match(exception.Message);
            if (match.Success)
            {
                try
                {
                    result = match.Value.AsSpan(1, match.Value.Length - 2).ToString();
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, $"Unexpected error in {this}");
                }
            }

            return result;
        }
    }
}
