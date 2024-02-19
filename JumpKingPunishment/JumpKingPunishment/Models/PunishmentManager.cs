using EntityComponent;
using HarmonyLib;
using JumpKing;
using JumpKing.GameManager;
using JumpKing.Player;
using JumpKing.Util;
using JumpKingPunishment.Devices;
using JumpKingPunishment.Preferences;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection;

namespace JumpKingPunishment.Models
{
    /// <summary>
    /// Contains functionality for patching JumpKing and the main logic for the punishment mod
    /// </summary>
    public static class PunishmentManager
    {
        private static MethodInfo getPlayerValuesJumpMethod;
        private static MethodInfo getPlayerValuesMaxFallMethod;

        private static IPunishmentDevice feedbackDevice;

        private static bool wasEnabled;
        private static bool isLevelRunning;

        private static bool isInAir;
        private static float lastGroundedY;
        private static float highestGroundY;
        private static float lastPlayerY;
        private static float teleportCompensation;
        private static float feedbackPauseTimer;

        private static float preTeleportedPlayerY;

        private static readonly float LastActionDisplayTime = 3.0f;
        private static readonly float TeleportPauseTime = 0.1f;
        private static readonly float TeleportDetectionMaxExpectedVelocityMultipler = 1.25f;

        private static string incomingPunishmentText;
        private static float lastActionDrawTimer;
        private static string lastActionText;
        private static Color lastActionDrawColor;

        private static bool wasTestHeld;

        struct FeedbackResults
        {
            public bool trigger;
            public float fraction;
            public float intensity;
            public float duration;
        }

        /// <summary>
        /// Initializes defaults for the class and applies patching via Harmony for menu functionality
        /// Called by <see cref="JumpKingPunishment.Setup"/>
        /// </summary>
        /// <param name="harmony">The harmony instance to useg</param>
        public static void Initialize(Harmony harmony)
        {
            var jumpKingUpdateMethod = typeof(JumpGame).GetMethod("Update");
            var managerUpdateMethod = typeof(PunishmentManager).GetMethod("Update");
            harmony.Patch(jumpKingUpdateMethod, postfix: new HarmonyMethod(managerUpdateMethod));

            var jumpKingDrawMethod = typeof(JumpGame).GetMethod("Draw");
            var managerDrawMethod = typeof(PunishmentManager).GetMethod("Draw");
            harmony.Patch(jumpKingDrawMethod, postfix: new HarmonyMethod(managerDrawMethod));

            var jumpKingTeleportPlayerMethod = typeof(JumpKing.BodyCompBehaviours.HandlePlayerTeleportBehaviour).GetMethod("ExecuteBehaviour");
            var managerPreHandleTeleportMethod = typeof(PunishmentManager).GetMethod("PreHandleTeleport");
            var managerPostHandleTeleportMethod = typeof(PunishmentManager).GetMethod("PostHandleTeleport");
            harmony.Patch(jumpKingTeleportPlayerMethod, prefix: new HarmonyMethod(managerPreHandleTeleportMethod), postfix: new HarmonyMethod(managerPostHandleTeleportMethod));

            // PlayerValues is internal so we can't directly call them, hence still using AccessTools, not sure if this
            // was always the case or it changed in the workshop update but whatever- JUMP is technically accessible through
            // the JumpState but MAX_FALL isn't exposed directly by anything
            getPlayerValuesJumpMethod = AccessTools.Method("JumpKing.PlayerValues:get_JUMP");
            getPlayerValuesMaxFallMethod = AccessTools.Method("JumpKing.PlayerValues:get_MAX_FALL");

            wasEnabled = JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
            isLevelRunning = false;

            ResetState();

            preTeleportedPlayerY = float.NaN;

            incomingPunishmentText = "";
            lastActionDrawTimer = 0.0f;
            lastActionText = "";
            lastActionDrawColor = Color.White;

            wasTestHeld = false;

            // Call UpdateFeedbackDevice now to try to create the device ASAP
            UpdateFeedbackDevice();
        }

        /// <summary>
        /// Called by <see cref="JumpKingPunishment.OnLevelStart"/> to reset state and let us know a level is running
        /// </summary>
        public static void OnLevelStart()
        {
            ResetState();
            isLevelRunning = true;
        }

