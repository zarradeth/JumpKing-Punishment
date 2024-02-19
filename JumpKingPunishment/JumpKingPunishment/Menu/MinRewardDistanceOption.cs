using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the min reward distance
    /// </summary>
    public class MinRewardDistanceOption : IPunishmentSlider
    {
        /// <summary>
        /// The ctor creating a <see cref="MinRewardDistanceOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public MinRewardDistanceOption(SpriteFont font) : base(font, 0.0f, 300.0f, 0.0f, 1.0f, stepAccelerationAmount: 1.0f, wholeNumbers: true)
        {
            SetValue(JumpKingPunishment.PunishmentPreferences.MinRewardDistance);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Min Reward Distance:";
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledRewards;
        }

        /// <inheritdoc/>
        protected override void OnSliderChange(float newValue)
        {
            JumpKingPunishment.PunishmentPreferences.MinRewardDistance = newValue;
            if (JumpKingPunishment.PunishmentPreferences.MinRewardDistance > JumpKingPunishment.PunishmentPreferences.MaxRewardDistance)
            {
                JumpKingPunishment.PunishmentPreferences.MinRewardDistance = JumpKingPunishment.PunishmentPreferences.MaxRewardDistance;
                SetValue(JumpKingPunishment.PunishmentPreferences.MinRewardDistance);
            }
        }
    }
}
