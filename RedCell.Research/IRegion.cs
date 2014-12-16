namespace RedCell.Research.Experiment
{
    public interface IRegion : IControl, IRectangle
    {
        void Add(IControl content);

        void Remove(IControl content);
    }
}
