using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the max reward distance
    /// </summary>
    public class MaxRewardDistanceOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MaxRewardDistanceOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MaxRewardDistanceOption(SpriteFont font) : base(font, 0.0f, 300.0f, 0.0f, 1.0f, stepAccelerationAmount: 1.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MaxRewardDistance);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Max Reward Distance:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MaxRewardDistance = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MaxRewardDistance < JumpKingPunishment.PunishmentPreferences.MinRewardDistance)
            {
                JumpKingPunishment.PunishmentPreferences.MaxRewardDistance = JumpKingPunishment.PunishmentPreferences.MinRewardDistance;
                SetValue(JumpKingPunishment.PunishmentPreferences.MaxRewardDistance);
            }
        }
    }
}
