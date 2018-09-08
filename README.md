# InputManagerModule
## Description
This .dll holds the implementation of a delegate based input manager for use with the Unity game engine. Essentially, it serves as a delegate interface for an XInput wrapper created by [Rémi Gillig](https://github.com/speps/XInputDotNet). This release has been created to aid student developers at DigiPen Institute of Technology.

## Background
To learn more about why you should use this system, you can access a slide deck [through this link](https://www.dropbox.com/s/bclenqnii530qi6/Delegate%20Based%20Input.pptx?dl=0).

## Setup
- Get the latest release of the InputManagerModule from [the release page](https://github.com/jeremymccarty/InputManagerModule/releases/latest)
- Open Unity
- Import the package by selecting Assets > Import Package > Custom Package and navigating to the correct .unitypackage
- Add the InputManager prefab to your scenes
- Refer to the Examples below for implementation

## Implementation
- Create a new .cs script
- Include the correct libraries (See Libraries below)
- Create a public PlayerIndex variable
- Create OnEnable() and OnDisable() functions
- Write function declarations (See SampleScript.cs)
- Subscribe to events (See SampleScript.cs)
- Write implementation
- If you are using multiple controllers, write one, single script for functionality and modify the PlayerIndex variable of each object to differentiate (See PlayerController.cs)

## Examples
Examples can be found in the SampleScript.cs and PlayerController.cs files that are generated on Import or in the SampleScene (note the SampleScene uses advanced Unity methodologies such as Unity Events and Scriptable Objects, refer to the SampleScript.cs for ALL functionality).

## Libraries
- InputManagerModule.dll, utility DLL which creates the delegate to interface with the XInputInterface.dll
  - You need to add a reference to this one in your C# project for example
- XInputDotNetPure.dll, .NET assembly containing the GamePad class
  - You need to add a reference to this one in your C# project for example

## Additional Resources
- This project was made possible with the XInputDotNet wrapper from Rémi Gillig [which can be found here](https://github.com/speps/XInputDotNet).
- For hardware support for Sony Playstation gamepads, I recommend using [DS4Windows](http://ds4windows.com/).

## Credit
Use "Input Manager Module Copyright (c) 2018 Jeremy McCarty All Rights Reserved" in your credits
