using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RedCell.Research.Experiment.UI
{
    public class TextView : Viewbox, IControl
    {
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
