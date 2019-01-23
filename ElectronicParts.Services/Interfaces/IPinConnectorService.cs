// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IPinConnectorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IPinConnectorService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Implementations
{
    using ElectronicParts.Models;
    using Shared;

    /// <summary>
    /// A interface used to implement classes which allow connect two pins with each other.
    /// </summary>
    public interface IPinConnectorService
    {
        /// <summary>
        /// Tries to connect two pins with each other.
        /// </summary>
        /// <param name="inputPin">The input pin.</param>
        /// <param name="outputPin">The output pin.</param>
        /// <param name="newConnection">The created connection.</param>
        /// <param name="noConnectionInsertion">A value indicating whether the connection should be added to a collection or not.</param>
        /// <returns>True if connecting was successful, false otherwise.</returns>
        bool TryConnectPins(IPin inputPin, IPin outputPin, out Connector newConnection, bool noConnectionInsertion);

        /// <summary>
        /// Tries to remove the connection between two pins.
        /// </summary>
        /// <param name="connectorToDelete">The connection which will be deleted.</param>
        /// <returns>True if the removing the connection was successful, false otherwise.</returns>
        bool TryRemoveConnection(Connector connectorToDelete);

        /// <summary>
        /// Checks if the tow given pins could be connected to each other.
        /// </summary>
        /// <param name="inputPin">The input pin.</param>
        /// <param name="outputPin">The output pin.</param>
        /// <returns>A value indicating whether the pins could be connected.</returns>
        bool IsConnectable(IPin inputPin, IPin outputPin);

        /// <summary>
        /// Adds a connection to the service.
        /// </summary>
        /// <param name="connectionToAdd">The connection which will be added.</param>
        void ManuallyAddConnectionToExistingConnections(Connector connectionToAdd);
    }
}