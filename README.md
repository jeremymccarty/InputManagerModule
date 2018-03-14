# InputManagerModule
## Description
This .dll holds the implementation of a delegate based input manager for use with the Unity game engine. Essentially, it serves as a delegate interface for an XInput wrapper created by [Rémi Gillig](https://github.com/speps/XInputDotNet). This release has been created to aid student developers at DigiPen.

## Setup
- Get the latest release of the InputManagerModule from [the release page](/Releases)
- Open Unity
- Import the package by selecting Assets > Import Package > Custom Package
- Attach the InputManager component to any actor needing gamepad input

## Examples
Examples can be found in the SampleScript.cs file that is generated on Import.

## Libraries
- InputManagerModule.dll, utility DLL which creates the delegate to interface with the XInputInterface.dll
  - You need to add a reference to this one in your C# project for example
- XInputDotNetPure.dll, .NET assembly containing the GamePad class
  - You need to add a reference to this one in your C# project for example
- XInputInterface.dll, utility DLL which makes the calls to XInput
  - You have to copy this one next to your .exe

## Additional Resources
- This project was made possible with the XInputDotNet wrapper from Rémi Gillig [which can be found here](https://github.com/speps/XInputDotNet).
- For hardware support for Sony Playstation gamepads, I recommend using [DS4Windows](http://ds4windows.com/).