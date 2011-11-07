WPFShapeGame - READ ME 

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the terms of the Microsoft Kinect for
Windows SDK Beta License Agreement:
http://kinectforwindows.org/KinectSDK-ToU


=============================
OVERVIEW  
.............................
This module provides sample code used to demonstrate how to create a simple game
 that uses Kinect audio and skeletal tracking information.
The game displays the tracked skeleton of the players and shapes (circles,
triangles, stars, and so on) falling from the sky. Players can move their limbs to
make shapes change direction or even explode, and speak commands such as
"make bigger"/"make smaller" to increase/decrease the size of the shapes or
"show yellow stars" to change the color and type of falling shapes. See below for
the full vocabulary.


=============================
SAMPLE LANGUAGE IMPLEMENTATIONS     
.............................
This sample is available in C#.


=============================
FILES   
.............................
- App.xaml: Declaration of application level resources.
- App.xaml.cs: Interaction logic behind app.xaml.
- FallingShapes.cs: Shape rendering, physics, and hit-testing logic.
- MainWindow.xaml: Declaration of layout within main application window.
- MainWindow.xaml.cs: NUI initialization, player skeleton tracking, and main game logic.
- Recognizer.cs: Speech verb definition and speech event recognition.


=============================
PREREQUISITES   
.............................
- Speech Platform Runtime (v10.2) x86. Even on x64 platforms, the x86 must be used because the Microsoft Kinect SDK Beta runtime is x86.
http://www.microsoft.com/downloads/en/details.aspx?FamilyID=bb0f72cb-b86b-46d1-bf06-665895a313c7
- Speech Platform SDK (v10.2) 
http://www.microsoft.com/downloads/en/details.aspx?FamilyID=1b1604d3-4f66-4241-9a21-90a294a5c9a4&displaylang=en
- Kinect English Language Pack: MSKinectLangPack_enUS.msi (available in the same location as the Kinect for Windows SDK Beta).


=============================
BUILDING THE SAMPLE   
.............................

To build the sample by using Visual Studio:
-----------------------------------------------------------
1. In Windows Explorer, navigate to the ShapeGame\CS directory.
2. Double-click the icon for the .sln (solution) file to open the file in Visual Studio.
3. On the Build menu, select Build Solution. The application will be built in the default \Debug or \Release directory.


=============================
RUNNING THE SAMPLE   
.............................

To run the sample:
------------------
1. Navigate to the directory that contains the new executable by using the command prompt or Windows Explorer.
2. Type ShapeGame at the command line, or double-click the icon for ShapeGame.exe to start it from Windows Explorer.


=============================
SPEECH VOCABULARY
.............................

To do this:                            Say any of these:
------------------------------------   -----------------
Reset everything to initial settings   Reset, Clear
Pause the falling shapes               Stop, Freeze, Pause Game
Resume game                            Go, Unfreeze, Resume, Continue, 
                                       Play
Fall faster                            Speed Up, Faster
Fall more slowly                       Slow Down, Slower
Drop more shapes                       More, More Shapes
Drop fewer shapes                      Less, Fewer
Show bigger shapes                     Larger, Bigger, Bigger Shapes
Show largest shapes                    Huge, Giant, Super Big, Biggest
Show smaller shapes                    Smaller
Show smallest shapes                   Smallest, Tiny, Super Small

Any combination of color and/or shape may be spoken as well:

Colors: Red, Orange, Yellow, Green, Blue, Purple, Violet, Pink,
        Brown, Gray, Black, White, Bright, Dark, Every Color, 
        All Colors, Random Colors

Shapes: Triangles, Squares, Stars, Pentagons, Hexagons, Circles,
        7 Pointed Stars, All Shapes, Everything, Bubbles, Shapes

Note: Most phrases can be preceded by any words, for example: 

"Now do green circles" will drop green circles.
"Please go faster" and "even faster" will indeed speed things up.
