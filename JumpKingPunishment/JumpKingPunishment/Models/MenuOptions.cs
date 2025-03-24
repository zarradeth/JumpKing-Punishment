using BehaviorTree;
using HarmonyLib;
using JumpKing;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;
using JumpKingPunishment.Menu;
using JumpKingPunishment.Menu.Actions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace JumpKingPunishment.Models
{
    /// <summary>
    /// Contains functionality for building the options menus for our mod
    /// </summary>
    public static class MenuOptions
    {
        /// <summary>
        /// The GuiFormat used for the menu selector for the main mod options
        /// </summary>
        private static readonly GuiFormat PunishmentOptionsGuiFormat = new GuiFormat
        {
            anchor_bounds = new Rectangle(0, 0, 480, 360),
            anchor = new Vector2(0, 0),
            margin = new GuiSpacing
            {
                top = 4,
                left = 8,
                right = 0,
                bottom = 0
            },
            element_margin = 0,
            padding = new GuiSpacing
            {
                top = 8,
                left = 16,
                right = 16,
                bottom = 16
            }
        };

        /// <summary>
        /// The GuiFormat used for the menu selected for selecting a device's options
        /// </summary>
        private static readonly GuiFormat DeviceOptionsGuiFormat = new GuiFormat
        {
            anchor_bounds = new Rectangle(0, 0, 480, 360),
            anchor = new Vector2(0, 0),
            margin = new GuiSpacing
            {
                top = 4,
                left = 8,
                right = 0,
                bottom = 0
            },
            element_margin = 0,
            padding = new GuiSpacing
            {
                top = 8,
                left = 16,
                right = 16,
                bottom = 16
            }
        };

        /// <summary>
        /// The GuiFormat used for the menu selected for specific device options
        /// </summary>
        private static readonly GuiFormat SubDeviceOptionsGuiFormat = new GuiFormat
        {
            anchor_bounds = new Rectangle(40, 0, 480, 360),
            anchor = new Vector2(0, 0),
            margin = new GuiSpacing
            {
                top = 4,
                left = 8,
                right = 0,
                bottom = 0
            },
            element_margin = 0,
            padding = new GuiSpacing
            {
                top = 8,
                left = 16,
                right = 16,
                bottom = 16
            }
        };

        /// <summary>
        /// Creates the punishment options menu
        /// </summary>
        public static MenuSelector CreatePunishmentOptions()
        {
            // We have too many options to just use a standard nested options menu, so we gotta roll our own-
            // basing this off the logic for the controls screen
            var font = Game1.instance.contentManager.font.MenuFontSmall;
            MenuSelector menuSelector = new MenuSelector(PunishmentOptionsGuiFormat);

            // General Options
            menuSelector.AddChild<ModEnabledOption>(new ModEnabledOption(font));
            //menuSelector.AddChild<PunishmentSpacer>(new PunishmentSpacer(new Point(250, 1), Color.Gray));

            menuSelector.AddChild<FeedbackDeviceOption>(new FeedbackDeviceOption(font));
            // I'm not gonna bother to jump through the hoops to do a rebind setting for the test key
            //  Also I'm out of screen space without doing something like pages or nested menus
            menuSelector.AddChild<PunishmentOnScreenDisplayBehaviorOption>(new PunishmentOnScreenDisplayBehaviorOption(font));
            menuSelector.AddChild<RoundDurationsOption>(new RoundDurationsOption(font));
            //menuSelector.AddChild<PunishmentSpacer>(new PunishmentSpacer(new Point(250, 1), Color.Gray));

            // Punishment Options
            menuSelector.AddChild<PunishmentEnabledOption>(new PunishmentEnabledOption(font));
            menuSelector.AddChild<MinPunishmentDurationOption>(new MinPunishmentDurationOption(font));
            menuSelector.AddChild<MaxPunishmentDurationOption>(new MaxPunishmentDurationOption(font));
            menuSelector.AddChild<MinPunishmentIntensityOption>(new MinPunishmentIntensityOption(font));
            menuSelector.AddChild<MaxPunishmentIntensityOption>(new MaxPunishmentIntensityOption(font));
            menuSelector.AddChild<MinFallDistanceOption>(new MinFallDistanceOption(font));
            menuSelector.AddChild<MaxFallDistanceOption>(new MaxFallDistanceOption(font));
            menuSelector.AddChild<PunishmentEasyModeOption>(new PunishmentEasyModeOption(font));
            //menuSelector.AddChild<PunishmentSpacer>(new PunishmentSpacer(new Point(250, 1), Color.Gray));

            // Reward Options
            menuSelector.AddChild<RewardsEnabledOption>(new RewardsEnabledOption(font));
            menuSelector.AddChild<MinRewardDurationOption>(new MinRewardDurationOption(font));
            menuSelector.AddChild<MaxRewardDurationOption>(new MaxRewardDurationOption(font));
            menuSelector.AddChild<MinRewardIntensityOption>(new MinRewardIntensityOption(font));
            menuSelector.AddChild<MaxRewardIntensityOption>(new MaxRewardIntensityOption(font));
            menuSelector.AddChild<MinRewardDistanceOption>(new MinRewardDistanceOption(font));
            menuSelector.AddChild<MaxRewardDistanceOption>(new MaxRewardDistanceOption(font));
            menuSelector.AddChild<RewardProgressOnlyOption>(new RewardProgressOnlyOption(font));

            menuSelector.Initialize(true);

            return menuSelector;
        }

        /// <summary>
        /// Creates the device options menu
        /// </summary>
        public static MenuSelector CreateDeviceOptions()
        {
            MenuSelector menuSelector = new MenuSelector(DeviceOptionsGuiFormat);

            // PiShock
            menuSelector.AddChild<TextButton>(new TextButton("PiShock", MenuOptions.CreatePiShockOptions()));
            menuSelector.Initialize(true);

            return menuSelector;
        }

        /// <summary>
        /// Creates the PiShock options menu
        /// </summary>
        public static MenuSelector CreatePiShockOptions()
        {
            // We aren't using standard menu nesting, again, but this time it's because we need text input
            var font = Game1.instance.contentManager.font.MenuFontSmall;
            PunishmentFocusCompatableMenuSelector menuSelector = new PunishmentFocusCompatableMenuSelector(SubDeviceOptionsGuiFormat);

            menuSelector.AddChild<PiShockUsernameOption>(new PiShockUsernameOption(font));
            menuSelector.AddChild<PiShockAPIKeyOption>(new PiShockAPIKeyOption(font));
            menuSelector.AddChild<PiShockShareCodeOption>(new PiShockShareCodeOption(font));
            menuSelector.AddChild<PiShockTestButton>(new PiShockTestButton(font));

            menuSelector.Initialize(true);

            return menuSelector;
        }
    }
}