using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for setting the device ID to target for the PiShock over serial
    /// </summary>
    public class PiShockSerialDeviceIdOption : IPunishmentTextInput
    {
        /// <summary>
        /// The ctor creating a <see cref="PiShockSerialDeviceIdOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PiShockSerialDeviceIdOption(SpriteFont font) : base(font, DeviceManager.PiShockSerialPreferences.DeviceId, 280, 75, true)
        {
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Device ID: ";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void OnTextChange(string newValue)
        {
            DeviceManager.PiShockSerialPreferences.DeviceId = newValue;
        }

        /// <inheritdoc/>
        protected override bool IsAllowedCharacter(char c)
        {
            return char.IsLetterOrDigit(c);
        }
    }
}
