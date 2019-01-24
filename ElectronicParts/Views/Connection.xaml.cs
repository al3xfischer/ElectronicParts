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
    using ElectronicParts.ViewModels;
    using System.Globalization;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Connection.
    /// </summary>
    public partial class Connection : UserControl
    {
        private ConnectorViewModel connectorViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        public Connection()
        {
            this.InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            string testString = this.connectorViewModel?.CurrentValue?.Current?.ToString() ?? string.Empty;

            // Create the initial formatted text string.
            FormattedText formattedText = new FormattedText(
                testString,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                10,
                Brushes.Black)
            {

                // Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
                MaxTextWidth = 50,
                MaxTextHeight = 20
            };

            // Use a larger font size beginning at the first (zero-based) character and continuing for 5 characters.
            // The font size is calculated in terms of points -- not as device-independent pixels.
            //formattedText.SetFontSize(10, 0, 5);

            // Use a Bold font weight beginning at the 6th character and continuing for 11 characters.
            //formattedText.SetFontWeight(FontWeights.Black, 6, 11);

            // Use a linear gradient brush beginning at the 6th character and continuing for 11 characters.
            formattedText.SetForegroundBrush(new SolidColorBrush((Color)ColorConverter.ConvertFromString("black")));

            // Use an Italic font style beginning at the 28th character and continuing for 28 characters.
            //formattedText.SetFontStyle(FontStyles.Italic, 28, 28);
            var point = this.connectorViewModel is null ? new Point(10,10) : this.connectorViewModel.CenterTopPoint;
            point.X -= testString.Length * 6;
            point.Y -= 15;
            // Draw the formatted text string to the DrawingContext of the control.
            drawingContext.DrawText(formattedText, point);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.connectorViewModel = (e.Source as FrameworkElement).DataContext as ConnectorViewModel;
            connectorViewModel.PropertyChanged += (vm, args) => Application.Current.Dispatcher.InvokeAsync(() =>this.InvalidateVisual());
            this.InvalidateVisual();
        }
    }
}
