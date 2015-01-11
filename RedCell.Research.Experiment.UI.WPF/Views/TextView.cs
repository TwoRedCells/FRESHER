using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Class TextView.
    /// </summary>
    public class TextView : Viewbox, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextView"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public TextView(string content)
        {
            Child = new Label
            {
                Content = content,
                FontSize = 64,
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
}
