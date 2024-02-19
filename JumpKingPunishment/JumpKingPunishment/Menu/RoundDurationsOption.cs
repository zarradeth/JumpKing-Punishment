using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for enabling/disabling rounding for generated feedback durations
    /// </summary>
    public class RoundDurationsOption : IPunishmentTextToggle
    {
        /// <summary>
        /// The ctor creating a <see cref="RoundDurationsOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public RoundDurationsOption(SpriteFont font) : base(font, JumpKingPunishment.PunishmentPreferences.RoundDurations)
        {
        }

        /// <inheritdoc/>
        protected override string GetName()
        {
            return "Round Durations";
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            JumpKingPunishment.PunishmentPreferences.RoundDurations = !JumpKingPunishment.PunishmentPreferences.RoundDurations;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
        }
    }
}