        /// <summary>
        /// Called by <see cref="JumpKingPunishment.OnLevelEnd"/> to reset state and let us know a level is no longer running
        /// </summary>
        public static void OnLevelEnd()
        {
            ResetState();
            isLevelRunning = false;
        }

        /// <summary>
        /// Called each frame after <see cref="JumpGame.Update"/> and used as the main update for our mod
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            float p_delta = 0.016666668f;
            if (!UpdateEnabled())
            {
                return;
            }

            bool isPaused = IsGamePaused();
            if (!isPaused)
            {
                feedbackPauseTimer = Math.Max(0.0f, feedbackPauseTimer - p_delta);
            }

            UpdateFeedbackDevice(p_delta);

            UpdateInput();

            PlayerEntity player = EntityManager.instance.Find<PlayerEntity>();
            BodyComp bodyComponent = player?.GetComponent<BodyComp>();
            if (IsFeedbackEnabled() && !isPaused)
            {
                if (!UpdateTeleportDetection(bodyComponent))
                {
                    // We only need to do normal feedback updates if we didn't detect a teleport as teleports reset state
                    CheckAndTriggerFeedback(bodyComponent);
                }
            }

            UpdateOnScreenText(bodyComponent, p_delta);
        }

        /// <summary>
        /// Called by <see cref="PunishmentManagerEntity.Update"/>
        /// Handles checking if the mod is enabled and the enabled state changing
        /// </summary>
        /// <returns>Returns a bool for whether or not the mod is enabled</returns>
        private static bool UpdateEnabled()
        {
            bool isEnabled = JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled;
            if (wasEnabled != isEnabled)
            {
                // When toggling the enabled state for the mod reset state
                ResetState();
            }
            wasEnabled = isEnabled;

            // We want to still run our updates if a level isn't loaded, we just won't update feedback/do feedback
            return isEnabled;
        }

        /// <summary>
        /// Called by <see cref="PunishmentManagerEntity.Update"/>
        /// Handles instantiating (especially if the device we should use changes) and updating the feedback device
        /// </summary>
        /// <param name="delta">A delta time since the last update, if one is not provided we will not call <see cref="IPunishmentDevice.Update"/> on the device</param>
        private static void UpdateFeedbackDevice(float delta = float.NaN)
        {
            // Check if we need to make a new feedback device due to settings changes
            bool bNeedNewDevice = false;
            if (feedbackDevice == null)
            {
                bNeedNewDevice = (JumpKingPunishment.PunishmentPreferences.FeedbackDevice != EFeedbackDevice.None);
            }
            else
            {
                bNeedNewDevice = (feedbackDevice.GetDeviceType() != JumpKingPunishment.PunishmentPreferences.FeedbackDevice);
            }

            if (bNeedNewDevice)
            {
                feedbackDevice?.Dispose();
                feedbackDevice = DeviceManager.CreateDevice(JumpKingPunishment.PunishmentPreferences.FeedbackDevice);
                // Trigger a test immediately to help the user know the device is working when we create it
                feedbackDevice?.Test(50.0f, 1.0f);
            }

            if (!float.IsNaN(delta))
            {
                feedbackDevice?.Update(delta);
            }
        }

