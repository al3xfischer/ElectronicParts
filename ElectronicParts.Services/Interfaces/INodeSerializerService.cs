// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="INodeSerializerService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the INodeSerializerService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using ElectronicParts.Models;

    /// <summary>
    /// A interface used to implement classes which allow to serialize <see cref="SnapShot"/> instances.
    /// </summary>
    public interface INodeSerializerService
    {
        /// <summary>
        /// Serializes the given snapshot.
        /// </summary>
        /// <param name="snapShot">The snapshot which will be serialized.</param>
        void Serialize(SnapShot snapShot);

        /// <summary>
        /// Deserializes a file into a snap shot.
        /// </summary>
        /// <returns>The deserialized snap shot.</returns>
        SnapShot Deserialize();
    }
}
