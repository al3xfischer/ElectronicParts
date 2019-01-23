// ***********************************************************************
// Author           : Peter Helf, Roman Jahn
// ***********************************************************************
// <copyright file="PreviewLineViewModel.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the PreviewLineViewModel class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    /// <summary>
    /// Represents the <see cref="PreviewLineViewModel"/> class.
    /// </summary>
    public class PreviewLineViewModel : BaseViewModel
    {
        /// <summary>
        /// Contains the x value of the first point.
        /// </summary>
        private double pointOneX;

        /// <summary>
        /// Contains the y value of the first point.
        /// </summary>
        private double pointOneY;

        /// <summary>
        /// Contains the x value of the second point.
        /// </summary>
        private double pointTwoX;

        /// <summary>
        /// Contains the y value of the second point.
        /// </summary>
        private double pointTwoY;

        /// <summary>
        /// Contains the value indicating whether the line is visible or not.
        /// </summary>
        private bool visible;

        /// <summary>
        /// Gets or sets a value indicating whether the line is visible or not.
        /// </summary>
        /// <value>The value indicating whether the line is visible or not.</value>
        public bool Visible
        {
            get
            {
                return this.visible;
            }

            set
            {
                this.visible = value;
                this.FirePropertyChanged(nameof(this.Visible));
            }
        }

        /// <summary>
        /// Gets or sets the x value of the first point.
        /// </summary>
        /// <value>The x value of the first point.</value>
        public double PointOneX
        {
            get
            {
                return this.pointOneX;
            }

            set
            {
                this.pointOneX = value;
                this.FirePropertyChanged(nameof(this.PointOneX));
            }
        }

        /// <summary>
        /// Gets or sets the y value of the first point.
        /// </summary>
        /// <value>The y value of the first point.</value>
        public double PointOneY
        {
            get
            {
                return this.pointOneY;
            }

            set
            {
                this.pointOneY = value;
                this.FirePropertyChanged(nameof(this.PointOneY));
            }
        }

        /// <summary>
        /// Gets or sets the x value of the second point.
        /// </summary>
        /// <value>The x value of the second point.</value>
        public double PointTwoX
        {
            get
            {
                return this.pointTwoX;
            }

            set
            {
                this.pointTwoX = value;
                this.FirePropertyChanged(nameof(this.PointTwoX));
            }
        }

        /// <summary>
        /// Gets or sets the y value of the second point.
        /// </summary>
        /// <value>The y value of the second point.</value>
        public double PointTwoY
        {
            get
            {
                return this.pointTwoY;
            }

            set
            {
                this.pointTwoY = value;
                this.FirePropertyChanged(nameof(this.PointTwoY));
            }
        }
    }
}
