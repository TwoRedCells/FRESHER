using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace RedCell.Research.Experiment.Scripting
{
    /// <summary>
    /// Class CSharpScript.
    /// </summary>
    public class CSharpScript : IScript
    {
        private dynamic _instance;

        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>IScript.</returns>
        public void Load(string path)
        {
            Source = File.ReadAllText(path);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Compiles the script.
        /// </summary>
        /// <remarks>If the engine does not require compilation, implement this as an empty method.</remarks>
        public void Compile()
        {
            try
            {
                string code = @"
                    using System; 
                    namespace RedCell.Research.Experiment.Scripting {  
                    public class DynamicCSharp {  
                        private IExperimentUI UI { get; set; }
                        private Camera Camera { get; set; }
                        public void Run() { ````Source```` } 
                    } }";
                code = code.Replace("````Source````", Source);
                
                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters
                {
                    GenerateInMemory = true,
                    GenerateExecutable = true
                };
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, Source);

                if(results.Errors.HasErrors)
                {
                    var sb = new StringBuilder();
                    foreach (CompilerError error in results.Errors)
                        sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    throw new ResearchException(sb.ToString());
                }
                if (results.Errors.HasWarnings)
                {
                    foreach (CompilerError error in results.Errors)
                        Debug.WriteLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }
                var assembly = results.CompiledAssembly;
                Type @class = assembly.GetType("RedCell.Research.Experiment.Scripting.DynamicCSharp");
                _instance = Activator.CreateInstance(@class);

                assembly = Assembly.Load("RedCell.Research.Experiment.Facilitator");
                var fac = assembly.GetType("RedCell.Research.Experiment.Facilitator");
                _instance.UI = fac.GetProperty("UI", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public).GetValue(null, null);
                _instance.Camera = fac.GetProperty("Camera", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public).GetValue(null, null);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Script compilation error: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }
    }
}
