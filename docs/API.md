FRESHER API
===========

User Interface
--------------
___________________________________________________________________

`void MessageBox (string message)`  
Shows a simple modal message box. Useful for debugging.

___________________________________________________________________

`ICameraView AddCameraView (float x, float y, float w, float h, ICamera camera, CameraViews view)`  
Adds a live camera view to the user interface

Argument      | Description
--------------|:----------------------
x             | The **x** position
y             | The **y** position
w             | The **width**
h             | THe **height**
camera        | A reference to the **camera** for this view
view          | Which of the camera's **views** to display.

___________________________________________________________________

`void AddCameraFaceAnnotation (ICameraView view)`  
Adds an annotation to a `CameraView` that indicates the location of a face in the image.

Argument      | Description
--------------|:----------------------
view          | The **`CameraView`** to which the annotation should be added.

___________________________________________________________________

`void Wait (double seconds)`  
Tells the user interface to hang out.

Argument      | Description
--------------|:----------------------
seconds       | The number of **seconds** to do nothing.

							  