using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the min punishment duration
    /// </summary>
    public class MinPunishmentDurationOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MinPunishmentDurationOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MinPunishmentDurationOption(SpriteFont font) : base(font, 0.0f, 15.0f, 0.0f, 0.1f, stepAccelerationAmount: 0.1f, displayPrecision: 1)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Min Punishment Duration:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration > JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration)
            {
                JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration = JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration;
                SetValue(JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration);
            }
        }
    }
}
