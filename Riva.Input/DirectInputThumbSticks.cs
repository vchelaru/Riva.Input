//#define o // debug output

using Microsoft.Xna.Framework;

// Debug
#if DEBUG || o
    using System.Diagnostics;
#endif


namespace Riva.Input
{
    /// <summary>
    /// A struct offering the current positions of as many as 3 thumbsticks on a PC gamepad or joystick
    /// </summary>
    /// <remarks>
    /// For unusual joysticks, these "thumbsticks" may be whatever the hardware-designer imagined;
    /// for example, Right.Y might be a jet-throttle and Right.X might be the rotational position of a steering wheel
    /// In other words, being in the list of Gamepads doesn't mean it looks anything like a Gamepad
    /// </remarks>
    public class DirectInputThumbSticks
	{
        /* ** Range conversion
            raw DX range float:         0  to  32 767.5  to  65 535
            +- range float:         -32767.5  to  0  to  32767.5
            output unit range float:      -1  to  0  to  1


           ** Hor / Vert mapping
           X    Left        Right
                0           65 535
                -32767.5    32767.5
                -1          1

           Y    Top         Bottom
                0           65 535
                -32767.5    32767.5
                -1          1
        */

        // 65 536 = 0 to 65 535
        // center 32767.5
        const float CENTER = 32767.5f;


        public readonly DirectInputDevice ParentDevice;


		/// <summary>
		/// Check HasLeft, HasRight, etc before getting these values; will always be 0 if this gamepad lacks the requested thumbstick.
        /// Range -1 to 1
        /// Is not normalized vector !
		/// </summary>
		public Vector2 Left { get { return _Positions[0]; } }
        /// <summary>
		/// Check HasLeft, HasRight, etc before getting these values; will always be 0 if this gamepad lacks the requested thumbstick.
        /// Range -1 to 1
        /// Is not normalized vector !
		/// </summary>
		public Vector2 Right { get { return _Positions[1]; } }
        /// <summary>
		/// Check HasLeft, HasRight, etc before getting these values; will always be 0 if this gamepad lacks the requested thumbstick.
        /// Range -1 to 1
        /// Is not normalized vector !
		/// </summary>
		public Vector2 Third { get { return _Positions[2]; } }

		public readonly bool HasLeft;
		public readonly bool HasRight;
		public readonly bool HasThird;

        public readonly int NumberOfThumbSticks;
        public int Count { get { return NumberOfThumbSticks; } }

        protected Vector2[] _Positions;

        public Vector2 this[int index]    // Indexer declaration  
        {  
            get { return _Positions[index]; }
        }

#if DEBUG
        protected Vector2[] _RawPositions;
        public Vector2[] RawPositions { get { return _RawPositions; } }
#endif

        // 2 = 100%     0.2 = 10%       0.02 = 1%
        protected float _DeadzoneLeft = 0.4f;       // 0.25f;

        protected float _DeadzoneRight = 0.4f;

        protected float _DeadzoneThird = 0.4f;
        
        /// <summary>In percent of half of stick range.
        /// Default Deadzone 2%</summary>
        public float DeadzoneForLeft { get { return _DeadzoneLeft * 50f; } }
        /// <summary>In percent of half of stick range.
        /// Default Deadzone 2%</summary>
        public float DeadzoneForRight { get { return _DeadzoneLeft * 50f; } }
        /// <summary>In percent of half of stick range.
        /// Default Deadzone 2%</summary>
        public float DeadzoneForThird { get { return _DeadzoneLeft * 50f; } }




