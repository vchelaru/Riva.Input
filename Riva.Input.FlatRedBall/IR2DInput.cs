using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using System;


namespace Riva.Input.FlatRedBall
{
    public interface IR2DInput : I2DInput, INeedsRefreshInput // Object has to implement this AND FRB I2DInput
    {
        // -- For Settings (serialization / deserialization)

        // Device itentification data
        InputDeviceType DeviceType { get; }
        /*/// <summary>
        /// Device GUID for Generic controllers.
        /// </summary>
        Guid? DeviceID { get; }
        /// <summary>
        /// PlayerIndex for XBox controllers.
        /// </summary>
        PlayerIndex? XNAPlayerIndex { get; }*/

        // -- For input functionality
        /// <summary>
        /// Range -1 to 1
        /// Is not normalized vector.
        /// </summary>
        Vector2 Vector { get; }

        bool GotInput { get; }

        // -- For Settings & input functionality
        // ? ParentDevice  { get; }

    }
}
