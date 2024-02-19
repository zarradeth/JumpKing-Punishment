using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the max punishment intensity
    /// </summary>
    public class MaxPunishmentIntensityOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MaxPunishmentIntensityOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MaxPunishmentIntensityOption(SpriteFont font) : base(font, 0.0f, 100.0f, 0.0f, 1.0f, stepAccelerationAmount: 1.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Max Punishment Intensity:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity < JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity)
            {
                JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity = JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity;
                SetValue(JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity);
            }
        }
    }
}
