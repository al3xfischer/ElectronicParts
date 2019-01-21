using System;
using System.Runtime.Serialization;

namespace ElectronicParts.Services.Interfaces
{
    public interface IAssemblyBinder
    {
        Type BindToType(string fullAssemblyString, string typeName);
    }
}