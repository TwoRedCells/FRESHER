using System.Windows;
using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    public class Region : Grid, IRegion
    {
        public Region(float x, float y, float w, float h)
        {
            Margin = new Thickness(Settings.ScaleX * x, Settings.ScaleY * y, 0, 0);
            Width = w * Settings.ScaleX;
            Height = h * Settings.ScaleY;
        }

        public double X
        {
            get { return Margin.Left; }
            set { Margin = new Thickness(value * Settings.ScaleX, Margin.Top, 0, 0); }
        }

        public double Y
        {
            get { return Margin.Left; }
            set { Margin = new Thickness(Margin.Left, value * Settings.ScaleY, 0, 0); }
        }

        public void Add(IControl content)
        {
            UISleepHelper.Do(() => Children.Add(content as UIElement));
        }

        public void Remove(IControl content)
        {
            Children.Remove(content as UIElement);
        }

        public void Clear()
        {
            UISleepHelper.Do(() => Children.Clear());
        }
    }
}
