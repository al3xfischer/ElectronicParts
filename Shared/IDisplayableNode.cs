// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="IDisplayableNode.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IDisplayableNode interface.</summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    /// A interface for a node which implements the <see cref="INode"/> interface as well as the <see cref="IDisplayable"/> interface.
    /// </summary>
    public interface IDisplayableNode : INode, IDisplayable
    {
    }
}
