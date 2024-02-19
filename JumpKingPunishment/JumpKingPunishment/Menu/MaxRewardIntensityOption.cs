using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the max reward intensity
    /// </summary>
    public class MaxRewardIntensityOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MaxRewardIntensityOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MaxRewardIntensityOption(SpriteFont font) : base(font, 0.0f, 100.0f, 0.0f, 1.0f, stepAccelerationAmount: 1.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Max Reward Intensity:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards;
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity < JumpKingPunishment.PunishmentPreferences.MinRewardIntensity)
            {
                JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity = JumpKingPunishment.PunishmentPreferences.MinRewardIntensity;
                SetValue(JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity);
            }
        }
    }
}
