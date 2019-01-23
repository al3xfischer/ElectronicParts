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
    using System;
    using System.Windows.Input;
    using System.Windows.Media;
    using ElectronicParts.Models;

    /// <summary>
    /// A view model for the <see cref="Rule{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of connection that the rule applies to.</typeparam>
    public class RuleViewModel<T> : BaseViewModel
    {
        /// <summary>
        /// Contains the color of the rule.
        /// </summary>
        private Color color;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleViewModel{T}"/> class.
        /// </summary>
        /// <param name="rule">The <see cref="Rule{T}"/> encapsulated by this view model.</param>
        /// <param name="deletionCommand">The <see cref="ICommand"/> to delete the rule.</param>
        public RuleViewModel(Rule<T> rule, ICommand deletionCommand)
        {
            this.Rule = rule ?? throw new ArgumentNullException(nameof(rule));
            this.DeletionCommand = deletionCommand ?? throw new ArgumentNullException(nameof(deletionCommand));
            this.Color = (Color)ColorConverter.ConvertFromString(rule.Color);
        }

        /// <summary>
        /// Gets or sets the value of the rule.
        /// </summary>
        /// <value>The value of the rule.</value>
        public T Value
        {
            get
            {
                return this.Rule.Value;
            }

            set
            {
                this.Rule.Value = value;
                this.FirePropertyChanged(nameof(this.Value));
            }
        }

        /// <summary>
        /// Gets or sets the color of the connection when the rule is active.
        /// </summary>
        /// <value>The color of the connection when the rule is active.</value>
        public Color Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                this.FirePropertyChanged(nameof(this.Color));
            }
        }

        /// <summary>
        /// Gets the command which is used for deleting the rule.
        /// </summary>
        /// <value>The command which is used for deleting the rule.</value>
        public ICommand DeletionCommand { get; }

        /// <summary>
        /// Gets the rule which can be changed.
        /// </summary>
        /// <value>The rule which can be changed.</value>
        public Rule<T> Rule { get; }
    }
}
