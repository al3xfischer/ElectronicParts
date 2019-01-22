using ElectronicParts.Models;
using Shared;

namespace ElectronicParts.Services.Implementations
{
    public interface IPinConnectorService
    {
        bool TryConnectPins(IPin inputPin, IPin outputPin, out Connector newConnection, bool noConnectionInsertion);
        bool TryRemoveConnection(Connector connectorToDelete);
        bool IsConnectable(IPin inputPin, IPin outputPin);
        void ManuallyAddConnectionToExistingConnections(Connector connectionToAdd);
    }
}