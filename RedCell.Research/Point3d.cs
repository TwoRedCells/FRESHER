namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Struct Point3d
    /// </summary>
    public struct Point3d
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Point3d(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X { get; private set; }

        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y { get; private set; }

        /// <summary>
        /// Gets the z.
        /// </summary>
        /// <value>The z.</value>
        public double Z { get; private set; }
    }
}
