using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for enabling/disabling punishment easy mode
    /// </summary>
    public class PunishmentEasyModeOption : IPunishmentTextToggle
    {
        /// <summary>
        /// The ctor creating a <see cref="PunishmentEasyModeOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PunishmentEasyModeOption(SpriteFont font) : base(font, JumpKingPunishment.PunishmentPreferences.PunishmentEasyMode)
        {
        }

        /// <inheritdoc/>
        protected override string GetName()
        {
            return "Punishment Easy Mode";
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            JumpKingPunishment.PunishmentPreferences.PunishmentEasyMode = !JumpKingPunishment.PunishmentPreferences.PunishmentEasyMode;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled && JumpKingPunishment.PunishmentPreferences.EnabledPunishment && (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != Devices.EFeedbackDevice.None);
        }
    }
}
