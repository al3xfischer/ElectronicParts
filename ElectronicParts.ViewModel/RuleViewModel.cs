// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="RuleViewModel.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the RuleViewModel class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    using System.Windows.Input;
    using System.Windows.Media;
    using ElectronicParts.Models;

    /// <summary>
    /// A viewmodel for the <see cref="Rule{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of connection that the rule applies to.</typeparam>
    public class RuleViewModel<T> : BaseViewModel
    {
        public RuleViewModel(Rule<T> rule, ICommand deletionCommand)
        {
            this.Rule = rule;
            this.DeletionCommand = deletionCommand;
            this.Color = (Color)ColorConverter.ConvertFromString(rule.Color);
        }

        /// <summary>
        /// Gets or sets the color of the connection when the rule is active.
        /// </summary>
        /// <value>The color of the connection when the rule is active.</value>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the command which is used for deleting the rule.
        /// </summary>
        public ICommand DeletionCommand { get; } 

        /// <summary>
        /// Gets the rule which can be changed.
        /// </summary>
        public Rule<T> Rule { get; }
    }
}
