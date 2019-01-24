// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="MainWindow.xaml.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the MainWindow class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Views
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using ElectronicParts.DI;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.ViewModels;
    using Microsoft.Extensions.Logging;
    using Microsoft.Win32;
    using MahApps.Metro.Controls;
    using Point = System.Windows.Point;


    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// The service used for the execution of nodes.
        /// </summary>
        private readonly IExecutionService executionService;

        /// <summary>
        /// The logger of the <see cref="MainWindow"/> class.
        /// </summary>
        private readonly ILogger<MainWindow> logger;

        /// <summary>
        /// The anchor point of the <see cref="selectionRectangle"/>.
        /// </summary>
        private System.Windows.Point anchorPoint;

        /// <summary>
        /// The canvas of the window.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The currently selected node.
        /// </summary>
        private NodeViewModel currentNode;

        /// <summary>
        /// A value indicating whether the rectangle is being dragged or not.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// A list of objects.
        /// </summary>
        private List<object> items;

        /// <summary>
        /// The rectangle for selections.
        /// </summary>
        private FrameworkElement selectionRectangle;

        /// <summary>
        /// The start point of the mouse drag.
        /// </summary>
        private Point? startPoint;

        /// <summary>
        /// The selection rectangle.
        /// </summary>
        private Rectangle rectangle;

        /// <summary>
        /// The moved the selection rectangle.
        /// </summary>
        private bool movedSelection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.DataContext = this;
            this.ViewModel = Container.Resolve<MainViewModel>();
            this.ViewModel.AddAssembly = () => this.AddAssembly_Click(this, new RoutedEventArgs());
            this.logger = Container.Resolve<ILogger<MainWindow>>();
            this.executionService = Container.Resolve<IExecutionService>();
            this.items = new List<object>();
            this.ViewModel.GetMousePosition = () => Mouse.GetPosition(this.canvas);
        }

        /// <summary>
        /// Gets the view model of the main window.
        /// </summary>
        /// <value>The view model of the main window.</value>
        public MainViewModel ViewModel { get; }

        /// <summary>
        /// Gets the information of a pin.
        /// </summary>
        /// <param name="node">The node of the pin.</param>
        /// <returns>A tuple with type <see cref="Type"/>, <see cref="int"/>, <see cref="NodeViewModel"/>.</returns>
        public Tuple<Type, int, NodeViewModel> GetPinInformation(NodeViewModel node)
        {
            var addPins = new AddPins();

            if (addPins.ShowDialog() == true)
            {
                return Tuple.Create(addPins.SelectedType, addPins.Amount, node);
            }

            return null;
        }

        /// <summary>
        /// Selects the items.
        /// </summary>
        public void SelectedItems()
        {
            System.Windows.Point currentMousePosition = Mouse.GetPosition(this.canvas);
            var left = Math.Min(currentMousePosition.X, this.anchorPoint.X);
            var top = Math.Min(currentMousePosition.Y, this.anchorPoint.Y);
            var rect = new Rect(new Point(left, top), new System.Windows.Size(this.selectionRectangle.Width, this.selectionRectangle.Height));
            var geometry = new RectangleGeometry(rect);
            VisualTreeHelper.HitTest(this.canvas, new HitTestFilterCallback(this.HitTestFilter), new HitTestResultCallback(this.HitTestTesultHandler), new GeometryHitTestParameters(geometry));
        }

        /// <summary>
        /// This method is called when the Add Assembly menu option is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void AddAssembly_Click(object sender, RoutedEventArgs e)
        {
            var assemblyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies");
            var fileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                Multiselect = true,
                Filter = "Node Assemblies |*.dll"
            };
            var result = fileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var files = fileDialog.FileNames.Select(path => new FileInfo(path));
                foreach (var file in files)
                {
                    try
                    {
                        file.CopyTo(System.IO.Path.Combine(assemblyPath, file.Name), true);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, $"Error while copying a file to assemblies folder ({nameof(this.AddAssembly_Click)})");
                        Debug.WriteLine("file exception");
                    }
                }
            }

            var reloadTask = this.ViewModel.ReloadAssemblies();
        }

        /// <summary>
        /// This method is called when the Add Input Pins menu option is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void AddInputPins_Click(object sender, RoutedEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement)?.DataContext is NodeViewModel node)
            {
                var pinsInformation = this.GetPinInformation(node);

                if (pinsInformation is null)
                {
                    return;
                }

                this.ViewModel.AddInputPinsCommand.Execute(pinsInformation);
            }
        }

        /// <summary>
        /// This method is called when the Add Output Pins menu option is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void AddOuputPins_Click(object sender, RoutedEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement)?.DataContext is NodeViewModel node)
            {
                var pinsInformation = this.GetPinInformation(node);

                if (pinsInformation is null)
                {
                    return;
                }

                this.ViewModel.AddOutputPinsCommand.Execute(pinsInformation);
            }
        }

        /// <summary>
        /// This method is called when the scrolls bar of the canvas is changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BoardScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.ViewModel.VerticalScrollerOffset = (int)this.boardScroller.ContentVerticalOffset;
            this.ViewModel.HorizontalScrollerOffset = (int)this.boardScroller.ContentHorizontalOffset;
        }

        /// <summary>
        /// This method is called when the right mouse button is clicked above the dock panel.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void DockPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.ResetPreviewLine();
        }

        /// <summary>
        /// Drags the selection rectangle.
        /// </summary>
        private void DragSelection()
        {
            var currentPoint = Mouse.GetPosition(this.canvas);
            this.selectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(currentPoint.X, this.anchorPoint.X));
            this.selectionRectangle.SetValue(Canvas.TopProperty, Math.Min(currentPoint.Y, this.anchorPoint.Y));
            this.selectionRectangle.Width = Math.Abs(currentPoint.X - this.anchorPoint.X);
            this.selectionRectangle.Height = Math.Abs(currentPoint.Y - this.anchorPoint.Y);
            if (this.selectionRectangle.Visibility == Visibility.Collapsed)
            {
                this.selectionRectangle.Visibility = Visibility.Visible;
            }

            this.SelectedItems();
        }

        /// <summary>
        /// Filters the hit test.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns><see cref="HitTestFilterBehavior"/>.</returns>
        private HitTestFilterBehavior HitTestFilter(DependencyObject dependencyObject)
        {
            var element = dependencyObject as FrameworkElement;
            if (element?.DataContext is NodeViewModel || element?.DataContext is ConnectorViewModel)
            {
                return HitTestFilterBehavior.Continue;
            }
            else
            {
                return HitTestFilterBehavior.ContinueSkipSelf;
            }
        }

        /// <summary>
        /// Handles the hit test.
        /// </summary>
        /// <param name="result">The result of the hit test.</param>
        /// <returns><see cref="HitTestResultBehavior.Continue"/>.</returns>
        private HitTestResultBehavior HitTestTesultHandler(HitTestResult result)
        {
            var dataContext = (result.VisualHit as FrameworkElement).DataContext;
            var nodeViewModel = dataContext as NodeViewModel;
            var connectorViewModel = dataContext as ConnectorViewModel;

            if (!(nodeViewModel is null) && !this.ViewModel.SelectedNodes.Contains(nodeViewModel))
            {
                this.ViewModel.SelectedNodes.Add(nodeViewModel);
            }
            else if (!(connectorViewModel is null) && !this.ViewModel.SelectedConntectors.Contains(connectorViewModel))
            {
                this.ViewModel.SelectedConntectors.Add(connectorViewModel);
            }

            return HitTestResultBehavior.Continue;
        }

        /// <summary>
        /// This method is called when the canvas is loaded.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void ItemsCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.canvas = sender as Canvas;
        }

        /// <summary>
        /// This method is called when the mouse leaves the canvas area.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void ItemsCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
        }

        /// <summary>
        /// This method is called when the left mouse button is clicked above the canvas.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void ItemsCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement).DataContext is NodeViewModel)
            {
                return;
            }

            this.startPoint = null;
            this.ViewModel.SelectedNodes.Clear();
            this.ViewModel.SelectedConntectors.Clear();

            if (this.selectionRectangle.Visibility == Visibility.Collapsed && e.LeftButton == MouseButtonState.Pressed)
            {
                this.isDragging = true;
                this.anchorPoint = Mouse.GetPosition(this.canvas);
            }
            else
            {
                this.ResetSelection();
            }
        }

        /// <summary>
        /// This method is called when the left mouse button is released above the canvas.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void ItemsCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
        }

        /// <summary>
        /// This method is called when the left mouse button is clicked above a list view item.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
            {
                var vm = item.DataContext as NodeViewModel;
                if (!(vm is null) && this.ViewModel.AddNodeCommand.CanExecute(null))
                {
                    this.ViewModel.AddNodeCommand.Execute(vm.Node);
                }
            }
        }

        /// <summary>
        /// This method is called when the main window is loaded.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void MainW_Loaded(object sender, RoutedEventArgs e)
        {
            this.selectionRectangle = this.FindUid("selectRect") as FrameworkElement;
        }

        /// <summary>
        /// This method is called when the left mouse button is clicked above a node.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.executionService.IsEnabled)
            {
                this.currentNode = (e.OriginalSource as FrameworkElement).DataContext as NodeViewModel;
            }
        }

        /// <summary>
        /// This method is called when the left mouse button is released above a node.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void Node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.currentNode = null;
        }

        /// <summary>
        /// This method is called when the Open Assembly Folder menu option is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void OpenAssemblyFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error while opening folder {System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies")}");
                Debug.WriteLine("Folder opening error");
            }
        }

        /// <summary>
        /// This method is called when the Preferences menu option is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            var preferences = new Preferences();
            preferences.ShowDialog();
            this.ViewModel.UpdateBoardSize.Execute(null);
        }

        /// <summary>
        /// Resets the selection.
        /// </summary>
        private void ResetSelection()
        {
            this.isDragging = false;
            this.selectionRectangle.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sets the start position of the preview line.
        /// </summary>
        /// <param name="pin">The pin at which the preview line starts.</param>
        /// <param name="mousePoint">The point at which the preview line ends.</param>
        private void SetPreviewLineStartPosition(PinViewModel pin, Point mousePoint)
        {
            PreviewLineViewModel previewLine = this.ViewModel.PreviewLines[0];

            previewLine.PointOneX = pin.Left;
            previewLine.PointOneY = pin.Top;

            previewLine.PointTwoX = mousePoint.X;
            previewLine.PointTwoY = mousePoint.Y;

            previewLine.Visible = true;
        }

        /// <summary>
        /// This method is called when the mouse is moved.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                this.DragSelection();
            }

            if (e.LeftButton == MouseButtonState.Pressed && !this.isDragging && this.currentNode is null && this.startPoint.HasValue)
            {
                var currentPosition = e.GetPosition(this.canvas);

                foreach (var nodeVm in this.ViewModel.SelectedNodes)
                {
                    nodeVm.Left -= this.startPoint.Value.X - currentPosition.X;
                    nodeVm.Top -= this.startPoint.Value.Y - currentPosition.Y;
                }

                var left = Canvas.GetLeft(this.rectangle);
                var top = Canvas.GetTop(this.rectangle);
                left -= this.startPoint.Value.X - currentPosition.X;
                top -= this.startPoint.Value.Y - currentPosition.Y;
                Canvas.SetLeft(this.rectangle, left);
                Canvas.SetTop(this.rectangle, top);
                this.startPoint = currentPosition;
                this.movedSelection = true;
            }

            if (e.LeftButton == MouseButtonState.Pressed && !(this.currentNode is null) && !this.isDragging)
            {
                var point = e.GetPosition(this.canvas);

                if (this.currentNode is null || point.X <= 0 || point.Y <= 0 || point.X + this.currentNode.Width >= this.canvas.ActualWidth || point.Y + ((this.currentNode.MaxPins - 1) * 20) >= this.canvas.ActualHeight)
                {
                    return;
                }

                this.currentNode.Left = (int)point.X - 40;
                this.currentNode.Top = (int)point.Y - 20;
                if (this.ViewModel.GridSnappingEnabled)
                {
                    this.currentNode.SnapToNewGrid(this.ViewModel.GridSize, false);
                }
            }

            var mousePoint = e.GetPosition(this.canvas);

            if (this.ViewModel.InputPin is null && this.ViewModel.OutputPin is null)
            {
                PreviewLineViewModel previewLine = this.ViewModel.PreviewLines[0];
                previewLine.Visible = false;
            }

            if (!(this.ViewModel.InputPin is null))
            {
                this.SetPreviewLineStartPosition(this.ViewModel.InputPin, mousePoint);
            }

            if (!(this.ViewModel.OutputPin is null))
            {
                this.SetPreviewLineStartPosition(this.ViewModel.OutputPin, mousePoint);
            }

            var mousePosition = e.GetPosition(this.boardScroller);
            if (mousePosition.X > 0 && mousePosition.X < this.boardScroller.ActualWidth)
            {
                if (mousePosition.Y <= 20 && mousePosition.Y > 0)
                {
                    boardScroller.ScrollToVerticalOffset(boardScroller.ContentVerticalOffset - 0.1);
                }
                else if (mousePosition.Y > this.boardScroller.ActualHeight - 40 && mousePosition.Y < this.boardScroller.ActualHeight)
                {
                    boardScroller.ScrollToVerticalOffset(boardScroller.ContentVerticalOffset + 0.1);
                }
            }

            if (mousePosition.Y > 0 && mousePosition.Y < this.ActualHeight)
            {
                if (mousePosition.X <= 20 && mousePosition.X > 0)
                {
                    boardScroller.ScrollToHorizontalOffset(boardScroller.ContentHorizontalOffset - 0.1);
                }
                else if (mousePosition.X > this.boardScroller.ActualWidth - 40 && mousePosition.X < this.boardScroller.ActualWidth)
                {
                    boardScroller.ScrollToHorizontalOffset(boardScroller.ContentHorizontalOffset + 0.1);
                }
            }
        }

        /// <summary>
        /// Opens a new 'About' window.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the Rectangle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.startPoint = Mouse.GetPosition(this.canvas);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the Loaded event of the Rectangle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Rectangle_Loaded(object sender, RoutedEventArgs e)
        {
            this.rectangle = (Rectangle)sender;
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the Rectangle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.startPoint.HasValue && !this.movedSelection)
            {
                this.ResetSelection();
            }
            else
            {
                this.movedSelection = false;
            }
        }
    }
}
