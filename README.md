# Simo

Simo, a novel approach, that transforms an off-the-shelf smartphone into a user motion tracking device and controller. Both the front and back cameras of the smartphone are used simultaneously to track the user’s hand as well as the head, eye (gaze) and body movements in real-world space and scale.

Simo is an ARKit iOS application made in Unity.


## Features

* **Device (hand) motion + touch inputs:** Users can interact by performing 3D hand movements in 6DOF (translation + rotation) and can reliably segment and further enhance their gestures by touchscreen inputs.

* **Head pose tracking:** 6DOF world-scale head tracking (can be used for head-pointing).

* **Eye-gaze tracking:** 6DOF world-scale eye-gaze tracking (can be used for eye-pointing).

* **Body pose tracking:** 6DOF world-scale tracking of the torso (can be used for body-position or ego-centric interactions).

* **No specialized hardware required:** No external hardware, as external trackers or cameras, are required.


## Compatibility
The project was made and tested in Unity 2020.1.6f1, Xcode 12.0.1, iPhone XS and iOS 14.0.1

System requirements:
* iPhone with ARKit capabilities
* Unity
* Xcode
* Mac


## More information
This work is based on a publication [**"Simo: Interactions with Distant Displays by Smartphones with Simultaneous Face and World Tracking"**](https://doi.org/10.1145/3334480.3382962 "Simo publication").
