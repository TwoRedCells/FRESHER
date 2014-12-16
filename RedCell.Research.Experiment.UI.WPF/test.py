import RedCell.Research.Experiment
from RedCell.Research.Experiment import CameraViews
import RedCell.Research.Experiment.UI
from RedCell.Research.Experiment.UI import *

print "Hello, world!"

#UI.MessageBox("Hello")


#Camera.Initialize();
#width = Camera.DefaultWidth
#height = Camera.DefaultHeight

## Create display regions for our camera views.
#camRegion1 = UI.AddRegion(0, 0, width, height)
#camRegion2 = UI.AddRegion(640, 0, width, height)
#camRegion3 = UI.AddRegion(1280, 0, width, height)
mainRegion = UI.AddRegion(0, 480, 1920, 600)

## Create camera views.
#camView1 = CameraView(Camera, CameraViews.Colour)
#camView2 = CameraView(Camera, CameraViews.Depth)
#camView3 = CameraView(Camera, CameraViews.Infrared)

## Add our camera views into their regions.
#camRegion1.Add(camView1)
#camRegion2.Add(camView2)
#camRegion3.Add(camView3)


##UI.AddCameraFaceAnnotation(viewColour)


#Camera.Start();

hello = TextView("Hello, world!")
mainRegion.Add(hello)

UI.Wait(1);

mainRegion.Clear()
mainRegion.Add(TextView("Goodbye."));

UI.Wait(1);

mainRegion.Clear()
mainRegion.Add(TextView("so long."))

UI.Wait(1);

mainRegion.Clear()
mainRegion.Add(TextView("la"))

UI.Wait(1);

mainRegion.Clear()
mainRegion.Add(TextView("booger"))

