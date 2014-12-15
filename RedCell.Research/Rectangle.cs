namespace RedCell.Research.Experiment
{
    public struct Rectangle : IRectangle
    {
        public Rectangle(double x, double y, double w, double h) : this()
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }
    }
}
