using ElectronicParts.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace ElectronicParts.Services.Implementations
{
    public class AssemblyNameExtractorService : IAssemblyNameExtractorService
    {
        private readonly ILogger<AssemblyNameExtractorService> logger;

        public AssemblyNameExtractorService(ILogger<AssemblyNameExtractorService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
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
                catch(Exception e)
                {
                    this.logger.LogError(e, $"Unexpected error in {this}");
                }
            }
            return result;
        }
    }
}
