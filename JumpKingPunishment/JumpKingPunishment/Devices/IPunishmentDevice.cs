using System;

namespace JumpKingPunishment.Devices
{
    /// <summary>
    /// An interface representing a device that can trigger feedback to the user
    /// To add support for another device, derive from and implement this interface
    /// </summary>
    public interface IPunishmentDevice : IDisposable
    {
        /// <summary>
        /// Returns what EFeedbackDevice this device is/implements
        /// </summary>
        /// <returns>The <see cref="EFeedbackDevice"/> this implementation implements</returns>
        EFeedbackDevice GetDeviceType();

        /// <summary>
        /// Update the state of the device
        /// </summary>
        /// <param name="p_delta">The number of seconds that have passed since the last update</param>
        void Update(float p_delta);

        /// <summary>
        /// Called when the device should punish the user
        /// </summary>
        /// <param name="intensity">A 1-100 number that should control the strength of the feedback</param>
        /// <param name="duration">The number of seconds the feedback should occur for</param>
        /// <param name="easyMode">Whether 'easy mode' punishment is enabled, which should reduce or change the type of feedback</param>
        void Punish(float intensity, float duration, bool easyMode);

        /// <summary>
        /// Called when the device should reward the user
        /// </summary>
        /// <param name="intensity">A 1-100 number that should control the strength of the feedback</param>
        /// <param name="duration">The number of seconds the feedback should occur for</param>
        void Reward(float intensity, float duration);

        /// <summary>
        /// Called when we want to trigger test feedback from the device
        /// </summary>
        /// <param name="intensity">A 1-100 number that should control the strength of the feedback</param>
        /// <param name="duration">The number of seconds the feedback should occur for</param>
        void Test(float intensity, float duration);
    }
}
