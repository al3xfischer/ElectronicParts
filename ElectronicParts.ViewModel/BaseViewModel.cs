// ***********************************************************************
// Author           : Alexander Fischer
// ***********************************************************************
// <copyright file="BaseViewModel.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the BaseViewModel class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the <see cref="BaseViewModel"/> class.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event for <see cref="INotifyPropertyChanged"/> event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the <see cref="INotifyPropertyChanged"/> event with the calling member name as the name of the property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Will be set to name of calling member if not specified.</param>
        protected virtual void FirePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of an item and invokes the <see cref="INotifyPropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="item">The item to be set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">The name of the property that changed. Will be set to name of calling member if not specified.</param>
        protected void Set<TValue>(ref TValue item, TValue value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<TValue>.Default.Equals(item, value))
            {
                item = value;
                this.FirePropertyChanged(propertyName);
            }
        }
    }
}
