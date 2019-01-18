using System;

namespace ElectronicParts.Services.Interfaces
{
    public interface IAssemblyNameExtractorService
    {
        string ExtractAssemblyNameFromErrorMessage(Exception exception);
    }
}