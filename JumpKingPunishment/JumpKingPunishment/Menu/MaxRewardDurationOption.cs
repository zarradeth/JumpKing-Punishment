using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the max reward duration
    /// </summary>
    public class MaxRewardDurationOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MaxRewardDurationOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MaxRewardDurationOption(SpriteFont font) : base(font, 0.0f, 15.0f, 0.0f, 0.1f, stepAccelerationAmount: 0.1f, displayPrecision: 1)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MaxRewardDuration);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Max Reward Duration:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards;
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MaxRewardDuration = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MaxRewardDuration < JumpKingPunishment.PunishmentPreferences.MinRewardDuration)
            {
                JumpKingPunishment.PunishmentPreferences.MaxRewardDuration = JumpKingPunishment.PunishmentPreferences.MinRewardDuration;
                SetValue(JumpKingPunishment.PunishmentPreferences.MaxRewardDuration);
            }
        }
    }
}
