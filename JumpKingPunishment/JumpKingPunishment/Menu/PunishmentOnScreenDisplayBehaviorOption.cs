using JumpKingPunishment.Menu.Actions;
using JumpKingPunishment.Preferences;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the on screen display behavior for feedback
    /// </summary>
    public class PunishmentOnScreenDisplayBehaviorOption : IEnumSizedMenuOption
    {
        /// <summary>
        /// The ctor creating a <see cref="PunishmentOnScreenDisplayBehaviorOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PunishmentOnScreenDisplayBehaviorOption(SpriteFont font) : base(0, EdgeMode.Wrap, font)
        {
            CurrentOption = (int)JumpKingPunishment.PunishmentPreferences.OnScreenDisplayBehavior;
        }

        /// <inheritdoc/>
        public override string GetOptionDisplayText(int option)
        {
            return "On Screen Display Behavior: " + ((EPunishmentOnScreenDisplayBehavior)option).ToString();
        }

        /// <inheritdoc/>
        public override int GetNumOptions()
        {
            return Enum.GetValues(typeof(EPunishmentOnScreenDisplayBehavior)).Length;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }

        /// <inheritdoc/>
        protected override void OnOptionChange(int option)
        {
            JumpKingPunishment.PunishmentPreferences.OnScreenDisplayBehavior = (EPunishmentOnScreenDisplayBehavior)option;
        }
    }
}
