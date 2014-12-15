import RedCell.Research.Experiment
from RedCell.Research.Experiment import CameraViews

print "Hello, world!"

#UI.MessageBox("Hello")


Camera.Initialize();
width = Camera.DefaultWidth
height = Camera.DefaultHeight
viewColour = UI.AddCameraView(0, 0, width, height, Camera, CameraViews.Colour)
viewDepth = UI.AddCameraView(640, 0, width, height, Camera, CameraViews.Depth)
viewInfrared = UI.AddCameraView(1280, 0, width, height, Camera, CameraViews.Infrared)

UI.AddCameraFaceAnnotation(viewColour)


Camera.Start();

