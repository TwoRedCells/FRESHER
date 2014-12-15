using System;

namespace RedCell.Research.Sensors
{
    public class CameraException : Exception
    {
        public CameraException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {

        }
    }
}
