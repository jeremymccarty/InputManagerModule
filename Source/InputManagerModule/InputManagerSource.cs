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
using UnityEngine;
using XInputDotNetPure;

namespace InputManagerModule
{
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
        public PlayerIndex PlayerIndex;
        public bool LoadedByDefault = true;

        /* State bools */
        private bool _faceDownPressed;
        private bool _faceLeftPressed;
        private bool _faceUpPressed;
        private bool _faceRightPressed;
        private bool _specialRightPressed;
        private bool _specialLeftPressed;
        private bool _rightShoulderPressed;
        private bool _leftShoulderPressed;
        private bool _homePressed;
        private bool _isRumbling;
        private bool _inputActive;

        // Use this for initialization
        void Start()
        {
            // Initialize the state of this controller
            _inputActive = LoadedByDefault;
        }

        // Update is called once per frame
        void Update()
        {
            // If this gamepad isn't active, return
            if (!_inputActive)
                return;

            // Save our current state
            GamePadState state = GamePad.GetState(PlayerIndex);
            GamePadButtons button = state.Buttons;

            // Check for pressed buttons
            EvaluateButtonPresses(button);

            // Check for axis inputs (triggers, joysticks, dpad)
            EvaluateAxisInput(state);

            // Check for released buttons
            EvaluateButtonReleases(button);
        }

        // Evaluate the current value of each axis input and call the delegates
        private void EvaluateAxisInput(GamePadState state)
        {
            // Process input for trigger buttons
            #region Right Trigger

            OnRightTriggerPressed?.Invoke(PlayerIndex, state.Triggers.Right);

            #endregion

            #region Left Trigger

            OnLeftTriggerPressed?.Invoke(PlayerIndex, state.Triggers.Left);

            #endregion

            // Process input for axiis
            #region Joystick Right

            Vector2 axisValue = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            OnRightJoystickMove?.Invoke(PlayerIndex, axisValue);

            #endregion

            #region Joystick Left

            axisValue = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            OnLeftJoystickMove?.Invoke(PlayerIndex, axisValue);

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

            OnDpadMove?.Invoke(PlayerIndex, axisValue);

            #endregion
        }

        // Calls the assigned delegates when the input buttons are pressed
        private void EvaluateButtonPresses(GamePadButtons button)
        {
            // Process input for face buttons
            #region Face Down

            if (button.A == ButtonState.Pressed)
            {
                OnFaceDownHeld?.Invoke(PlayerIndex);

                if (!_faceDownPressed)
                    OnFaceDownPressed?.Invoke(PlayerIndex);

                _faceDownPressed = true;
            }

            #endregion

            #region Face Left

            if (button.X == ButtonState.Pressed)
            {
                OnFaceLeftHeld?.Invoke(PlayerIndex);

                if (!_faceLeftPressed)
                    OnFaceLeftPressed?.Invoke(PlayerIndex);

                _faceLeftPressed = true;
            }

            #endregion

            #region Face Up

            if (button.Y == ButtonState.Pressed)
            {
                OnFaceUpHeld?.Invoke(PlayerIndex);

                if (!_faceUpPressed)
                    OnFaceUpPressed?.Invoke(PlayerIndex);

                _faceUpPressed = true;
            }

            #endregion

            #region Face Right

            if (button.B == ButtonState.Pressed)
            {
                OnFaceRightHeld?.Invoke(PlayerIndex);

                if (!_faceRightPressed)
                    OnFaceRightPressed?.Invoke(PlayerIndex);

                _faceRightPressed = true;
            }

            #endregion

            // Process input for special buttons
            #region Special Left

            if (button.Back == ButtonState.Pressed)
            {
                OnSpecialLeftHeld?.Invoke(PlayerIndex);

                if (!_specialLeftPressed)
                    OnSpecialLeftPressed?.Invoke(PlayerIndex);

                _specialLeftPressed = true;
            }

            #endregion

            #region Special Right

            if (button.Start == ButtonState.Pressed)
            {
                OnSpecialRightHeld?.Invoke(PlayerIndex);

                if (!_specialRightPressed)
                    OnSpecialRightPressed?.Invoke(PlayerIndex);

                _specialRightPressed = true;
            }

            #endregion

            // Process input for shoulder buttons
            #region Right Shoulder

            if (button.RightShoulder == ButtonState.Pressed)
            {
                OnRightShoulderHeld?.Invoke(PlayerIndex);

                if (!_rightShoulderPressed)
                    OnRightShoulderPressed?.Invoke(PlayerIndex);

                _rightShoulderPressed = true;
            }

            #endregion

            #region Left Shoulder

            if (button.LeftShoulder == ButtonState.Pressed)
            {
                OnLeftShoulderHeld?.Invoke(PlayerIndex);

                if (!_leftShoulderPressed)
                    OnLeftShoulderPressed?.Invoke(PlayerIndex);

                _leftShoulderPressed = true;
            }

            #endregion

            // Process input for home button
            #region Home
            if (button.Guide == ButtonState.Pressed)
            {
                OnHomeHeld?.Invoke(PlayerIndex);

                if (!_homePressed)
                    OnHomePressed?.Invoke(PlayerIndex);

                _homePressed = true;
            }
            #endregion
        }

