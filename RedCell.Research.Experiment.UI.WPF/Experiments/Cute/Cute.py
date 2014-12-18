from RedCell.Research.Experiment import CameraViews
from RedCell.Research.Experiment import Log
import RedCell.Research.Experiment.UI
from RedCell.Research.Experiment.UI import *
from RedCell.UI.WPF.Charts import *

print "Cute.py started."

#UI.MessageBox("Hello")

# This is the main window.
mainRegion = UI.AddRegion(0, 0, 1920, 1080)

##################################################################################################
# Configure camera
cameraOnline = Camera.Initialize();
width = Camera.DefaultWidth
height = Camera.DefaultHeight

if (cameraOnline):

    Camera.EnableFace = True
    Camera.EnableExpression = True
    Camera.EnableEmotion = False
    Camera.EnableStreaming = True

    ## Create display regions for our camera views.
    camRegion1 = UI.AddRegion(0, 0, 320, 240)
    #camRegion2 = UI.AddRegion(640, 0, width, height)
    #camRegion3 = UI.AddRegion(1280, 0, width, height)

    ## Create camera views.
    camView1 = CameraView(Camera, CameraViews.Colour)
    #camView2 = CameraView(Camera, CameraViews.Depth)
    #camView3 = CameraView(Camera, CameraViews.Infrared)

    ## Add our camera views into their regions.
    camRegion1.Add(camView1)
    #camRegion2.Add(camView2)
    #camRegion3.Add(camView3)


##UI.AddCameraFaceAnnotation(viewColour)



Camera.Start();

##################################################################################################
# Introduce the experiment.

thanks = TextView("Thank you for participating in this experiment.")
mainRegion.Add(thanks)

UI.Wait(5)

mainRegion.Clear()
mainRegion.Add(TextView("You will be shown 20 photographs.\r\nReact as you would normally."))

UI.Wait(5)


##################################################################################################
# Start logging
expressions = [ "EXPRESSION_SMILE", "EXPRESSION_MOUTH_OPEN", "EXPRESSION_BROW_RAISER_LEFT", "EXPRESSION_KISS" ]

log = Log();
log.Monitor(Camera, expressions)

stripRegion = UI.AddRegion(320,0,1600,240)
strip = StripChartView(log)
stripRegion.Add(strip)
strip.Start()


##################################################################################################
# Show the photos.
images = [ 
    "bunny.jpg", "chick.jpg", "dog.jpg", "ducky.jpg", "elephant.jpg", 
    "fox.jpg", "hamster.jpg", "husky.jpg", "kitten.jpg", "lemur.jpg",
    "lizard.jpg", "lynx.jpg", "meercat.jpg", "monkey.jpg", "owl.jpg",
    "penguin.jpg", "piglet.jpg", "polar-bear.jpg", "seal.jpg", "turtle.jpg"
]

for a in range(1,1000):
	for image in images:
		imageView = ImageView('Cute\\images\\' + image)
		mainRegion.Clear()
		mainRegion.Add(imageView)
		UI.Wait(5)

