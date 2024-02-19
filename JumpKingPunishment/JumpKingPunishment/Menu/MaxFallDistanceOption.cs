using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for setting the max fall distance
    /// </summary>
    public class MaxFallDistanceOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MaxFallDistanceOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MaxFallDistanceOption(SpriteFont font) : base(font, 0.0f, 5000.0f, 0.0f, 1.0f, stepAccelerationAmount: 3.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MaxFallDistance);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Max Fall Distance:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment;
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MaxFallDistance = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MaxFallDistance < JumpKingPunishment.PunishmentPreferences.MinFallDistance)
            {
                JumpKingPunishment.PunishmentPreferences.MaxFallDistance = JumpKingPunishment.PunishmentPreferences.MinFallDistance;
                SetValue(JumpKingPunishment.PunishmentPreferences.MaxFallDistance);
            }
        }
    }
}
