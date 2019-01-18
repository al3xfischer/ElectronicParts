using ElectronicParts.Models;
using ElectronicParts.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Implementations
{
    public class NodeSerializerService : INodeSerializerService
    {
        private readonly IAssemblyBinder assemblyBinder;

        public NodeSerializerService(IAssemblyBinder assemblyBinder)
        {
            this.assemblyBinder = assemblyBinder ?? throw new ArgumentNullException(nameof(assemblyBinder));
        }

        public SnapShot Deserialize()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Binary file | *.bin";
            openFileDialog.ShowDialog();

            SnapShot snapShot = null;

            if (openFileDialog.FileName != "")
            {
                using (FileStream fileStream = (FileStream)openFileDialog.OpenFile())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Binder = new AssemblyBinder();

                    formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

                    snapShot = formatter.Deserialize(fileStream) as SnapShot;
                }
            }

            return snapShot;
        }

        public void Serialize(SnapShot snapShot)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary file | *.bin";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                using (FileStream fileStream = (FileStream)saveFileDialog.OpenFile())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;

                    formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

                    formatter.Serialize(fileStream, snapShot);
                }                
            }
        }
    }
}
