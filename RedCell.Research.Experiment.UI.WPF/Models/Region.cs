using System.Windows;
using System.Windows.Controls;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Class Region.
    /// </summary>
    public class Region : Grid, IRegion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        public Region(float x, float y, float w, float h)
        {
            Margin = new Thickness(Settings.ScaleX * x, Settings.ScaleY * y, 0, 0);
            Width = w * Settings.ScaleX;
            Height = h * Settings.ScaleY;
        }

        /// <summary>
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X
        {
            get { return Margin.Left; }
            set { Margin = new Thickness(value * Settings.ScaleX, Margin.Top, 0, 0); }
        }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y
        {
            get { return Margin.Left; }
            set { Margin = new Thickness(Margin.Left, value * Settings.ScaleY, 0, 0); }
        }

        /// <summary>
        /// Adds the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        public void Add(IControl content)
        {
            UISleepHelper.Do(() => Children.Add(content as UIElement));
        }

        /// <summary>
        /// Removes the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        public void Remove(IControl content)
        {
            Children.Remove(content as UIElement);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            UISleepHelper.Do(() => Children.Clear());
        }
    }
}
