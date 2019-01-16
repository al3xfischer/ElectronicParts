using ElectronicParts.Models;
using Shared;

namespace ElectronicParts.Services.Implementations
{
    public interface IPinConnectorService
    {
        bool TryConnectPins(IPin inputPin, IPin outputPin, out Connector newConnection);
        bool TryRemoveConnection(Connector connectorToDelete);
    }
}