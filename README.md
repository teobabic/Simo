# Simo

Simo, a novel approach, that transforms an off-the-shelf smartphone into a user motion tracking device and controller. Both the front and back cameras of the smartphone are used simultaneously to track the user’s hand, head, eye-gaze and body movements in real-world space and scale.

Simo is an ARKit iOS application made in Unity.


## Features

* **Device/hand motion + touch inputs:** Users can interact by performing 3D hand movements in 6DOF (translation + rotation) and can reliably segment and further enhance their gestures by touchscreen inputs.

* **Head pose tracking:** 6DOF head tracking. Example: This can be used for head-pointing.

* **Eye-gaze tracking:** 6DOF eye-gaze tracking. Example: This can be used for eye-pointing.

* **Body pose tracking:** 6DOF tracking of the torso (position + orientation). Example: This can be used for body-position or ego-centric interactions.

* **No specialized hardware required:** No external hardware, external trackers, markers or cameras are required. Everything relies only on a single iPhone.


The Simo app tracks all following user motions simultaneously, in real-time and in world-scale:

<img src="https://media.giphy.com/media/lbY1MQDxbmHtrz8bSQ/giphy.gif" width="400">

*Tracking areas* of the front and back iPhone camera.

<img src="https://media.giphy.com/media/KrNPKNWYHXQZeFBz2P/giphy.gif" width="400">

*Device/Hand tracking*.

<img src="https://media.giphy.com/media/misvwdpqAYXIZLGeJn/giphy.gif" width="400">

*Head pose tracking*.

<img src="https://media.giphy.com/media/plHJJpkoHLZ3AISmxU/giphy.gif" width="400">

*Eye-Gaze tracking*.

<img src="https://media.giphy.com/media/b6niaXupgi7VUK98Jl/giphy.gif" width="400">

*Body tracking*.

## Quickstart guide

* Open the *Simo* project in Unity.
* Select iOS as the target platform under File > Build Settings > Platform >  iOS > click *Switch Platform*.
* Build the iOS app under File > Build Settings > Build > Create folder > Choose a name > click *Save*.
* In the created folder select *Unity-iPhone.xcodeproj* and open it in Xcode.
* Connect your iPhone to your Mac and select it as the build *Device*.
* In Xcode project settings under *Signing & Capabilities* select your Apple Developer Signing *Team* ID, *Bundle Identifier*, and press *Play*.
* Open the Simo app on your iPhone.
* Place an AR Anchor on the floor of your room and use the *Change camera* and *Change view* buttons to toggle between different tracking views.

## Compatibility
The project was made and tested in Unity 2020.2.0f1, Xcode 12.3, iPhone XS and iOS 14.2

System requirements:
* iPhone with ARKit and FaceID capabilities
* Unity
* Xcode
* MacOS

## More information
This work is based on a publication [**"Simo: Interactions with Distant Displays by Smartphones with Simultaneous Face and World Tracking"**](https://doi.org/10.1145/3334480.3382962 "Simo publication") in the CHI EA '20: Extended Abstracts of the 2020 CHI Conference on Human Factors in Computing Systems. The publication also includes related work, user studies, applications, and future work directions.

------

Copyright (C) 2020 Teo Babic
