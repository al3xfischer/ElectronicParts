namespace ElectronicParts.EventArgs
{
    using System;
    using System.Windows;

    public class StoppedEventArgs : EventArgs
    {

        public StoppedEventArgs(Point point)
        {
            Position = point;
        }

        public Point Position { get; }
    }
}