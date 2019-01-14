using System;
using System.Drawing;

namespace Shared
{
    public interface IDisplayable
    {
        Bitmap Picture { get; }

        event EventHandler PictureChanged;
    }
}
