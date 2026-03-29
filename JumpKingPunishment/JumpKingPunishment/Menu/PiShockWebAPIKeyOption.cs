using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for setting the API Key for the PiShock when using web requests
    /// </summary>
    public class PiShockWebAPIKeyOption : IPunishmentTextInput
    {
        /// <summary>
        /// The ctor creating a <see cref="PiShockWebAPIKeyOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PiShockWebAPIKeyOption(SpriteFont font) : base(font, DeviceManager.PiShockWebPreferences.APIKey, 280, 75, true)
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
            DeviceManager.PiShockWebPreferences.APIKey = newValue;
        }

        /// <inheritdoc/>
        protected override bool IsAllowedCharacter(char c)
        {
            return char.IsLetterOrDigit(c) || (c == '-');
        }
    }
}
