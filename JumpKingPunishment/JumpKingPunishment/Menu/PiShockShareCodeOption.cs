using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for setting th Share Code for the PiShock
    /// </summary>
    public class PiShockShareCodeOption : IPunishmentTextInput
    {
        /// <summary>
        /// The ctor creating a <see cref="PiShockShareCodeOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PiShockShareCodeOption(SpriteFont font) : base(font, DeviceManager.PiShockPreferences.ShareCode, 280, 75, true)
        {
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Share Code: ";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void OnTextChange(string newValue)
        {
            DeviceManager.PiShockPreferences.ShareCode = newValue;
        }

        /// <inheritdoc/>
        protected override bool IsAllowedCharacter(char c)
        {
            return char.IsLetterOrDigit(c);
        }
    }
}