        public DirectInputThumbSticks(DirectInputDevice parentDevice)
		{
            ParentDevice = parentDevice;

            NumberOfThumbSticks = parentDevice.RawDevice.Caps.NumberAxes / 2;

            //Debug.WriteLine("_NumberOfThumbSticks: " + _NumberOfThumbSticks);

            _Positions = new Vector2[NumberOfThumbSticks];

#if DEBUG
            _RawPositions = new Vector2[NumberOfThumbSticks];
#endif

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
#if !DEBUG || true
			if (NumberOfThumbSticks > 0) // Left
			{
                //_Positions[0].X = _ConvertRange(ParentDevice.DeviceState.X, ref _DeadzoneNegLeft, ref _DeadzonePosLeft);
                //_Positions[0].Y = -_ConvertRange(ParentDevice.DeviceState.Y, ref _DeadzoneNegLeft, ref _DeadzonePosLeft);

                _Positions[0] = _ConvertRange(ParentDevice.DeviceState.X, ParentDevice.DeviceState.Y, _DeadzoneLeft, true);

				if (NumberOfThumbSticks > 1) // Right
				{
                    //_Positions[1].X = _ConvertRange(ParentDevice.DeviceState.Rz, ref _DeadzoneRight, ref _DeadzonePosRight);
                    //_Positions[1].Y = -_ConvertRange(ParentDevice.DeviceState.Z, ref _DeadzoneRight, ref _DeadzonePosRight);

                    _Positions[1] = _ConvertRange(ParentDevice.DeviceState.Rz, ParentDevice.DeviceState.Z, _DeadzoneRight);

					if (NumberOfThumbSticks > 2) // Third
					{
                        //_Positions[2].X = _ConvertRange(ParentDevice.DeviceState.Rx, ref _DeadzoneThird, ref _DeadzonePosThird);
                        //_Positions[2].Y = -_ConvertRange(ParentDevice.DeviceState.Ry, ref _DeadzoneThird, ref _DeadzonePosThird);

                        _Positions[2] = _ConvertRange(ParentDevice.DeviceState.Rx, ParentDevice.DeviceState.Ry, _DeadzoneThird);
					}
				}
			}
#else
            if (NumberOfThumbSticks > 0)
			{
                _RawPositions[0].X = ParentDevice.DeviceState.X;
                _RawPositions[0].Y = ParentDevice.DeviceState.Y;

				if (NumberOfThumbSticks > 1)
				{
                    _RawPositions[1].X = ParentDevice.DeviceState.Rz;
                    _RawPositions[1].Y = ParentDevice.DeviceState.Z;

					if (NumberOfThumbSticks > 2)
					{
                        _RawPositions[2].X = ParentDevice.DeviceState.Rx;
                        _RawPositions[2].Y = ParentDevice.DeviceState.Ry;
					}
				}
			}
#endif
        }

        //private float _Temp;
        //private void _ConvertRange(float rawXAxisValue, ref ushort deadzoneNeg, ref ushort deadzonePos)
        private Vector2 _ConvertRange(float rawXAxisValue, float rawYAxisValue, float deadzone, bool writeDebug = false)
        {
            /*_Temp = (rawAxisValue - CENTER) / CENTER;

            Debug.WriteLine("rawAxisValue: " + rawAxisValue + "\tConvert result: " + _Temp + "\tis lesser than -1 ? " + (_Temp < -1f));

            if (_Temp < -1f)
                return 0f;
            else
                return _Temp;*/

            // ** true center 32767.5

            /* v1: Bad implementation !
            if (rawAxisValue > deadzoneNeg && rawAxisValue < deadzonePos)
                return 0f;
            else
                return (rawAxisValue - CENTER) / CENTER;*/
                
            // X -1 Left to 1 Right
            // Y -1 Top to 1 Bottom

            // v2: Radial Dead Zone - better:
            Vector2 stickInputNormalized = new Vector2( 
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
            if (stickInputNormalized.Length() < deadzone)
#endif
                stickInputNormalized = Vector2.Zero;

            // X -1 Left to 1 Right
            // Y -1 Top to 1 Bottom

            return stickInputNormalized;
        }

        /// <summary>In percent of half of stick range.</summary>
        public void SetDeadzoneForAll(byte percent)
        {
            var inUnits = percent / 50f;
            _DeadzoneLeft = _DeadzoneRight = _DeadzoneThird = inUnits;
        }
        /// <summary>In percent of half of stick range.</summary>
        public void SetDeadzoneForLeft(byte percent)
        {
            _DeadzoneLeft = percent / 50f;
        }
        /// <summary>In percent of half of stick range.</summary>
        public void SetDeadzoneForRight(byte percent)
        {
            _DeadzoneRight = percent / 50f;
        }
        /// <summary>In percent of half of stick range.</summary>
        public void SetDeadzoneForThird(byte percent)
        {
            _DeadzoneThird = percent / 50f;
        }
	}
}
