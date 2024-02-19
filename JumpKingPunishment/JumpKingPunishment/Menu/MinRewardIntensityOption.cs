using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the min reward intensity
    /// </summary>
    public class MinRewardIntensityOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MinRewardIntensityOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MinRewardIntensityOption(SpriteFont font) : base(font, 0.0f, 100.0f, 0.0f, 1.0f, stepAccelerationAmount: 1.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MinRewardIntensity);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Min Reward Intensity:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MinRewardIntensity = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MinRewardIntensity > JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity)
            {
                JumpKingPunishment.PunishmentPreferences.MinRewardIntensity = JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity;
                SetValue(JumpKingPunishment.PunishmentPreferences.MinRewardIntensity);
            }
        }
    }
}
