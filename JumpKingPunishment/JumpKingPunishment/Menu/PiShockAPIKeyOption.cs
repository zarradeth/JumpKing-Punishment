using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for setting the API Key for the PiShock
    /// </summary>
    public class PiShockAPIKeyOption : IPunishmentTextInput
    {
        /// <summary>
        /// The ctor creating a <see cref="PiShockAPIKeyOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PiShockAPIKeyOption(SpriteFont font) : base(font, DeviceManager.PiShockPreferences.APIKey, 280, 75, true)
        {
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "API Key: ";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void OnTextChange(string newValue)
        {
            DeviceManager.PiShockPreferences.APIKey = newValue;
        }

        /// <inheritdoc/>
        protected override bool IsAllowedCharacter(char c)
        {
            return char.IsLetterOrDigit(c) || (c == '-');
        }
    }
}
