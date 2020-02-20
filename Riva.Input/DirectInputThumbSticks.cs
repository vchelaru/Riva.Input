//#define o // debug output

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections;
using System;

// Debug
#if DEBUG || o
using System.Diagnostics;
#endif


namespace Riva.Input
{
    /// <summary>
    /// A struct offering the current positions of as many as 3 thumbsticks on a PC gamepad or joystick.
    /// </summary>
    /// <remarks>
    /// I only know how to connect input variables from DX to 3 (actually reliably 2) ThumbSticks. 
    /// So only 3 ThumbSticks are supported in this class.
    /// </br></br>
    /// For unusual joysticks, these "thumbsticks" may be whatever the hardware-designer imagined.
    /// For example, Right.Y might be a jet-throttle and Right.X might be the rotational position of a steering wheel.
    /// In other words, being in the list of Gamepads doesn't mean it looks anything like a Gamepad.
    /// </remarks>
    public class DirectInputThumbSticks : IEnumerable<Vector2>
	{
        /* ** Range conversion
            raw DX range float:         0  to  32 767.5  to  65 535
            +- range float:         -32767.5  to  0  to  32767.5
            output range float:           -1  to  0  to  1


           ** Hor / Vert mapping
           X    Left        Right
                0           65 535
                -32767.5    32767.5
                -1          1

           Y    Top         Bottom
                0           65 535
                32767.5    -32767.5
                1          -1
        */

        // 65 536 = 0 to 65 535
        // center 32767.5
        const float CENTER = 32767.5f;

        const float VECTOR_RANGE1TO1_MAX_LENGTH = 1.4142135623730951f;
        const float VECTOR_RANGE1TO1_LENGTH_FROM_PERCENT = 0.014142135623730951f;

        const float DEFAULT_DEADZONE_THRESHOLD = 0.3535f; // cca 25% of thumbstick range

        public readonly DirectInputGamepad ParentDevice;


		/// <summary>
		/// Check HasLeft, HasRight, etc before getting these values; will always be 0 if this gamepad lacks the requested thumbstick.
        /// Range -1 to 1.
        /// -1 left, 1 right. -1 down, 1 up.
        /// Is not normalized vector !
		/// </summary>
		public Vector2 Left { get { return _Positions[0]; } }
        /// <summary>
		/// Check HasLeft, HasRight, etc before getting these values; will always be 0 if this gamepad lacks the requested thumbstick.
        /// Range -1 to 1
        /// -1 left, 1 right. -1 down, 1 up.
        /// Is not normalized vector !
		/// </summary>
		public Vector2 Right { get { return _Positions[1]; } }
        /// <summary>
		/// Check HasLeft, HasRight, etc before getting these values; will always be 0 if this gamepad lacks the requested thumbstick.
        /// Range -1 to 1.
        /// -1 left, 1 right. -1 down, 1 up.
        /// Is not normalized vector !
		/// </summary>
		public Vector2 Third { get { return _Positions[2]; } }

		public readonly bool HasLeft;
		public readonly bool HasRight;
		public readonly bool HasThird;

        public readonly int NumberOfThumbSticks;
        public int Count { get { return NumberOfThumbSticks; } }

        protected Vector2[] _Positions;

        public Vector2 this[byte index]    // Indexer declaration  
        {  
            get { return _Positions[index]; }
        }

#if DEBUG
        protected Vector2[] _RawPositions;
        public Vector2[] RawPositions { get { return _RawPositions; } }
#endif

        // 1 = 100%     0.1 = 10%       0.01 = 1%
        protected float[] _DeadzoneThresholds_Range0to1_4;
        
        /// <summary>In percent of half of stick range.
        /// Default Deadzone 25%</summary>
        public float DeadzoneForLeft { get { return _DeadzoneThresholds_Range0to1_4[0] / VECTOR_RANGE1TO1_LENGTH_FROM_PERCENT; } }
        /// <summary>In percent of half of stick range.
        /// Default Deadzone 25%</summary>
        public float DeadzoneForRight { get { return _DeadzoneThresholds_Range0to1_4[1] / VECTOR_RANGE1TO1_LENGTH_FROM_PERCENT; } }
        /// <summary>In percent of half of stick range.
        /// Default Deadzone 25%</summary>
        public float DeadzoneForThird { get { return _DeadzoneThresholds_Range0to1_4[2] / VECTOR_RANGE1TO1_LENGTH_FROM_PERCENT; } }




        public DirectInputThumbSticks(DirectInputGamepad parentDevice)
		{
            ParentDevice = parentDevice;

            NumberOfThumbSticks = Math.Min(parentDevice.RawDevice.Caps.NumberAxes / 2, 3);

            //Debug.WriteLine("_NumberOfThumbSticks: " + _NumberOfThumbSticks);

            _Positions = new Vector2[NumberOfThumbSticks];

#if DEBUG
            _RawPositions = new Vector2[NumberOfThumbSticks];
#endif

            _DeadzoneThresholds_Range0to1_4 = new float[NumberOfThumbSticks];
            for (int i = 0; i < NumberOfThumbSticks; i++)
                _DeadzoneThresholds_Range0to1_4[i] = DEFAULT_DEADZONE_THRESHOLD;

            if (NumberOfThumbSticks > 0)
			{
				HasLeft = true;

				if (NumberOfThumbSticks > 1)
				{
					HasRight = true;

					if (NumberOfThumbSticks > 2)
					{
						HasThird = true;
					}
				}
			}
		}




