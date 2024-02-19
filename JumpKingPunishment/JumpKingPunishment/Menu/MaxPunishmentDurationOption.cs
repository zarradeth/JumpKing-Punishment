using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the punishment max duration
    /// </summary>
    public class MaxPunishmentDurationOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MaxPunishmentDurationOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MaxPunishmentDurationOption(SpriteFont font) : base(font, 0.0f, 15.0f, 0.0f, 0.1f, stepAccelerationAmount: 0.1f, displayPrecision: 1)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Max Punishment Duration:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration < JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration)
            {
                JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration = JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration;
                SetValue(JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration);
            }
        }
    }
}
