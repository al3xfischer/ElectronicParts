using System;

namespace ElectronicParts.Services.Interfaces
{
    public interface IAssemblyBinder
    {
        Type BindToType(string fullAssemblyString, string typeName);
    }
}