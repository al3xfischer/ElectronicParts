// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="NodeType.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the NodeType enumeration.</summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    ///     The available types of nodes.
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        ///     A logical node.
        /// </summary>
        Logic,

        /// <summary>
        ///     A display node.
        /// </summary>
        Display,

        /// <summary>
        ///     A source node.
        /// </summary>
        Source,

        /// <summary>
        ///     A node which can be switch on/off.
        /// </summary>
        Switch
    }
}