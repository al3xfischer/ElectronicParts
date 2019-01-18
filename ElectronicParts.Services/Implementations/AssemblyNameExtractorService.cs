using ElectronicParts.Services.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace ElectronicParts.Services.Implementations
{
    public class AssemblyNameExtractorService : IAssemblyNameExtractorService
    {
        public string ExtractAssemblyNameFromErrorMessage(Exception exception)
        {
            string result = "Unknown";

            if (exception == null)
            {
                return result;
            }
            var test = exception.Message;
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
                    // TODO Exception handeling
                }
            }
            return result;
        }
    }
}
