using ElectronicParts.DI;
using ElectronicParts.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Canvas canvas;

        private NodeViewModel currentNode;

        public MainWindow()
        {
            this.DataContext = this;
            this.ViewModel = Container.Resolve<MainViewModel>();
        }

        public MainViewModel ViewModel { get; }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            var preferences = new Preferences();
            preferences.ShowDialog();
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.currentNode = (e.OriginalSource as FrameworkElement).DataContext as NodeViewModel;
            //var point = e.GetPosition(this.canvas);
            //this.ViewModel.SelectedNode.Left = (int)point.X;
            //this.ViewModel.SelectedNode.Top = (int)point.Y;
        }

        private void ItemsCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.canvas = sender as Canvas;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //var vm = (e.OriginalSource as FrameworkElement).DataContext as NodeViewModel;
            if (e.LeftButton == MouseButtonState.Pressed && !(currentNode is null))
            {
                var point = e.GetPosition(this.canvas);

                if (this.currentNode is null || point.X <= 0 || point.Y <= 0 || point.X >= this.canvas.ActualWidth || point.Y >= this.canvas.ActualHeight)
                {
                    return;
                }

                this.currentNode.Left = (int)point.X - 40;
                this.currentNode.Top = (int)point.Y - 20;
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
    }
}
