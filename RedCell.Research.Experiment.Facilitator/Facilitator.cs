using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RedCell.Research.Experiment.Scripting;
using RedCell.Research.Sensors;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class Facilitator.
    /// </summary>
    public static class Facilitator
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
                Camera = new RealSenseCamera();
            Log = new Log();
        }

        /// <summary>
        /// Gets or sets the UI.
        /// </summary>
        /// <value>The UI.</value>
        public static IExperimentUI UI { get; set; }

        /// <summary>
        /// Gets or sets the camera.
        /// </summary>
        /// <value>The camera.</value>
        public static ICamera Camera { get; private set; }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        public static Log Log { get; private set; }

        /// <summary>
        /// Loads the experiment.
        /// </summary>
        public static void RunExperiment(Experiment experiment)
        {
            IScript script = null;
            switch (experiment.Engine)
            {
                case ScriptingEngines.Python:
                    script = new PythonScript();
                    break;

                case ScriptingEngines.CSharp:
                    script = new CSharpScript();
                    break;

                default:
                    throw new NotImplementedException("The scripting language " + experiment.Engine + "has not yet been implemented.");
            }

            var path = Path.Combine(experiment.Name, experiment.Script);
            script.Load(path);
            script.Compile();
            script.Start();
        }

        /// <summary>
        /// Guesses the engine.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>ScriptingEngines.</returns>
        private static ScriptingEngines GuessEngine(string path)
        {
            Match m = Regex.Match(path, @"\.(?<Extension>.+)$");
            if(!m.Success)
                return ScriptingEngines.Unknown;

            string extension = m.Groups["Extension"].Value.ToLower();
            switch (extension)
            {
                case "py":
                case "python":
                    return ScriptingEngines.Python;
                case "js":
                case "javascript":
                    return ScriptingEngines.JavaScript;
                case "cs":
                case "csharp":
                    return ScriptingEngines.CSharp;
                default:
                    return ScriptingEngines.Unknown;
            }
        }

        /// <summary>
        /// Shows the image.
        /// </summary>
        public static void ShowImage()
        {
            Console.WriteLine("Showing picture.");
        }
    }
}
