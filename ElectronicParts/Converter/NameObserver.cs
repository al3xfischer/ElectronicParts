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

        public static string GetObservedName(FrameworkElement frameworkElement)
        {
            return frameworkElement.GetValue(ObservedNameProperty).ToString();
        }

        public static void SetObservedName(FrameworkElement frameworkElement, string observedName)
        {
            frameworkElement.SetValue(ObservedNameProperty, observedName);
        }

        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                ((ColorPicker)frameworkElement).SelectedColorChanged += NameObserverSelectedColorChanged; ;
                UpdateObservedNameForFrameworkElement(frameworkElement);
            }
            else
            {
                ((ColorPicker)frameworkElement).SelectedColorChanged -= NameObserverSelectedColorChanged;
            }
        }

        private static void NameObserverSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            UpdateObservedNameForFrameworkElement((FrameworkElement)sender);
        }

        private static void UpdateObservedNameForFrameworkElement(FrameworkElement frameworkElement)
        {
            frameworkElement.SetCurrentValue(ObservedNameProperty, ((ColorPicker)frameworkElement).SelectedColorText);
        }
    }
}
