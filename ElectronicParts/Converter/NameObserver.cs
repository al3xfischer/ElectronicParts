// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="NameObserver.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the NameObserver class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System.Windows;
    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// Observes a <see cref="ColorPicker"/> and notifies when the <see cref="ColorPicker.SelectedColorText"/> property changes.
    /// </summary>
    public static class NameObserver
    {
        /// <summary>
        /// The observed name of the selected color which can be bound to the view model.
        /// </summary>
        private static readonly DependencyProperty ObservedNameProperty = DependencyProperty.RegisterAttached(
            "ObservedName",
            typeof(string),
            typeof(NameObserver));

        /// <summary>
        /// Indicates whether the color picker is observed or not.
        /// </summary>
        private static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(NameObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));

        /// <summary>
        /// Gets a value indicating whether the color picker is observed or not.
        /// </summary>
        /// <param name="frameworkElement">The observed element.</param>
        /// <returns>A value indicating whether the color picker is observed or not.</returns>
        public static bool GetObserve(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }

        /// <summary>
        /// Gets the observed name of the selected color which can be bound to the view model.
        /// </summary>
        /// <param name="frameworkElement">The observed element.</param>
        /// <returns>The observed name of the selected color which can be bound to the view model.</returns>
        public static string GetObservedName(FrameworkElement frameworkElement)
        {
            return frameworkElement.GetValue(ObservedNameProperty).ToString();
        }

        /// <summary>
        /// Sets a value indicating whether the color picker is observed or not.
        /// </summary>
        /// <param name="frameworkElement">The observed element.</param>
        /// <param name="observe">A value indicating whether the color picker is observed or not.</param>
        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        /// <summary>
        /// Sets the observed name of the selected color which can be bound to the view model.
        /// </summary>
        /// <param name="frameworkElement">The observed element.</param>
        /// <param name="observedName">The observed name of the selected color which can be bound to the view model.</param>
        public static void SetObservedName(FrameworkElement frameworkElement, string observedName)
        {
            frameworkElement.SetValue(ObservedNameProperty, observedName);
        }

        /// <summary>
        /// This method gets called when the selected color of the color picker changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void NameObserverSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            UpdateObservedNameForFrameworkElement((FrameworkElement)sender);
        }

        /// <summary>
        /// This method gets called when the observed status gets changed.
        /// </summary>
        /// <param name="dependencyObject">The observed element.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                ((ColorPicker)frameworkElement).SelectedColorChanged += NameObserverSelectedColorChanged;
                UpdateObservedNameForFrameworkElement(frameworkElement);
            }
            else
            {
                ((ColorPicker)frameworkElement).SelectedColorChanged -= NameObserverSelectedColorChanged;
            }
        }

        /// <summary>
        /// Updates the observer <see cref="ObservedNameProperty"/>.
        /// </summary>
        /// <param name="frameworkElement">The observed element.</param>
        private static void UpdateObservedNameForFrameworkElement(FrameworkElement frameworkElement)
        {
            frameworkElement.SetCurrentValue(ObservedNameProperty, ((ColorPicker)frameworkElement).SelectedColorText);
        }
    }
}
