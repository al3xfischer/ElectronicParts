using System;

namespace ElectronicParts.Services.Interfaces
{
    public interface IGenericTypeComparerService
    {
        bool IsSameGenericType(object first, object second);
    }
}