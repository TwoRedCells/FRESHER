using System;
using System.IO;
using System.Xml.Linq;
using RedCell.Research.Experiment.Scripting;

namespace RedCell.Research.Experiment
{
    /// <summary>
    /// Class Experiment.
    /// </summary>
    public class Experiment
    {
        #region Constants
        /// <summary>
        /// The current schema version
        /// </summary>
        public const int CurrentSchemaVersion = 1;

        /// <summary>
        /// The experiment filename
        /// </summary>
        public const string ExperimentFilename = "experiment.xml";
        
        /// <summary>
        /// The experiment element name
        /// </summary>
        public const string ExperimentElementName = "Experiment";
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Experiment(string name = null)
        {
            Name = name;
            SchemaVersion = CurrentSchemaVersion;
            Researcher = "";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the engine.
        /// </summary>
        /// <value>The engine.</value>
        public ScriptingEngines Engine { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the script path.
        /// </summary>
        /// <value>The path.</value>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the researcher.
        /// </summary>
        /// <value>The researcher.</value>
        public string Researcher { get; set; }

        /// <summary>
        /// Gets or sets the schema version.
        /// </summary>
        /// <value>The schema version.</value>
        public int SchemaVersion { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Experiment.</returns>
        public static Experiment Load(string name)
        {
            var path = System.IO.Path.Combine(name, ExperimentFilename);
            var xml = XDocument.Load(path);
            if (xml.Root.Name != ExperimentElementName)
                throw new ResearchException("This is not a valid experiment.");

            if (int.Parse(xml.Root.Attribute("schemaVersion").Value) != CurrentSchemaVersion)
                UpgradeSchema(xml);

            var experiment = new Experiment
            {
                Name = xml.Root.Attribute("name").Value,
                Researcher = xml.Root.Attribute("researcher").Value,
                Script = xml.Root.Attribute("script").Value,
                SchemaVersion = int.Parse(xml.Root.Attribute("schemaVersion").Value),
                Engine = (ScriptingEngines) Enum.Parse(typeof(ScriptingEngines), xml.Root.Attribute("engine").Value)
            };
            return experiment;
        }

        /// <summary>
        /// Upgrades the schema.
        /// </summary>
        /// <param name="xml">The XML.</param>
        private static void UpgradeSchema(XDocument xml)
        {
            int version = int.Parse(xml.Root.Attribute("schemaVersion").Value);

            if(version == 1)
            {
                xml.Root.Add(new XAttribute("engine", "Python"));
            }
        }

        /// <summary>
        /// Saves the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public void Save(string directory = null)
        {
            var parentDirectory = new DirectoryInfo(directory ?? Directory.GetCurrentDirectory());

            // Create Experiment directory
            if (parentDirectory.GetDirectories(Name).Length == 0)
            {
                parentDirectory.CreateSubdirectory(Name);
            }

            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(ExperimentElementName,
                    new XAttribute("name", Name),
                    new XAttribute("researcher", Researcher),
                    new XAttribute("schemaVersion", SchemaVersion),
                    new XAttribute("script", Script),
                    new XAttribute("engine", Engine))
                );
            var path = System.IO.Path.Combine(parentDirectory.FullName, Name, ExperimentFilename);
            xml.Save(path);
        }
        #endregion
    }
}
