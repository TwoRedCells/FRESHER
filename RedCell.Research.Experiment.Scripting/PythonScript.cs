﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace RedCell.Research.Experiment.Scripting
{
    /// <summary>
    /// Class PythonScript.
    /// </summary>
    public class PythonScript : IScript
    {
        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>IScript.</returns>
        public void Load(string path)
        {
            var engine = Python.CreateEngine();
            engine.Runtime.IO.RedirectToConsole();
            Source = engine.CreateScriptSourceFromFile(path);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            var scope = Source.Engine.CreateScope();

            var facilitator = Assembly.Load("RedCell.Research.Experiment.Facilitator");
            Source.Engine.Runtime.LoadAssembly(facilitator);
            var fac = facilitator.GetType("RedCell.Research.Experiment.Facilitator");
            var ui = fac.GetProperty("UI", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public).GetValue(null, null);
            scope.SetVariable("UI", ui);

            var camera = fac.GetProperty("Camera", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public).GetValue(null, null);
            scope.SetVariable("Camera", camera);

            Source.Engine.Runtime.LoadAssembly(Assembly.Load("RedCell.Research.Experiment"));
            Source.Engine.Runtime.LoadAssembly(Assembly.Load("RedCell.Research.Experiment.UI.WPF"));
            Source.Engine.Runtime.LoadAssembly(Assembly.Load("RedCell.Research.Experiment.Logging"));
            Source.Engine.Runtime.LoadAssembly(Assembly.Load("RedCell.UI.WPF.Charts"));

            Source.Execute(scope);
        }

        /// <summary>
        /// Compiles the script.
        /// </summary>
        /// <remarks>If the engine does not require compilation, implement this as an empty method.</remarks>
        public void Compile()
        {
            try
            {
                Source.Compile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Script compilation error: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public ScriptSource Source { get; set; }
    }
}
