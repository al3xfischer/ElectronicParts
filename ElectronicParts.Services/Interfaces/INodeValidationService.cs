using Shared;

namespace ElectronicParts.Services.Interfaces
{
    public interface INodeValidationService
    {
        bool Validate(IDisplayableNode node);
    }
}