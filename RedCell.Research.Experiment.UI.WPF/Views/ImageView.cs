using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RedCell.Research.Experiment.UI
{
    /// <summary>
    /// Class ImageView.
    /// </summary>
    public class ImageView : Image, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageView"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ImageView(string path)
        {
            this.Stretch = Stretch.Uniform;
            this.Source = new BitmapImage(new Uri(new FileInfo(path).FullName));
        }
    }
}
 