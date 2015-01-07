using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RedCell.Research.Experiment.UI
{
    public class ImageView : Image, IControl
    {
        public ImageView(string path)
        {
            this.Stretch = Stretch.Uniform;
            this.Source = new BitmapImage(new Uri(new FileInfo(path).FullName));
        }
    }
}
 