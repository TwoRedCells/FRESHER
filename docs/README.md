FRESHER (
**FR**ramework for 
**E**motional 
**S**ensory 
**H**uman 
**E**xperimental 
**R**esearch
) 
==============================================================

Application Mechanics
---------------------
Aspects of the SDK used:
* Capture streaming
* Face detection
* Emotion analysis

The software is a kit for researchers to create and run experiments that analyze human reactions. 

With the kit a researcher can write a few lines of code in their favourite language 
(at this time Python and C# are supported) to build an experiment that is ready for 
test subjects to use.

The kit then runs these experiments and collects data. The data can be analyzed or 
even viewed in a strip chart.

Tools and Technology
--------------------
The application uses:
* RealSense camera and SDK
* .NET Framework 4.5.1
* Portable Class Libraries
* Windows x86/x64
* IronPython

Product Specifications
----------------------
The kit was designed to be very modular. Each facet of the design resides in its own DLL.

Since the research is writing their own scripts, the **Scripting** module compiles these and 
executes them at runtime.

The user interface is a separate module. The **WPF.UI** module implements this using WPF; 
however a module could be created to use Windows Forms, "Windows Store", Silverlight, iOS, etc. 
without changing any of the other modules.

The **Camera** module handles native communication with the RealSense camera, and other 
modules can query it to perform various tasks.

The **Logging** module serializes data for later retrieval.

There is a central controller called the **Facilitator** that ties everything together.

When the application is started:
* A script is referenced the script can call components in the Experiment API.
* The script is compiled.
* The script is executed and the tasks that the researcher specified are carried out. 
  For example, the script could tell the kit to show 100 images, and record the subject's 
  reaction to each.
* The results are logged.
* The researcher analyzes the results.

Special Requirements
--------------------
A video settings of 1920x1080 is recommended.