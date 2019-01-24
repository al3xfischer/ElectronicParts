// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="PinConnectorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the PinConnectorService class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using Shared;

    /// <summary>
    /// Represents the <see cref="PinConnectorService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Implementations.IPinConnectorService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Implementations.IPinConnectorService" />
    public class PinConnectorService : IPinConnectorService
    {
        /// <summary>
        /// Represents the Existing connections.
        /// </summary>
        private readonly List<Connector> existingConnections;

        /// <summary>
        /// Represents the Type comparer service.
        /// </summary>
        private readonly IGenericTypeComparerService typeComparerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinConnectorService"/> class.
        /// </summary>
        /// <param name="typeComparerService">The type comparer service.</param>
        /// <exception cref="ArgumentNullException">Is thrown if the injected <see cref="IGenericTypeComparerService"/> instance is null.</exception>
        public PinConnectorService(IGenericTypeComparerService typeComparerService)
        {
            this.existingConnections = new List<Connector>();
            this.typeComparerService = typeComparerService ?? throw new ArgumentNullException(nameof(typeComparerService));
        }

        /// <summary>
        /// Determines whether the specified output is connectable.
        /// </summary>
        /// <param name="input">The input pin.</param>
        /// <param name="output">The output pin.</param>
        /// <returns>True if the specified output is connectable and otherwise, False.</returns>
        public bool IsConnectable(IPin input, IPin output)
        {
            return this.typeComparerService.IsSameGenericType(input, output) && !this.existingConnections.Any(connection => connection.InputPin == input);
        }

        /// <summary>
        /// Determines whether the specified pin is involved in a connection.
        /// </summary>
        /// <param name="pin">The pin to be checked.</param>
        /// <returns>True if the specified pin has a connection and otherwise, False.</returns>
        public bool HasConnection(IPin pin)
        {
            return this.existingConnections.Any(conn => conn.InputPin == pin || conn.OutputPin == pin);
        }

        /// <summary>
        /// Manually adds a connection to the service.
        /// </summary>
        /// <param name="connectionToAdd">The connection which will be added.</param>
        public void ManuallyAddConnectionToExistingConnections(Connector connectionToAdd)
        {
            if (!this.existingConnections.Contains(connectionToAdd))
            {
                this.existingConnections.Add(connectionToAdd);
            }
        }

        /// <summary>
        /// Tries to connect two pins.
        /// </summary>
        /// <param name="inputPin">The input pin.</param>
        /// <param name="outputPin">The output pin.</param>
        /// <param name="newConnection">The new connection.</param>
        /// <param name="noConnectionInsertion">A value indicating whether the connection should be added to a collection or not.</param>
        /// <returns>true if connecting was successful, false otherwise.</returns>
        public bool TryConnectPins(IPin inputPin, IPin outputPin, out Connector newConnection, bool noConnectionInsertion)
        {
            newConnection = null;

            // returning false if one pin is null or pins are not connectable.
            if (inputPin is null || outputPin is null || !this.IsConnectable(outputPin, inputPin))
            {
                return false;
            }

            // null checking value instances of the pins 
            // if this returns true then we check for the generic type of the pin and set the Value to a new valueInstance.
            if (inputPin.Value is null)
            {
                if (!this.TryRefreshPinValue(inputPin))
                {
                    return false;
                }
            }

            if (outputPin.Value is null)
            {
                if (!this.TryRefreshPinValue(outputPin))
                {
                    return false;
                }
            }

            // trying to insert the outputpin value of the first pin into the inputpin of the second pin.
            try
            {
                inputPin.Value = outputPin.Value;
                newConnection = new Connector(inputPin, outputPin, outputPin.Value);
                if (!noConnectionInsertion)
                {
                    this.existingConnections.Add(newConnection);
                }

                return true;
            }
            catch (InvalidCastException)
            {
                // If the types of pins are not compatible an InvalidCastException gets thrown by the pin instance.
                return false;
            }
        }

        /// <summary>
        /// Tries to remove an existing connection and sets the Value properties of both pins to null.
        /// This way no further communication will happen.
        /// </summary>
        /// <param name="connectorToDelete">Converts to delete.</param>
        /// <returns>true if deletion was successful, false otherwise.</returns>
        public bool TryRemoveConnection(Connector connectorToDelete)
        {
            // In this case the connector is faulty and we have nothing to delete
            if (connectorToDelete is null || !this.existingConnections.Contains(connectorToDelete))
            {
                return false;
            }

            // Setting the inputPin of the connection to a new IValue instance.
            if (!this.TryRefreshPinValue(connectorToDelete.InputPin))
            {
                return false;
            }

            this.existingConnections.Remove(connectorToDelete);
            return true;
        }

        /// <summary>
        /// Tries to reset the value of the pin.
        /// </summary>
        /// <param name="pin">Represents the pin.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        private bool TryRefreshPinValue(IPin pin)
        {
            var pinType = pin.GetType();
            var argumentList = pinType.GetGenericArguments();
            if (argumentList.Length > 1)
            {
                return false;
            }

            var genericPinType = argumentList[0];
            var nonGenericValueType = typeof(Value<>);
            var genericValueType = nonGenericValueType.MakeGenericType(genericPinType);
            var instance = Activator.CreateInstance(genericValueType);
            pin.Value = (IValue)instance;

            return true;
        }
    }
}
