using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedCell.Research.Experiment.Tests
{
    [TestClass]
    public class Facilitator
    {
        [TestMethod]
        public void Initialize_NoExceptions()
        {
            Experiment.Facilitator.Initialize();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadExperiment_EmptyArgument()
        {
            Experiment.Facilitator.RunExperiment("  ");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void LoadExperiment_NoSuchFile()
        {
            Experiment.Facilitator.RunExperiment("foo.py");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadExperiment_ScriptingEngineIndeterminate()
        {
            Experiment.Facilitator.RunExperiment("test.jp");
        }

        [TestMethod]
        public void LoadExperiment_ValidPythonScript()
        {
            Experiment.Facilitator.RunExperiment("test.py");
        }

        [TestMethod]
        public void RunExperiment_ValidPythonScript()
        {
            Experiment.Facilitator.RunExperiment("test.py");
        }
    }
}
