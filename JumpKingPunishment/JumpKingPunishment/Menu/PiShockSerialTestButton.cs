using JumpKingPunishment.Devices;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu
{
    /// <summary>
    /// An option implementation for creating a test button for the PiShock Serial
    /// </summary>
    public class PiShockSerialTestButton : IPunishmentPushButton
    {
        private IPunishmentDevice device;

        /// <summary>
        /// The ctor creating a <see cref="PiShockSerialTestButton"/>
        /// </summary>
        /// <param name="font">What <see cref="SpriteFont"/> this option should render with</param>
        public PiShockSerialTestButton(SpriteFont font) : base(font)
        {
        }

        ~PiShockSerialTestButton()
        {
            device?.Dispose();
        }

        /// <inheritdoc/>
        protected override bool CanExecute()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void OnExecute()
        {
            // Not the greatest way to do this but it's fine- the dispose/destructor logic is due to some Test implementations
            // taking time/being async, so if we immediately dispose of the device it would interrupt it so it can never trigger.
            device?.Dispose();
            device = DeviceManager.CreateDevice(EFeedbackDevice.PiShockSerial);
            device?.Test(50.0f, 1.0f);
        }

        /// <inheritdoc/>
        protected override string GetDisplayName()
        {
            return "Test";
        }
    }
}
