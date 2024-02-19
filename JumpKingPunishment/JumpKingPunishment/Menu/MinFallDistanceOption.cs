using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the min fall distance
    /// </summary>
    public class MinFallDistanceOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MinFallDistanceOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MinFallDistanceOption(SpriteFont font) : base(font, 0.0f, 5000.0f, 0.0f, 1.0f, stepAccelerationAmount: 3.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MinFallDistance);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Min Fall Distance:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MinFallDistance = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MinFallDistance > JumpKingPunishment.PunishmentPreferences.MaxFallDistance)
            {
                JumpKingPunishment.PunishmentPreferences.MinFallDistance = JumpKingPunishment.PunishmentPreferences.MaxFallDistance;
                SetValue(JumpKingPunishment.PunishmentPreferences.MinFallDistance);
            }
        }
    }
}
