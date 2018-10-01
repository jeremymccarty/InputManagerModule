/****************************************************************************/
/*!
\file   InputManagerSource.cs
\author Jeremy McCarty
\par    email: jeremymmccarty\@gmail.com
\brief
This file contains the library implemntation of an input manager module for Unity.
"Copyright (c) 2018 Jeremy McCarty <jeremymmccarty@gmail.com>"
"Portions Copyright (c) 2009 Remi Gillig <remigillig@gmail.com>"
All rights reserved worldwide.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
/****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace InputManagerModule
{
    public class GamepadInput
    {
        public GamepadInput() {}

        public bool _isActive { set; get; }
        public bool _faceDownPressed { set; get; }
        public bool _faceLeftPressed { set; get; }
        public bool _faceUpPressed { set; get; }
        public bool _faceRightPressed { set; get; }
        public bool _specialRightPressed { set; get; }
        public bool _specialLeftPressed { set; get; }
        public bool _rightShoulderPressed { set; get; }
        public bool _leftShoulderPressed { set; get; }
        public bool _homePressed { set; get; }
        public bool _isRumbling { set; get; }
    };

    public class InputManager : MonoBehaviour
    {
        #region Axis Delegates
        /* Joystick and DPAD */
        public delegate void LeftJoystickMoved(PlayerIndex index, Vector2 axis);
        public static event LeftJoystickMoved OnLeftJoystickMove;

        public delegate void RightJoystickMoved(PlayerIndex index, Vector2 axis);
        public static event RightJoystickMoved OnRightJoystickMove;

        public delegate void DpadMoved(PlayerIndex index, Vector2 axis);
        public static event DpadMoved OnDpadMove;

        /* Trigger Buttons */
        public delegate void RightTriggerPressed(PlayerIndex index, float axisValue);
        public static event RightTriggerPressed OnRightTriggerPressed;

        public delegate void LeftTriggerPressed(PlayerIndex index, float axisValue);
        public static event LeftTriggerPressed OnLeftTriggerPressed;
        #endregion

        #region Pressed Delegates
        /* Face Buttons */
        public delegate void FaceDownPressed(PlayerIndex index);
        public static event FaceDownPressed OnFaceDownPressed;

        public delegate void FaceLeftPressed(PlayerIndex index);
        public static event FaceLeftPressed OnFaceLeftPressed;

        public delegate void FaceRightPressed(PlayerIndex index);
        public static event FaceRightPressed OnFaceRightPressed;

        public delegate void FaceUpPressed(PlayerIndex index);
        public static event FaceUpPressed OnFaceUpPressed;

        /* Special Buttons */
        public delegate void SpecialRightPressed(PlayerIndex index);
        public static event SpecialRightPressed OnSpecialRightPressed;

        public delegate void SpecialLeftPressed(PlayerIndex index);
        public static event SpecialLeftPressed OnSpecialLeftPressed;

        /* Shoulder Buttons */
        public delegate void RightShoulderPressed(PlayerIndex index);
        public static event RightShoulderPressed OnRightShoulderPressed;

        public delegate void LeftShoulderPressed(PlayerIndex index);
        public static event LeftShoulderPressed OnLeftShoulderPressed;

        /* Home Button */
        public delegate void HomePressed(PlayerIndex index);
        public static event HomePressed OnHomePressed;
        #endregion

        #region Released Delegates
        /* Face Buttons */
        public delegate void FaceDownReleased(PlayerIndex index);
        public static event FaceDownReleased OnFaceDownReleased;

        public delegate void FaceLeftReleased(PlayerIndex index);
        public static event FaceLeftReleased OnFaceLeftReleased;

        public delegate void FaceRightReleased(PlayerIndex index);
        public static event FaceRightReleased OnFaceRightReleased;

        public delegate void FaceUpReleased(PlayerIndex index);
        public static event FaceUpReleased OnFaceUpReleased;

        /* Special Buttons */
        public delegate void SpecialRightReleased(PlayerIndex index);
        public static event SpecialRightReleased OnSpecialRightReleased;

        public delegate void SpecialLeftReleased(PlayerIndex index);
        public static event SpecialLeftReleased OnSpecialLeftReleased;

        /* Shoulder Buttons */
        public delegate void RightShoulderReleased(PlayerIndex index);
        public static event RightShoulderReleased OnRightShoulderReleased;

        public delegate void LeftShoulderReleased(PlayerIndex index);
        public static event LeftShoulderReleased OnLeftShoulderReleased;

        /* Home Button */
        public delegate void HomeReleased(PlayerIndex index);
        public static event HomeReleased OnHomeReleased;
        #endregion

        #region Held Delegates
        /* Face Buttons */
        public delegate void FaceDownHeld(PlayerIndex index);
        public static event FaceDownHeld OnFaceDownHeld;

        public delegate void FaceLeftHeld(PlayerIndex index);
        public static event FaceLeftHeld OnFaceLeftHeld;

        public delegate void FaceRightHeld(PlayerIndex index);
        public static event FaceRightHeld OnFaceRightHeld;

        public delegate void FaceUpHeld(PlayerIndex index);
        public static event FaceUpHeld OnFaceUpHeld;

        /* Special Buttons */
        public delegate void SpecialRightHeld(PlayerIndex index);
        public static event SpecialRightHeld OnSpecialRightHeld;

        public delegate void SpecialLeftHeld(PlayerIndex index);
        public static event SpecialLeftHeld OnSpecialLeftHeld;

        /* Shoulder Buttons */
        public delegate void RightShoulderHeld(PlayerIndex index);
        public static event RightShoulderHeld OnRightShoulderHeld;

        public delegate void LeftShoulderHeld(PlayerIndex index);
        public static event LeftShoulderHeld OnLeftShoulderHeld;

        /* Home Button */
        public delegate void HomeHeld(PlayerIndex index);
        public static event HomeHeld OnHomeHeld;
        #endregion

        /* Public variables */
        public bool LoadedByDefault = true;
        public int PlayerCount = 4;

        /* Collection of connected gamepads */
        private static GamepadInput[] _activeGamepads = new GamepadInput[4];

        // Use this for initialization
        void Start()
        {
            // Initialize the active state of each controller and add 4 new gamepads to the class array
            for (int i = 0; i < PlayerCount; ++i)
                _activeGamepads[i] = new GamepadInput { _isActive = LoadedByDefault };
        }

        // Update is called once per frame
        void Update()
        {
            // For each CONNECTED controller
            for (int i = 0; i < PlayerCount; ++i)
            {
                // Save the current controller
                PlayerIndex currentIndex = (PlayerIndex)i;

                // Verify that the controller is connected and active
                if (!GamePad.GetState(currentIndex).IsConnected || !_activeGamepads[i]._isActive)
                    continue;

                // Save our current state
                GamePadState state = GamePad.GetState(currentIndex);
                GamePadButtons button = state.Buttons;

                // Check for pressed buttons
                EvaluateButtonPresses(currentIndex, button);

                // Check for axis inputs (triggers, joysticks, dpad)
                EvaluateAxisInput(currentIndex, state);

                // Check for released buttons
                EvaluateButtonReleases(currentIndex, button);
            }
        }

        // Evaluate the current value of each axis input and call the delegates
        private void EvaluateAxisInput(PlayerIndex currentIndex, GamePadState state)
        {
            // Process input for trigger buttons
            #region Right Trigger

            OnRightTriggerPressed?.Invoke(currentIndex, state.Triggers.Right);

            #endregion

            #region Left Trigger

            OnLeftTriggerPressed?.Invoke(currentIndex, state.Triggers.Left);

            #endregion

            // Process input for axiis
            #region Joystick Right

            Vector2 axisValue = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            OnRightJoystickMove?.Invoke(currentIndex, axisValue);

            #endregion

            #region Joystick Left

            axisValue = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            OnLeftJoystickMove?.Invoke(currentIndex, axisValue);

            #endregion

            #region DPAD

            axisValue = Vector2.zero;
            if (state.DPad.Left == ButtonState.Pressed)
                axisValue.x = -1;

            if (state.DPad.Right == ButtonState.Pressed)
                axisValue.x = 1;

            if (state.DPad.Up == ButtonState.Pressed)
                axisValue.y = 1;

            if (state.DPad.Down == ButtonState.Pressed)
                axisValue.y = -1;

            OnDpadMove?.Invoke(currentIndex, axisValue);

            #endregion
        }

        // Calls the assigned delegates when the input buttons are pressed
        private void EvaluateButtonPresses(PlayerIndex currentIndex, GamePadButtons button)
        {
            // Process input for face buttons
            #region Face Down

            if (button.A == ButtonState.Pressed)
            {
                OnFaceDownHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._faceDownPressed)
                    OnFaceDownPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._faceDownPressed = true;
            }

            #endregion

            #region Face Left

            if (button.X == ButtonState.Pressed)
            {
                OnFaceLeftHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._faceLeftPressed)
                    OnFaceLeftPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._faceLeftPressed = true;
            }

            #endregion

            #region Face Up

            if (button.Y == ButtonState.Pressed)
            {
                OnFaceUpHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._faceUpPressed)
                    OnFaceUpPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._faceUpPressed = true;
            }

            #endregion

            #region Face Right

            if (button.B == ButtonState.Pressed)
            {
                OnFaceRightHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._faceRightPressed)
                    OnFaceRightPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._faceRightPressed = true;
            }

            #endregion

            // Process input for special buttons
            #region Special Left

            if (button.Back == ButtonState.Pressed)
            {
                OnSpecialLeftHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._specialLeftPressed)
                    OnSpecialLeftPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._specialLeftPressed = true;
            }

            #endregion

            #region Special Right

            if (button.Start == ButtonState.Pressed)
            {
                OnSpecialRightHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._specialRightPressed)
                    OnSpecialRightPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._specialRightPressed = true;
            }

            #endregion

            // Process input for shoulder buttons
            #region Right Shoulder

            if (button.RightShoulder == ButtonState.Pressed)
            {
                OnRightShoulderHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._rightShoulderPressed)
                    OnRightShoulderPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._rightShoulderPressed = true;
            }

            #endregion

            #region Left Shoulder

            if (button.LeftShoulder == ButtonState.Pressed)
            {
                OnLeftShoulderHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._leftShoulderPressed)
                    OnLeftShoulderPressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._leftShoulderPressed = true;
            }

            #endregion

            // Process input for home button
            #region Home
            if (button.Guide == ButtonState.Pressed)
            {
                OnHomeHeld?.Invoke(currentIndex);

                if (!_activeGamepads[(int)currentIndex]._homePressed)
                    OnHomePressed?.Invoke(currentIndex);

                _activeGamepads[(int)currentIndex]._homePressed = true;
            }
            #endregion
        }

        // Calls the assigned delegates when the input buttons are released
        private void EvaluateButtonReleases(PlayerIndex currentIndex, GamePadButtons button)
        {
            // Process input for face buttons
            #region Face Down

            if (button.A == ButtonState.Released && _activeGamepads[(int)currentIndex]._faceDownPressed)
            {
                OnFaceDownReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._faceDownPressed = false;
            }

            #endregion

            #region Face Left

            if (button.X == ButtonState.Released && _activeGamepads[(int)currentIndex]._faceLeftPressed)
            {
                OnFaceLeftReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._faceLeftPressed = false;
            }

            #endregion

            #region Face Up

            if (button.Y == ButtonState.Released && _activeGamepads[(int)currentIndex]._faceUpPressed)
            {
                OnFaceUpReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._faceUpPressed = false;
            }

            #endregion

            #region Face Right

            if (button.B == ButtonState.Released && _activeGamepads[(int)currentIndex]._faceRightPressed)
            {
                OnFaceRightReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._faceRightPressed = false;
            }

            #endregion

            // Process input for special buttons
            #region Special Left

            if (button.Back == ButtonState.Released && _activeGamepads[(int)currentIndex]._specialLeftPressed)
            {
                OnSpecialLeftReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._specialLeftPressed = false;
            }

            #endregion

            #region Special Right

            if (button.Start == ButtonState.Released && _activeGamepads[(int)currentIndex]._specialRightPressed)
            {
                OnSpecialRightReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._specialRightPressed = false;
            }

            #endregion

            // Process input for shoulder buttons
            #region Right Shoulder

            if (button.RightShoulder == ButtonState.Released && _activeGamepads[(int)currentIndex]._rightShoulderPressed)
            {
                OnRightShoulderReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._rightShoulderPressed = false;
            }

            #endregion

            #region Left Shoulder

            if (button.LeftShoulder == ButtonState.Released && _activeGamepads[(int)currentIndex]._leftShoulderPressed)
            {
                OnLeftShoulderReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._leftShoulderPressed = false;
            }

            #endregion

            // Process input for home button
            #region Home
            if (button.Guide == ButtonState.Released && _activeGamepads[(int)currentIndex]._homePressed)
            {
                OnHomeReleased?.Invoke(currentIndex);
                _activeGamepads[(int)currentIndex]._homePressed = false;
            }
            #endregion
        }

        // Rumble for t time at the given strength of each motor (defaults to 1)
        public static IEnumerator Rumble(PlayerIndex index, float time, float leftMotor = 1, float rightMotor = 1)
        {
            // If we are rumbling, return
            if (_activeGamepads[(int)index]._isRumbling) yield return null;

            // Otherwise, set rumbling on
            _activeGamepads[(int)index]._isRumbling = true;

            GamePad.SetVibration(index, leftMotor, rightMotor);
            yield return new WaitForSecondsRealtime(time);

            // Reset rumbling when complete
            _activeGamepads[(int)index]._isRumbling = false;
            GamePad.SetVibration(index, 0, 0);
        }

        // Allows users to activate and deactivate the controllers
        public static void SetInputActive(PlayerIndex index, bool activeValue)
        {
            // Find the controller in the array and set it's state
            _activeGamepads[(int)index]._isActive = activeValue;
        }
    }
}