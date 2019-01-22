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

                if (this.currentNode is null || point.X <= 0 || point.Y <= 0 || point.X + 20 >= this.canvas.ActualWidth || point.Y + (this.currentNode.MaxPins - 1) * 20 >= this.canvas.ActualHeight)
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

        private void Node_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.currentNode = null;
        }

        private void OpenAssemblyFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error while opening folder {Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies")}");
                Debug.WriteLine("Folder opening error");
            }
        }

        private void AddAssembly_Click(object sender, RoutedEventArgs e)
        {
            var assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assemblies");
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
                        file.CopyTo(Path.Combine(assemblyPath, file.Name), true);
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
            this.ViewModel.VerticalScrollerOffset = (int)boardScroller.ContentVerticalOffset;
        }
    }
}
