using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for enabling/disabling reward generation for new progress only
    /// </summary>
    public class RewardProgressOnlyOption : IPunishmentTextToggle
    {
        /// <summary>
        /// The ctor creating a <see cref="RewardProgressOnlyOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public RewardProgressOnlyOption(SpriteFont font) : base(font, JumpKingPunishment.PunishmentPreferences.RewardProgressOnlyMode)
        {
        }

        /// <inheritdoc/>
        protected override string GetName()
        {
            return "Reward New Progress Only";
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            JumpKingPunishment.PunishmentPreferences.RewardProgressOnlyMode = !JumpKingPunishment.PunishmentPreferences.RewardProgressOnlyMode;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards;
        }
    }
}