        public void Refresh()
        {
			if (NumberOfThumbSticks > 0) // Left
			{
                //_Positions[0].X = _ConvertRange(ParentDevice.DeviceState.X, ref _DeadzoneNegLeft, ref _DeadzonePosLeft);
                //_Positions[0].Y = -_ConvertRange(ParentDevice.DeviceState.Y, ref _DeadzoneNegLeft, ref _DeadzonePosLeft);

                _Positions[0] = _ConvertRange(ParentDevice.RawDeviceState.X, ParentDevice.RawDeviceState.Y, _DeadzoneThresholds_Range0to1_4[0]);

				if (NumberOfThumbSticks > 1) // Right
				{
                    //_Positions[1].X = _ConvertRange(ParentDevice.DeviceState.Rz, ref _DeadzoneRight, ref _DeadzonePosRight);
                    //_Positions[1].Y = -_ConvertRange(ParentDevice.DeviceState.Z, ref _DeadzoneRight, ref _DeadzonePosRight);

                    _Positions[1] = _ConvertRange(ParentDevice.RawDeviceState.Rz, ParentDevice.RawDeviceState.Z, _DeadzoneThresholds_Range0to1_4[1]);

					if (NumberOfThumbSticks > 2) // Third
					{
                        //_Positions[2].X = _ConvertRange(ParentDevice.DeviceState.Rx, ref _DeadzoneThird, ref _DeadzonePosThird);
                        //_Positions[2].Y = -_ConvertRange(ParentDevice.DeviceState.Ry, ref _DeadzoneThird, ref _DeadzonePosThird);

                        _Positions[2] = _ConvertRange(ParentDevice.RawDeviceState.Rx, ParentDevice.RawDeviceState.Ry, _DeadzoneThresholds_Range0to1_4[2]);
					}
				}
			}
#if DEBUG
            if (NumberOfThumbSticks > 0)
			{
                _RawPositions[0].X = ParentDevice.RawDeviceState.X;
                _RawPositions[0].Y = ParentDevice.RawDeviceState.Y;

				if (NumberOfThumbSticks > 1)
				{
                    _RawPositions[1].X = ParentDevice.RawDeviceState.Rz;
                    _RawPositions[1].Y = ParentDevice.RawDeviceState.Z;

					if (NumberOfThumbSticks > 2)
					{
                        _RawPositions[2].X = ParentDevice.RawDeviceState.Rx;
                        _RawPositions[2].Y = ParentDevice.RawDeviceState.Ry;
					}
				}
			}
#endif
        }

        private Vector2 _ConvertRange(float rawXAxisValue, float rawYAxisValue, float deadzone_Range0to1, bool writeDebug = false)
        {
            // ** true center 32767.5

            /* v1: Naive deadzone (per axis) - bad:
            if (rawAxisValue > deadzoneNeg && rawAxisValue < deadzonePos)
                return 0f;
            else
                return (rawAxisValue - CENTER) / CENTER;*/
                
            // X: -1 Left to 1 Right
            // Y: 1 Top to -1 Bottom

            // v2: Radial Dead Zone - better:
            Vector2 stickInput_RangeNeg1Pos1 = new Vector2( 
                    (rawXAxisValue - CENTER) / CENTER, 
                    -( (rawYAxisValue - CENTER) / CENTER )
                );
#if o
            var length = stickInputNormalized.Length();
            Debug.WriteLineIf(
                writeDebug, 
                $"* Riva.Input.DirectInputThumbSticks._ConvertRange(): raw values {rawXAxisValue} {rawYAxisValue}   stickInputNormalized {stickInputNormalized}   length {length}"
            );
            if (length < deadzone)
#else
            if (stickInput_RangeNeg1Pos1.Length() < deadzone_Range0to1)
#endif
                return Vector2.Zero;

            /* v3: Scaled Radial Dead Zone - smooth transition from deadzone:
            float deadzone = 0.25f;
            Vector2 stickInput = new Vector2(Input.GetAxis(“Horizontal”), Input.GetAxis(“Vertical”));
            if(stickInput.magnitude < deadzone)
                stickInput = Vector2.zero;
            else
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
            */

            // X -1 Left to 1 Right
            // Y 1 Top to -1 Bottom

            return stickInput_RangeNeg1Pos1;
        }

        /// <summary>In percent of half of stick range (radius).</summary>
        public void SetDeadzoneForAll(byte percent)
        {
            var inRange0to1_4 = MathHelper.Clamp(percent * VECTOR_RANGE1TO1_LENGTH_FROM_PERCENT, 0f, VECTOR_RANGE1TO1_MAX_LENGTH);
            for (int i = 0; i < NumberOfThumbSticks; i++)
                _DeadzoneThresholds_Range0to1_4[i] = inRange0to1_4;
        }
        /// <summary>In percent of half of stick range (radius).</summary>
        public void SetDeadzone(byte thumbStickIndex, byte percent)
        {
            _DeadzoneThresholds_Range0to1_4[thumbStickIndex] = 
                MathHelper.Clamp(percent * VECTOR_RANGE1TO1_LENGTH_FROM_PERCENT, 0f, VECTOR_RANGE1TO1_MAX_LENGTH);
        }


        public IEnumerator<Vector2> GetEnumerator()
        {
            return ((IEnumerable<Vector2>)_Positions).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Positions.GetEnumerator();
        }


        public override string ToString()
        {
            return $"{base.ToString()}{{ParentDevice: {ParentDevice.Name}}}";
        }
    }
}
