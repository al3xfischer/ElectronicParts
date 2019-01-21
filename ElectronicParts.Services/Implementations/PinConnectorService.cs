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
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;

    public class PinConnectorService : IPinConnectorService
    {
        private readonly List<Connector> ExistingConnections;
        private readonly IGenericTypeComparerService typeComparerService;

        public PinConnectorService(IGenericTypeComparerService typeComparerService)
        {
            this.ExistingConnections = new List<Connector>();
            this.typeComparerService = typeComparerService ?? throw new ArgumentNullException(nameof(typeComparerService));
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
            if (inputPin is null || outputPin is null || !this.typeComparerService.IsSameGenericType(inputPin, outputPin))
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

            // Setting the inputPin of the connection to a new IValue instance.
            if (!this.TryRefreshPinValue(connectorToDelete.InputPin))
            {
                return false;
            }

            this.ExistingConnections.Remove(connectorToDelete);
            return true;
        }

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
