using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for selecting the feedback device
    /// </summary>
    public class FeedbackDeviceOption : IEnumSizedMenuOption
    {
        /// <summary>
        /// The ctor creating a <see cref="FeedbackDeviceOption"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public FeedbackDeviceOption(SpriteFont font) : base(0, EdgeMode.Wrap, font)
        {
            CurrentOption = (int)JumpKingPunishment.PunishmentPreferences.FeedbackDevice;
        }

        /// <inheritdoc/>
        public override string GetOptionDisplayText(int option)
        {
            return "Feedback Device: " + ((EFeedbackDevice)option).ToString();
        }

        /// <inheritdoc/>
        public override int GetNumOptions()
        {
            return Enum.GetValues(typeof(EFeedbackDevice)).Length;
        }

        /// <inheritdoc/>
        protected override bool CanChange()
        {
            return JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
        }

        /// <inheritdoc/>
        protected override void OnOptionChange(int option)
        {
            JumpKingPunishment.PunishmentPreferences.FeedbackDevice = (EFeedbackDevice)option;
        }
    }
}
