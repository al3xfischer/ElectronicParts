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
using System.Windows.Shapes;

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> logger;

        private Canvas canvas;

        private NodeViewModel currentNode;

        public MainWindow()
        {
            this.DataContext = this;
            this.ViewModel = Container.Resolve<MainViewModel>();
            this.ViewModel.AddAssembly = () => this.AddAssembly_Click(this, new RoutedEventArgs());
            this.logger = Container.Resolve<ILogger<MainWindow>>();
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
            this.currentNode = (e.OriginalSource as FrameworkElement).DataContext as NodeViewModel;
        }

        private void ItemsCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.canvas = sender as Canvas;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
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
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            var vm = e.AddedItems[0] as NodeViewModel;
            if (vm is null)
            {
                return;
            }

            this.ViewModel.AddNodeCommand.Execute(vm.Node);
            (sender as ListView).SelectedItems.Clear();
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

        private void DockPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ViewModel.PreviewLines[0].Visible = false;
            this.ViewModel.OutputPin = null;
            this.ViewModel.InputPin = null;
        }
    }
}
