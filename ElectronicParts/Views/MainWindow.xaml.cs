using ElectronicParts.DI;
using ElectronicParts.ViewModels;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using ElectronicParts.Services.Interfaces;
using System.Windows.Media;
using System.Collections.Generic;

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> logger;

        private readonly IExecutionService executionService;

        private Canvas canvas;

        private NodeViewModel currentNode;

        private System.Windows.Point ancorPoint;

        private bool isDragging;

        private FrameworkElement selectionRectangle;

        private List<object> items;

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

        public MainViewModel ViewModel { get; }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            var preferences = new Preferences();
            preferences.ShowDialog();
            this.ViewModel.UpdateBoardSize.Execute(null);
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.executionService.IsEnabled)
            {
                this.currentNode = (e.OriginalSource as FrameworkElement).DataContext as NodeViewModel;
            }
        }

        private void ItemsCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.canvas = sender as Canvas;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.DragSelection();
            }

            if (e.LeftButton == MouseButtonState.Pressed && !(currentNode is null))
            {
                var point = e.GetPosition(this.canvas);

                if (this.currentNode is null || point.X <= 0 || point.Y <= 0 || point.X + this.currentNode.Width - 30 >= this.canvas.ActualWidth || point.Y + (this.currentNode.MaxPins - 1) * 20 >= this.canvas.ActualHeight)
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

            PreviewLineViewModel previewLine = this.ViewModel.PreviewLines[0];

            if ((ViewModel.InputPin is null) && (ViewModel.OutputPin is null))
            {
                previewLine.Visible = false;
            }

            if (!(ViewModel.InputPin is null))
            {
                previewLine.PointOneX = ViewModel.InputPin.Left;
                previewLine.PointOneY = ViewModel.InputPin.Top;

                previewLine.PointTwoX = mousePoint.X;
                previewLine.PointTwoY = mousePoint.Y;

                previewLine.Visible = true;
            }

            if (!(ViewModel.OutputPin is null))
            {
                previewLine.PointOneX = ViewModel.OutputPin.Left;
                previewLine.PointOneY = ViewModel.OutputPin.Top;

                previewLine.PointTwoX = mousePoint.X;
                previewLine.PointTwoY = mousePoint.Y;

                previewLine.Visible = true;
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

        private void DragSelection()
        {
            var currentPoint = Mouse.GetPosition(this.canvas);
            this.selectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(currentPoint.X, this.ancorPoint.X));
            this.selectionRectangle.SetValue(Canvas.TopProperty, Math.Min(currentPoint.Y, this.ancorPoint.Y));
            this.selectionRectangle.Width = Math.Abs(currentPoint.X - ancorPoint.X);
            this.selectionRectangle.Height = Math.Abs(currentPoint.Y - ancorPoint.Y);
            if (this.selectionRectangle.Visibility == Visibility.Collapsed)
            {
                this.selectionRectangle.Visibility = Visibility.Visible;
            }
            this.SelectedItems();
        }

        private void Node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.currentNode = null;
        }

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

        private void AddAssembly_Click(object sender, RoutedEventArgs e)
        {
            var assemblyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies");
            var fileDialog = new OpenFileDialog();
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.AddExtension = true;
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Node Assemblies |*.dll";
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

        private void BoardScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.ViewModel.VerticalScrollerOffset = (int)this.boardScroller.ContentVerticalOffset;
            this.ViewModel.HorizontalScrollerOffset = (int)this.boardScroller.ContentHorizontalOffset;
        }

        private void DockPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.ResetPreviewLine();
        }

        private void ItemsCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement).DataContext is NodeViewModel)
            {
                return;
            }

            if (this.selectionRectangle.Visibility == Visibility.Collapsed && e.LeftButton == MouseButtonState.Pressed)
            {
                this.isDragging = true;
                this.ancorPoint = Mouse.GetPosition(this.canvas);
            }
            else
            {
                this.ResetSelection();
            }
        }

        private void ItemsCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
        }

        private void ResetSelection()
        {
            this.isDragging = false;
            this.selectionRectangle.Visibility = Visibility.Collapsed;
        }

        private void MainW_Loaded(object sender, RoutedEventArgs e)
        {
            this.selectionRectangle = this.FindUid("selectRect") as FrameworkElement;
        }

        private void ItemsCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
        }

        public void SelectedItems()
        {
            System.Windows.Point currentMousePosition = Mouse.GetPosition(this.canvas);
            var rect = new Rect(this.ancorPoint, new Size(this.selectionRectangle.Width, this.selectionRectangle.Height));
            var geometry = new RectangleGeometry(rect);
            VisualTreeHelper.HitTest(this.canvas, new HitTestFilterCallback(this.HitTestFilter), new HitTestResultCallback(this.HitTestTesultHandler), new GeometryHitTestParameters(geometry));
        }

        private HitTestResultBehavior HitTestTesultHandler(HitTestResult result)
        {
            var dataContext = (result.VisualHit as FrameworkElement).DataContext;

            if (dataContext is NodeViewModel nodeViewModel && !this.ViewModel.SelectedNodes.Contains(nodeViewModel))
            {
                this.ViewModel.SelectedNodes.Add(nodeViewModel);
            }
            else if (dataContext is ConnectorViewModel connectorViewModel && !this.ViewModel.SelectedConntectors.Contains(connectorViewModel))
            {
                this.ViewModel.SelectedConntectors.Add(connectorViewModel);
            }

            return HitTestResultBehavior.Continue;
        }

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
    }
}
