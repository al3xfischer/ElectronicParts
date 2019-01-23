// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IAssemblyNameExtractorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IAssemblyNameExtractorService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using System;

    /// <summary>
    /// A interface used to implement classes which allow the extraction of assembly names.
    /// </summary>
    public interface IAssemblyNameExtractorService
    {
        /// <summary>
        /// Extracts the assembly name out of a exception.
        /// </summary>
        /// <param name="exception">The thrown exception.</param>
        /// <returns>The name of the assembly.</returns>
        string ExtractAssemblyNameFromErrorMessage(Exception exception);
    }
}