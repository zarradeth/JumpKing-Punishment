using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for enabling/disabling punishment feedback
    /// </summary>
    public class PunishmentEnabledOption : IPunishmentTextToggle
    {
        /// <summary>
        /// The ctor creating a <see cref="PunishmentEnabledOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PunishmentEnabledOption(SpriteFont font) : base(font, JumpKingPunishment.PunishmentPreferences.EnabledPunishment, new Point(250, 1), Color.Gray, true)
        {
        }

        /// <inheritdoc/>
        protected override string GetName()
        {
            return "Punishment Enabled";
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            JumpKingPunishment.PunishmentPreferences.EnabledPunishment = !JumpKingPunishment.PunishmentPreferences.EnabledPunishment;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
        }
    }
}
