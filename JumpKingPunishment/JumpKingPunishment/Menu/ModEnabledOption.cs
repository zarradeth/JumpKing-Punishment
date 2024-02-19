using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for enabling/disabling the mod logic as a whole
    /// </summary>
    public class ModEnabledOption : IPunishmentTextToggle
    {
        /// <summary>
        /// The ctor creating a <see cref="ModEnabledOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public ModEnabledOption(SpriteFont font) : base(font, JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled, new Point(250, 1), Color.Gray)
        {
        }

        /// <inheritdoc/>
        protected override string GetName()
        {
            return "Mod Enabled";
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled = !JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
        }
    }
}