        // Calls the assigned delegates when the input buttons are released
        private void EvaluateButtonReleases(GamePadButtons button)
        {
            // Process input for face buttons
            #region Face Down

            if (button.A == ButtonState.Released && _faceDownPressed)
            {
                OnFaceDownReleased?.Invoke(PlayerIndex);
                _faceDownPressed = false;
            }

            #endregion

            #region Face Left

            if (button.X == ButtonState.Released && _faceLeftPressed)
            {
                OnFaceLeftReleased?.Invoke(PlayerIndex);
                _faceLeftPressed = false;
            }

            #endregion

            #region Face Up

            if (button.Y == ButtonState.Released && _faceUpPressed)
            {
                OnFaceUpReleased?.Invoke(PlayerIndex);
                _faceUpPressed = false;
            }

            #endregion

            #region Face Right

            if (button.B == ButtonState.Released && _faceRightPressed)
            {
                OnFaceRightReleased?.Invoke(PlayerIndex);
                _faceRightPressed = false;
            }

            #endregion

            // Process input for special buttons
            #region Special Left

            if (button.Back == ButtonState.Released && _specialLeftPressed)
            {
                OnSpecialLeftReleased?.Invoke(PlayerIndex);
                _specialLeftPressed = false;
            }

            #endregion

            #region Special Right

            if (button.Start == ButtonState.Released && _specialRightPressed)
            {
                OnSpecialRightReleased?.Invoke(PlayerIndex);
                _specialRightPressed = false;
            }

            #endregion

            // Process input for shoulder buttons
            #region Right Shoulder

            if (button.RightShoulder == ButtonState.Released && _rightShoulderPressed)
            {
                OnRightShoulderReleased?.Invoke(PlayerIndex);
                _rightShoulderPressed = false;
            }

            #endregion

            #region Left Shoulder

            if (button.LeftShoulder == ButtonState.Released && _leftShoulderPressed)
            {
                OnLeftShoulderReleased?.Invoke(PlayerIndex);
                _leftShoulderPressed = false;
            }

            #endregion

            // Process input for home button
            #region Home
            if (button.Guide == ButtonState.Released && _homePressed)
            {
                OnHomeReleased?.Invoke(PlayerIndex);
                _homePressed = false;
            }
            #endregion
        }

        // Rumble for t time at the given strength of each motor (defaults to 1)
        public IEnumerator Rumble(float time, float leftMotor = 1, float rightMotor = 1)
        {
            // If we are rumbling, return
            if (_isRumbling) yield return null;

            // Otherwise, set rumbling on
            _isRumbling = true;

            // Set vibration and wait for t time
            GamePad.SetVibration(PlayerIndex, leftMotor, rightMotor);
            yield return new WaitForSecondsRealtime(time);

            // Reset rumbling when complete
            _isRumbling = false;
            GamePad.SetVibration(PlayerIndex, 0, 0);
        }

        // Allows users to activate and deactivate the controllers
        public void SetInputActive(bool activeValue)
        {
            _inputActive = activeValue;
        }
    }
}
