using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the min punishment intensity
    /// </summary>
    public class MinPunishmentIntensityOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MinPunishmentIntensityOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MinPunishmentIntensityOption(SpriteFont font) : base(font, 0.0f, 100.0f, 0.0f, 1.0f, stepAccelerationAmount: 1.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Min Punishment Intensity:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity > JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity)
            {
                JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity = JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity;
                SetValue(JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity);
            }
        }
    }
}
