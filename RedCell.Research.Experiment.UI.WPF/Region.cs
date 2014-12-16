using System.Windows;
using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    public class Region : Grid, IRegion
    {
        public Region(float x, float y, float w, float h)
        {
            Margin = new Thickness(x, y, 0, 0);
            Width = w;
            Height = h;
        }

        public double X
        {
            get { return Margin.Left; }
            set { Margin = new Thickness(value, Margin.Top, 0, 0); }
        }

        public double Y
        {
            get { return Margin.Left; }
            set { Margin = new Thickness(Margin.Left, value, 0, 0); }
        }

        public void Add(IControl content)
        {
            Children.Add(content as UIElement);
        }

        public void Remove(IControl content)
        {
            Children.Remove(content as UIElement);
        }
    }
}
