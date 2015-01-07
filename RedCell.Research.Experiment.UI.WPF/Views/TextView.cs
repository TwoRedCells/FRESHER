using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RedCell.Research.Experiment.UI
{
    public class TextView : Label, IControl
    {
        public TextView(string content)
        {
            this.Content = content;
            this.FontSize = 64;
            this.Background = new SolidColorBrush(Colors.White);
            this.Foreground = new SolidColorBrush(Colors.Black);
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
