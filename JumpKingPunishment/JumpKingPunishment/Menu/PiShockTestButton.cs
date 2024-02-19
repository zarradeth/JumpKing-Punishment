using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for creating a test button for the PiShock
    /// </summary>
    public class PiShockTestButton : IPunishmentTestButton
    {
        /// <summary>
        /// The ctor creating a <see cref="PiShockTestButton"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PiShockTestButton(SpriteFont font) : base(font)
        {
        }

        /// <inheritdoc/>
        protected override bool CanExecute()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void OnExecute()
        {
            // Not the greatest way to do this but it's fine
            IPunishmentDevice device = DeviceManager.CreateDevice(EFeedbackDevice.PiShock);
            device?.Test(50.0f, 1.0f);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Test";
        }
    }
}