        /// <summary>
        /// Called by <see cref="PunishmentManagerEntity.Update"/>
        /// Handles updating debug/keyboard input
        /// </summary>
        private static void UpdateInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            bool testHeld = keyboardState.IsKeyDown(JumpKingPunishment.PunishmentPreferences.PunishmentTestFeedbackDebugKey);
            if (testHeld && !wasTestHeld)
            {
                feedbackDevice?.Test(50.0f, 1.0f);
            }
            wasTestHeld = testHeld;
        }

        /// <summary>
        /// Called by <see cref="PunishmentManagerEntity.Update"/>
        /// Handles updating teleport detection and handling feedback when a teleport is detected
        /// </summary>
        private static bool UpdateTeleportDetection(BodyComp bodyComponent)
        {
            if (bodyComponent == null)
            {
                return false;
            }

            float yLocation = bodyComponent.Position.Y + teleportCompensation;
            bool teleportDetected = false;
            if (!float.IsNaN(lastPlayerY))
            {
                // We don't need to divide out delta here as Jump King doesn't apply velocities based on delta time (the current velocity
                // is just added to the player position each frame- entities in Jump King update with a fixed time step).
                // MAX_FALL and JUMP from PlayerValues are the same- they are fixed numbers that are never multiplied with DT when they
                // influence the player's velocity.
                float lastYVelocity = (yLocation - lastPlayerY);

                // Values to check against to see if the player has exceeded what should be the max possible velocity in a single frame.
                // TeleportDetectionMaxExpectedVelocityMultipler exists to help prevent false positives as well as potentially allow
                // some wiggle room (the value may need to be adjusted) if mods ever do weird things with launching the player or something
                float maxExpectedNegativeVelocity = GetPlayerValuesJUMP() * TeleportDetectionMaxExpectedVelocityMultipler;
                float maxExpectedPositiveVelocity = GetPlayerValuesMAX_FALL() * TeleportDetectionMaxExpectedVelocityMultipler;

                teleportDetected = (lastYVelocity > 0.0f) ? (lastYVelocity > maxExpectedPositiveVelocity) : (lastYVelocity < maxExpectedNegativeVelocity);
            }

            // If we detected a teleport trigger a punishment from before the teleport (if needed) and then reset state
            // like we do for OnPlayerSaveStateApplying
            if (teleportDetected)
            {
                // Trigger feedback (so they can't get out of it) from the point they were at before the teleport
                CheckAndTriggerFeedback(bodyComponent, lastPlayerY, true);
                // Reset state since we don't want to carry any positioning/movement/feedback/etc through the teleport
                // Pause for a bit as depending on how the teleport was done the player's state might not be valid for a bit
                ResetState(TeleportPauseTime);
                // No need to update lastPlayerY as it was cleared in ResetState and will be updated again on future ticks
            }
            else
            {
                lastPlayerY = yLocation;
            }

            return teleportDetected;
        }

        /// <summary>
        /// Called by <see cref="PunishmentManagerEntity.Update"/>
        /// Updates on screen text for punishment actions and incoming punishment
        /// </summary>
        private static void UpdateOnScreenText(BodyComp bodyComponent, float delta)
        {
            // Fade the last action text out overtime (over the second half of it's lifetime)
            lastActionDrawTimer = Math.Max(0.0f, lastActionDrawTimer - delta);
            float Alpha = 1.0f;
            if (lastActionDrawTimer < (LastActionDisplayTime / 2.0f))
            {
                Alpha = lastActionDrawTimer / (LastActionDisplayTime / 2.0f);
            }
            lastActionDrawColor = new Color(lastActionDrawColor, Alpha);

            // Update the incoming text entity
            // Only show punishments incoming as rewards will be weird with the arcs of a jump (and punishments
            // generally can't be avoided once they start)
            FeedbackResults incomingPunishment = new FeedbackResults();
            if (IsFeedbackEnabled() && (bodyComponent != null) && isInAir && !float.IsNaN(lastGroundedY))
            {
                float currentYDelta = (bodyComponent.Position.Y + teleportCompensation) - lastGroundedY;
                if (currentYDelta > 0.0f)
                {
                    incomingPunishment = CalculatePunishment(currentYDelta);
                }
            }

            if (!incomingPunishment.trigger)
            {
                incomingPunishmentText = "";
            }
            else
            {
                incomingPunishmentText = GenerateFeedbackInfoString("Incoming punishment", incomingPunishment, "...");
            }
        }

        /// <summary>
        /// Checks and updates state to see if feedback should be triggered, and triggers it if needed
        /// Called by <see cref="PunishmentManagerEntity.Update"/>
        /// Also called manually when teleports are detected/handled to immediately trigger feedback
        /// </summary>
        private static void CheckAndTriggerFeedback(BodyComp bodyComponent, float overridePlayerY = float.NaN, bool forcePunishment = false)
        {
            if (bodyComponent == null)
            {
                return;
            }

            // Consider sand ground too for our purposes
            bool isOnGround = bodyComponent.IsOnGround || bodyComponent.IsOnBlock<JumpKing.Level.SandBlock>();
            if (isOnGround || forcePunishment)
            {
                float yLocation = !float.IsNaN(overridePlayerY) ? overridePlayerY : (bodyComponent.Position.Y + teleportCompensation);
                if (isInAir && !float.IsNaN(lastGroundedY))
                {
                    // We have landed, calculate and execute a shock/reward
                    // Note, Y DECREASES as you move upward
                    float yDelta = yLocation - lastGroundedY;

                    // Positive progress- we only want to trigger positive progress if you are actually on the ground
                    // as forced rewards should only really be possible if the player is mid jump when something happens
                    // and triggering a reward would be incorrect/potentially problematic in that case
                    if ((yDelta < 0.0f) && isOnGround && !float.IsNaN(highestGroundY))
                    {
                        if (JumpKingPunishment.PunishmentPreferences.RewardProgressOnlyMode)
                        {
                            yDelta = yLocation - highestGroundY;
                        }

                        // Make sure we actually made progress in the case of progress only rewards
                        if (yDelta < 0.0f)
                        {
                            var reward = CalculateReward(yDelta);
                            if (reward.trigger)
                            {
                                feedbackDevice?.Reward(reward.intensity, reward.duration);
                                UpdateLastAction(GenerateFeedbackInfoString("Reward!", reward), Color.Lime);
                            }
                        }
                    }
                    else if (yDelta > 0.0f)  // Negative progress
                    {
                        var punishment = CalculatePunishment(yDelta);
                        if (punishment.trigger)
                        {
                            feedbackDevice?.Punish(punishment.intensity, punishment.duration, JumpKingPunishment.PunishmentPreferences.PunishmentEasyMode);
                            UpdateLastAction(GenerateFeedbackInfoString("Punishment!", punishment), JumpKingPunishment.PunishmentPreferences.PunishmentEasyMode ? Color.Lime : Color.Red);
                        }
                    }
                }
                // This can short the player rewards if they are making positive progress when we try to
                // force a punishment, but that generally shouldn't happen and isn't a huge deal if it does
                lastGroundedY = yLocation;
                if (isOnGround)     // Don't update highest grounded if they aren't actually on the ground
                {
                    highestGroundY = float.IsNaN(highestGroundY) ? yLocation : Math.Min(yLocation, highestGroundY);
                }
            }
            isInAir = !isOnGround;
        }

        /// <summary>
        /// Called each frame after <see cref="JumpGame.Draw"/> and used to draw text on screen for the mod
        /// </summary>
        public static void Draw()
        {
            if (!JumpKingPunishment.PunishmentPreferences.PunishmentModEnabled)
            {
                return;
            }

            var styleFont = Game1.instance.contentManager.font.StyleFont;
            if (styleFont == null)
            {
                return;
            }

            // Incoming punishment
            if (!string.IsNullOrEmpty(incomingPunishmentText))
            {
                Vector2 textSize = styleFont.MeasureString(incomingPunishmentText);
                Vector2 drawLocation = new Vector2(240.0f, 16.0f - (textSize.Y * 1.5f));
                Color drawColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                TextHelper.DrawString(styleFont, incomingPunishmentText, drawLocation, drawColor, new Vector2(0.5f, -0.5f));
            }

            // Last action text
            if (!string.IsNullOrEmpty(lastActionText) && (lastActionDrawColor.A > 0.0f))
            {
                Vector2 textSize = styleFont.MeasureString(lastActionText);
                Vector2 drawLocation = new Vector2(240.0f, 360.0f - (textSize.Y * 1.5f));

                TextHelper.DrawString(styleFont, lastActionText, drawLocation, lastActionDrawColor, new Vector2(0.5f, -0.5f));
            }
        }

        /// <summary>
        /// Called before <see cref="JumpKing.BodyCompBehaviours.HandlePlayerTeleportBehaviour.ExecuteBehavior"/> and helps detect and handle expected
        /// teleports during gameplay from stuff like TeleportLinks
        /// </summary>
        public static void PreHandleTeleport(object __instance, JumpKing.BodyCompBehaviours.BehaviourContext behaviourContext)
        {
            preTeleportedPlayerY = behaviourContext.BodyComp.Position.Y;
        }

        /// <summary>
        /// Called after <see cref="JumpKing.BodyCompBehaviours.HandlePlayerTeleportBehaviour.ExecuteBehavior"/> and helps detect and handle expected
        /// teleports during gameplay from stuff like TeleportLinks
        /// </summary>
        public static void PostHandleTeleport(object __instance, JumpKing.BodyCompBehaviours.BehaviourContext behaviourContext)
        {
            float postTeleportedPlayerY = behaviourContext.BodyComp.Position.Y;
            if (preTeleportedPlayerY != postTeleportedPlayerY)
            {
                teleportCompensation += (preTeleportedPlayerY - postTeleportedPlayerY);
            }
            preTeleportedPlayerY = float.NaN;
        }

        /// <summary>
        /// Checks if the game is currently paused
        /// </summary>
        private static bool IsGamePaused()
        {
            return Traverse.Create(GameLoop.instance).Field("m_pause_manager").Property("IsPaused", null).GetValue<bool>();
        }

        /// <summary>
        /// Returns whether feedback is enabled and should generate or not
        /// </summary>
        private static bool IsFeedbackEnabled()
        {
            return isLevelRunning && (feedbackPauseTimer <= 0.0f);
        }

        /// <summary>
        /// Calculates a Reward given the provided delta and considering the player settings
        /// </summary>
        /// <param name="yDelta">The difference in the player's Y location we using for the calculation</param>
        /// <returns>A <see cref="FeedbackResults"/> struct containing the calculated reward</returns>
        private static FeedbackResults CalculateReward(float yDelta)
        {
            FeedbackResults results;
            results.trigger = false;
            results.fraction = 0.0f;
            results.intensity = 0.0f;
            results.duration = 0.0f;

            if (JumpKingPunishment.PunishmentPreferences.EnabledRewards)
            {
                float rewardDistance = Math.Abs(yDelta);
                if (rewardDistance >= JumpKingPunishment.PunishmentPreferences.MinRewardDistance)
                {
                    results.trigger = true;
                    results.fraction = 0.0f;

                    float rewardDistanceDiff = JumpKingPunishment.PunishmentPreferences.MaxRewardDistance - JumpKingPunishment.PunishmentPreferences.MinRewardDistance;
                    if (rewardDistanceDiff > 0.0f)
                    {
                        results.fraction = (rewardDistance - JumpKingPunishment.PunishmentPreferences.MinRewardDistance) / rewardDistanceDiff;
                        results.fraction = Math.Min(results.fraction, 1.0f);
                    }

                    results.intensity = JumpKingPunishment.PunishmentPreferences.MinRewardIntensity + ((JumpKingPunishment.PunishmentPreferences.MaxRewardIntensity - JumpKingPunishment.PunishmentPreferences.MinRewardIntensity) * results.fraction);
                    results.duration = JumpKingPunishment.PunishmentPreferences.MinRewardDuration + ((JumpKingPunishment.PunishmentPreferences.MaxRewardDuration - JumpKingPunishment.PunishmentPreferences.MinRewardDuration) * results.fraction);

                    if (JumpKingPunishment.PunishmentPreferences.RoundDurations)
                    {
                        results.duration = (float)Math.Round(results.duration);
                    }

                    // If we didn't calculate a positive intensity or duration we aren't actually receiving a reward
                    if ((results.duration <= 0.0f) || (results.intensity <= 0.0f))
                    {
                        results.trigger = false;
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Calculates a Punishment given the provided delta and considering the player settings
        /// </summary>
        /// <param name="yDelta">The difference in the player's Y location we using for the calculation</param>
        /// <returns>A <see cref="FeedbackResults"/> struct containing the calculated punishment</returns>
        private static FeedbackResults CalculatePunishment(float yDelta)
        {
            FeedbackResults results;
            results.trigger = false;
            results.fraction = 0.0f;
            results.intensity = 0.0f;
            results.duration = 0.0f;

            if (JumpKingPunishment.PunishmentPreferences.EnabledPunishment)
            {
                float punishmentDistance = Math.Abs(yDelta);
                if (punishmentDistance >= JumpKingPunishment.PunishmentPreferences.MinFallDistance)
                {
                    results.trigger = true;
                    results.fraction = 0.0f;

                    float punishmentDistanceDiff = JumpKingPunishment.PunishmentPreferences.MaxFallDistance - JumpKingPunishment.PunishmentPreferences.MinFallDistance;
                    if (punishmentDistanceDiff > 0.0f)
                    {
                        results.fraction = (punishmentDistance - JumpKingPunishment.PunishmentPreferences.MinFallDistance) / punishmentDistanceDiff;
                        results.fraction = Math.Min(results.fraction, 1.0f);
                    }

                    results.intensity = JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity + ((JumpKingPunishment.PunishmentPreferences.MaxPunishmentIntensity - JumpKingPunishment.PunishmentPreferences.MinPunishmentIntensity) * results.fraction);
                    results.duration = JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration + ((JumpKingPunishment.PunishmentPreferences.MaxPunishmentDuration - JumpKingPunishment.PunishmentPreferences.MinPunishmentDuration) * results.fraction);

                    if (JumpKingPunishment.PunishmentPreferences.RoundDurations)
                    {
                        results.duration = (float)Math.Round(results.duration);
                    }

                    // If we didn't calculate a positive intensity or duration we aren't actually receiving a punishment
                    if ((results.duration <= 0.0f) || (results.intensity <= 0.0f))
                    {
                        results.trigger = false;
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Resets falling/grounded/teleport/etc. state and allows pausing future state updates for a provided amount of time.
        /// </summary>
        /// <param name="pauseTime">The amount of time in seconds feedback shouldn't be able to trigger for</param>
        private static void ResetState(float pauseTime = 0.0f)
        {
            isInAir = false;
            lastGroundedY = float.NaN;
            highestGroundY = float.NaN;
            lastPlayerY = float.NaN;
            teleportCompensation = 0.0f;

            feedbackPauseTimer = pauseTime;
        }

        /// <summary>
        /// Generations a string for display on screen feedback information, taking the current on screen display behavior into account
        /// </summary>
        /// <param name="baseString">The base text for the generated info string</param>
        /// <param name="feedback">The feedback results we are generating the string for</param>
        /// <param name="postFix">Any text that should be appended to teh end of the string</param>
        /// <returns>The formatted feedback info string</returns>
        private static string GenerateFeedbackInfoString(string baseString, FeedbackResults feedback, string postFix = "")
        {
            // When displaying values do some rounding to keep the string length sane
            switch (JumpKingPunishment.PunishmentPreferences.OnScreenDisplayBehavior)
            {
                case EPunishmentOnScreenDisplayBehavior.FeedbackIntensityAndDuration:
                    return $"{baseString} ({Math.Round(feedback.intensity)}% x {Math.Round(feedback.duration, 2)}s){postFix}";
                case EPunishmentOnScreenDisplayBehavior.DistanceBasedPercentage:
                    return $"{baseString} ({Math.Round(feedback.fraction * 100.0f)}%){postFix}";
                case EPunishmentOnScreenDisplayBehavior.MessageOnly:
                    return baseString + postFix;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Updates the last action text and restarts its display timer
        /// </summary>
        /// <param name="actionText">The text to display as the last action</param>
        /// <param name="textColor">The color the the text should be rendered with</param>
        private static void UpdateLastAction(string actionText, Color textColor)
        {
            lastActionDrawTimer = LastActionDisplayTime;
            lastActionText = actionText;
            lastActionDrawColor = textColor;
        }

        /// <summary>
        /// Returns <see cref="JumpKing.PlayerValues.JUMP"/>
        /// </summary>
        private static float GetPlayerValuesJUMP()
        {
            return (float)getPlayerValuesJumpMethod.Invoke(null, null);
        }

        /// <summary>
        /// Returns <see cref="JumpKing.PlayerValues.MAX_FALL"/>
        /// </summary>
        private static float GetPlayerValuesMAX_FALL()
        {
            return (float)getPlayerValuesMaxFallMethod.Invoke(null, null);
        }
    }
}
