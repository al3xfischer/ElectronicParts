using ElectronicParts.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Implementations
{
    public class AssemblyBinder : SerializationBinder, IAssemblyBinder
    {
        public override Type BindToType(string fullAssemblyString, string typeName)
        {
            Type wantedType = null;
            try
            {
                string assemblyName = fullAssemblyString.Split(',')[0];
                Assembly[] currentlyLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in currentlyLoadedAssemblies)
                {
                    if (assembly.FullName.Split(',')[0] == assemblyName)
                    {
                        wantedType = assembly.GetType(typeName);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                // TODO proper exception handeling
                Debug.WriteLine(e.Message);
            }
            return wantedType;
        }
    }
}
