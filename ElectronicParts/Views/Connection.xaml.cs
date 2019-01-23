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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for Connection.
    /// </summary>
    public partial class Connection : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        public Connection()
        {
            this.InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ////DrawingGroup drawingGroup = VisualTreeHelper.GetDrawing(sender as Visual);

            ////HitTestResult hitTestResult = VisualTreeHelper.HitTest(sender as Visual, e.GetPosition(this));
            ////var path = hitTestResult.VisualHit as Path;
            ////if (path == null)
            ////    return;

            ////// 2. Iterate through geometries of the Path and hit test each one
            //////    to find a line to delete

            ////var geometryGroup = path.Data;
            ////if (geometryGroup == null)
            ////    return;

            ////var geometries = geometryGroup as PathGeometry;
            ////PathGeometry.Parse()
            ////var segments = geometries.Figures[0];
            ////var first = segments.Segments[0];
            ////VisualTreeHelper.GetDrawing(first);
            ////Point point = e.GetPosition(path);
            ////var pen = new Pen(path.Stroke, path.StrokeThickness);

            //// 3. Delete link
        }
    }
}
