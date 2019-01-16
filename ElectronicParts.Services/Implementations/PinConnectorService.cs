﻿// ***********************************************************************
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
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ElectronicParts.Models;

    public class PinConnectorService : IPinConnectorService
    {
        private readonly List<Connector> ExistingConnections;

        public PinConnectorService()
        {
            this.ExistingConnections = new List<Connector>();
        }

        /// <summary>
        /// Tries to connect two pins.
        /// </summary>
        /// <param name="inputPin">The input pin.</param>
        /// <param name="outputPin">The output pin.</param>
        /// <param name="newConnection">The new connection.</param>
        /// <returns>true if connecting was successfull, false otherwise.</returns>
        public bool TryConnectPins(IPin inputPin, IPin outputPin, out Connector newConnection)
        {
            newConnection = null;

            // nullcheck returning false if one pin is null
            if (inputPin is null || outputPin is null)
            {
                return false;
            }

            // null checking value instances of the pins 
            // if this returns true then we check for the generic type of the pin and set the Value to a new valueInstance.
            if (inputPin.Value is null)
            {
                var pinType = inputPin.GetType();
                var argumentList = pinType.GetGenericArguments();
                if (argumentList.Length > 1)
                {
                    return false;
                }

                var genericPinType = argumentList[0];
                var nonGenericValueType = typeof(Value<>);
                var genericValueType = nonGenericValueType.MakeGenericType(genericPinType);
                var instance = Activator.CreateInstance(genericValueType);
                inputPin.Value = (IValue)instance;
            }

            if (outputPin.Value is null)
            {
                var pinType = outputPin.GetType();
                var argumentList = pinType.GetGenericArguments();
                if (argumentList.Length > 1)
                {
                    return false;
                }

                var genericPinType = argumentList[0];
                var nonGenericValueType = typeof(Value<>);
                var genericValueType = nonGenericValueType.MakeGenericType(genericPinType);
                var instance = Activator.CreateInstance(genericValueType);
                outputPin.Value = (IValue)instance;
            }

            // checking if the secondPin already has a connection
            // if second pin has connection returning false.
            foreach (var conn in this.ExistingConnections)
            {
                if (conn.InputPin == inputPin)
                {
                    return false;
                }
            }

            // trying to insert the outputpin value of the first pin into the inputpin of the second pin.
            try
            {
                inputPin.Value = outputPin.Value;
                newConnection = new Connector(inputPin, outputPin, outputPin.Value);
                this.ExistingConnections.Add(newConnection);
                return true;
            }
            // If the types of pins are not compatible an InvalidCastException gets thrown by the pin instance
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to remove an existing connection and sets the Value properties of both pins to null.
        /// This way no further communication will happen;
        /// </summary>
        /// <param name="connectorToDelete">Converts to delete.</param>
        /// <returns>true if deletion was successfull, false otherwise.</returns>
        public bool TryRemoveConnection(Connector connectorToDelete)
        {
            // In this case the connector is faulty and we have nothing to delete
            if (connectorToDelete is null || !this.ExistingConnections.Contains(connectorToDelete))
            {
                return false;
            }

            // Counting the connections for the output pin (one outputpin can supply multiple inputpins)
            // In case this is the only connection for this outputPin we set the pinvalue to null to prevent further influences between the two pins
            // In case the output pin has multiple connections we only remove the common value from the one inputpin that is to be deleted
            if (this.ExistingConnections.Count(conn => conn.OutputPin == connectorToDelete.OutputPin) == 1)
            {
                connectorToDelete.OutputPin.Value = null;
            }

            // In every case we will remove the connection from our known connections. And return true.
            connectorToDelete.InputPin.Value = null;
            this.ExistingConnections.Remove(connectorToDelete);
            return true;
        }
    }
}
