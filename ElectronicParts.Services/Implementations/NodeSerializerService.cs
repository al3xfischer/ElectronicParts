// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="NodeSerializerService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the NodeSerializerService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using Microsoft.Win32;

    /// <summary>
    /// Represents the <see cref="NodeSerializerService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.INodeSerializerService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.INodeSerializerService" />
    public class NodeSerializerService : INodeSerializerService
    {
        /// <summary>
        /// Represents the Assembly binder which is used to retrieve needed types from the currently loaded assemblies.
        /// </summary>
        private readonly AssemblyBinder assemblyBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeSerializerService"/> class.
        /// </summary>
        /// <param name="assemblyBinder">The assembly binder.</param>
        /// <exception cref="ArgumentNullException">Throws if the injected <see cref="AssemblyBinder"/> is null.</exception>
        public NodeSerializerService(AssemblyBinder assemblyBinder)
        {
            this.assemblyBinder = assemblyBinder ?? throw new ArgumentNullException(nameof(assemblyBinder));
        }

        /// <summary>
        /// Deserializes a file into a snap shot.
        /// </summary>
        /// <returns>The deserialized snap shot.</returns>
        public SnapShot Deserialize()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Binary file | *.bin"
            };

            openFileDialog.ShowDialog();

            SnapShot snapShot = null;

            if (openFileDialog.FileName != string.Empty)
            {
                using (FileStream fileStream = (FileStream)openFileDialog.OpenFile())
                {
                    BinaryFormatter formatter = new BinaryFormatter
                    {
                        Binder = this.assemblyBinder,
                        AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                    };

                    snapShot = formatter.Deserialize(fileStream) as SnapShot;
                }
            }

            return snapShot;
        }

        /// <summary>
        /// Serializes the given snapshot.
        /// </summary>
        /// <param name="snapShot">The snapshot which will be serialized.</param>
        public void Serialize(SnapShot snapShot)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Binary file | *.bin"
            };

            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != string.Empty)
            {
                using (FileStream fileStream = (FileStream)saveFileDialog.OpenFile())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, snapShot);
                }
            }
        }
    }
}
