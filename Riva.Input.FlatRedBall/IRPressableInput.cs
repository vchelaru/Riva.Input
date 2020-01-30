using System;
using FlatRedBall.Input;


namespace Riva.Input.FlatRedBall
{
    public interface IRPressableInput : IPressableInput // Object has to implement this AND FRB IPressableInput
    {
        // -- For Settings (serialization / deserialization)
        eInputDeviceType DeviceType { get; }

        /*/// <summary>
        /// Device GUID for Generic controllers.
        /// </summary>
        Guid? DeviceID { get; }
        /// <summary>
        /// PlayerIndex for XBox controllers.
        /// </summary>
        PlayerIndex? XNAPlayerIndex { get; }*/

        /// <summary>
        /// ID number of controler button or keyboard key
        /// </summary>
        int ButtonNumber { get; }
    }
}
