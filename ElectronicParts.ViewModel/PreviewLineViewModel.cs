using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicParts.ViewModels
{
    public class PreviewLineViewModel : BaseViewModel
    {
        private double pointTwoY;
        private double pointTwoX;
        private double pointOneY;
        private double pointOneX;
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                this.FirePropertyChanged(nameof(Visible));
            }
        }

        public double PointOneX
        {
            get { return pointOneX; }
            set
            {
                pointOneX = value;
                FirePropertyChanged(nameof(this.PointOneX));
            }
        }
        public double PointOneY
        {
            get { return pointOneY; }
            set
            {
                pointOneY = value;
                FirePropertyChanged(nameof(this.PointOneY));
            }
        }

        public double PointTwoX
        {
            get { return pointTwoX; }
            set
            {
                pointTwoX = value;
                FirePropertyChanged(nameof(this.PointTwoX));
            }
        }
        public double PointTwoY
        {
            get { return pointTwoY; }
            set
            {
                pointTwoY = value;
                FirePropertyChanged(nameof(this.PointTwoY));
            }
        }
    }
}
