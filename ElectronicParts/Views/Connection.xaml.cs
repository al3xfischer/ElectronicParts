// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="Connection.xaml.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the partial Connection class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Views
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using ElectronicParts.ViewModels;

    /// <summary>
    /// Interaction logic for Connection.
    /// </summary>
    public partial class Connection : UserControl
    {
        /// <summary>
        /// The connector according view model.
        /// </summary>
        private ConnectorViewModel connectorViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        public Connection()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            string text = this.connectorViewModel?.CurrentValue?.Current?.ToString() ?? string.Empty;
            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            FormattedText formattedText = new FormattedText(
                text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                10,
                Brushes.Black,
                dpi);

            var targetPoint = new Point();
            var leftPoint = this.connectorViewModel is null ? new Point(10, 10) : this.connectorViewModel.CenterBottomPoint;
            var rightPoint = this.connectorViewModel is null ? new Point(10, 10) : this.connectorViewModel.SelfConnectionInputPoint;
            targetPoint.X = leftPoint.X > rightPoint.X - 20 ? leftPoint.X - rightPoint.X - 20 : rightPoint.X - 20 - leftPoint.X;
            targetPoint.Y = leftPoint.Y > rightPoint.Y ? leftPoint.Y - rightPoint.Y : rightPoint.Y - leftPoint.Y;
            targetPoint.X *= 0.5;
            targetPoint.Y *= 0.5;
            targetPoint.Y -= 15;
            targetPoint.X += Math.Min(leftPoint.X, rightPoint.X);
            targetPoint.Y += Math.Min(leftPoint.Y, rightPoint.Y);

            drawingContext.DrawText(formattedText, targetPoint);
            base.OnRender(drawingContext);
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.connectorViewModel = (e.Source as FrameworkElement).DataContext as ConnectorViewModel;
            this.connectorViewModel.PropertyChanged += (vm, args) => Application.Current.Dispatcher.InvokeAsync(() => this.InvalidateVisual());
            this.InvalidateVisual();
        }
    }
}
