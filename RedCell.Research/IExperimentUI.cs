using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCell.Research.Experiment
{
    public interface IExperimentUI
    {
        /// <summary>
        /// Messages the box.
        /// </summary>
        /// <param name="message">The message.</param>
        void MessageBox(string message);

        /// <summary>
        /// Adds the camera view.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="camera">The camera.</param>
        /// <returns>ICameraView.</returns>
        ICameraView AddCameraView(float x, float y, float w, float h, ICamera camera, CameraViews view);
    }
}
