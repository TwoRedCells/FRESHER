namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Interface IScriptEngine
    /// </summary>
    public interface IScriptEngine
    {
        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>IScript.</returns>
        IScript Load(string path);
    }
}
