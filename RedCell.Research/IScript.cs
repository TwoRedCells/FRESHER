namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Interface IScript
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        void Load(string path);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Compiles the script.
        /// </summary>
        /// <remarks>
        /// If the engine does not require compilation, implement this as an empty method.
        /// </remarks>
        void Compile();
    }
}
