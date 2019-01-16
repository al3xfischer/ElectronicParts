using ElectronicParts.DI;
using ElectronicParts.ViewModels;
<<<<<<< HEAD
using System;
using System.Windows;
=======
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
>>>>>>> NodeTemplate

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Canvas canvas;

        private bool ShowCross;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.ViewModel = Container.Resolve<MainViewModel>();
        }

        public MainViewModel ViewModel { get; }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            var preferences = new Preferences();
            preferences.ShowDialog();
<<<<<<< HEAD
=======
        }

        private void Node_Moving(object sender, System.EventArgs e)
        {
        }

        private void Node_Stopped(object sender, EventArgs.StoppedEventArgs e)
        {
            MessageBox.Show(e.Position.Y.ToString());
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this.canvas);
            this.ViewModel.SelectedNode.Left = (int)point.X;
            this.ViewModel.SelectedNode.Top = (int)point.Y;
        }

        private void ItemsCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.canvas = sender as Canvas;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !(this.ViewModel.SelectedNode is null))
            {
                var point = e.GetPosition(this.canvas);

                if (point.X <= 0 || point.Y <= 0 ||point.X >= this.canvas.ActualWidth || point.Y >= this.canvas.ActualHeight)
                {
                    return;
                }

                this.ViewModel.SelectedNode.Left = (int)point.X - 40;
                this.ViewModel.SelectedNode.Top = (int)point.Y - 20;
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

>>>>>>> NodeTemplate
        }
    }
}
