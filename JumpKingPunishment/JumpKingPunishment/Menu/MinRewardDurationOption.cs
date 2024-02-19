using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the min reward duration
    /// </summary>
    public class MinRewardDurationOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MinRewardDurationOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MinRewardDurationOption(SpriteFont font) : base(font, 0.0f, 15.0f, 0.0f, 0.1f, stepAccelerationAmount: 0.1f, displayPrecision: 1)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MinRewardDuration);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Min Reward Duration:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MinRewardDuration = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MinRewardDuration > JumpKingPunishment.PunishmentPreferences.MaxRewardDuration)
            {
                JumpKingPunishment.PunishmentPreferences.MinRewardDuration = JumpKingPunishment.PunishmentPreferences.MaxRewardDuration;
                SetValue(JumpKingPunishment.PunishmentPreferences.MinRewardDuration);
            }
        }
    }
}
