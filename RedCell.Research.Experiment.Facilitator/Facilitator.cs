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
//            Camera.Initialize();
//            Camera.Start();
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
        public static ICamera Camera { get; set; }

        /// <summary>
        /// Loads the experiment.
        /// </summary>
        public static void RunExperiment(string path, ScriptingEngines engine = ScriptingEngines.Unknown)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            if(!File.Exists(path))
                throw new FileNotFoundException();

            if (engine == ScriptingEngines.Unknown)
            {
                engine = GuessEngine(path);
                if(engine == ScriptingEngines.Unknown)
                    throw new ArgumentException("The scripting engine could not be determined. Pass it as an argument or add an extension to the path.", "path");
            }

            IScript script = null;
            switch (engine)
            {
                case ScriptingEngines.Python:
                    script = new PythonScript();
                    break;

                case ScriptingEngines.CSharp:
                    script = new CSharpScript();
                    break;

                default:
                    throw new NotImplementedException("The scripting language " + engine + "has not yet been implemented.");
            }

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
