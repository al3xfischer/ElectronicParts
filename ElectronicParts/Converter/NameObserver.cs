using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace ElectronicParts.Converter
{
    public static class NameObserver
    {
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(NameObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));

        public static readonly DependencyProperty ObservedNameProperty = DependencyProperty.RegisterAttached(
            "ObservedName",
            typeof(string),
            typeof(NameObserver));

        public static bool GetObserve(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }

        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        public static double GetObservedName(FrameworkElement frameworkElement)
        {
            return (double)frameworkElement.GetValue(ObservedNameProperty);
        }

        public static void SetObservedName(FrameworkElement frameworkElement, double observedWidth)
        {
            frameworkElement.SetValue(ObservedNameProperty, observedWidth);
        }

        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                ((ColorPicker)frameworkElement).SelectedColorChanged += NameObserverSelectedColorChanged; ;
                UpdateObservedSizesForFrameworkElement(frameworkElement);
            }
            else
            {
                ((ColorPicker)frameworkElement).SelectedColorChanged -= NameObserverSelectedColorChanged;
            }
        }

        private static void NameObserverSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
        }

        private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
        {
            frameworkElement.SetCurrentValue(ObservedNameProperty, ((ColorPicker)frameworkElement).SelectedColorText);
        }
    }
}
