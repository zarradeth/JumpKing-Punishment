using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for enabling/disabling reward feedback
    /// </summary>
    public class RewardsEnabledOption : IPunishmentTextToggle
    {
        /// <summary>
        /// The ctor creating a <see cref="RewardsEnabledOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public RewardsEnabledOption(SpriteFont font) : base(font, JumpKingPunishment.PunishmentPreferences.EnabledRewards, new Point(250, 1), Color.Gray, true)
        {
        }

        /// <inheritdoc/>
        protected override string GetName()
        {
            return "Rewards Enabled";
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            JumpKingPunishment.PunishmentPreferences.EnabledRewards = !JumpKingPunishment.PunishmentPreferences.EnabledRewards;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
        }
    }
}
