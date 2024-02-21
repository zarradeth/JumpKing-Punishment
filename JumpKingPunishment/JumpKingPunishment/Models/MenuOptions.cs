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
    /// Contains functionality for patching the options menu to add our options to it
    /// </summary>
    public static class MenuOptions
    {
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

        private static List<JumpKing.Util.IDrawable> MenuFactoryDrawables
        {
            get
            {
                return MenuFactoryDrawablesAccessor.GetValue<List<JumpKing.Util.IDrawable>>();
            }
            set
            {
                MenuFactoryDrawablesAccessor.SetValue(value);
            }
        }
        private static Traverse MenuFactoryDrawablesAccessor;

        /// <summary>
        /// Initializes defaults for the class and applies patching via Harmony for menu functionality
        /// Called by <see cref="JumpKingPunishment.Setup"/>
        /// </summary>
        /// <param name="harmony">The harmony instance to use</param>
        public static void Initialize(Harmony harmony)
        {
            MenuFactoryDrawablesAccessor = null;

            var jumpKingCreateInGameOptionsMethod = AccessTools.Method("JumpKing.PauseMenu.MenuFactory:CreateIngameOptions");
            var menuInGameOptionsExtender = typeof(MenuOptions).GetMethod("ExtendInGameOptions");
            harmony.Patch(jumpKingCreateInGameOptionsMethod, postfix: new HarmonyMethod(menuInGameOptionsExtender));

            var jumpKingCreateOptionsMethod = AccessTools.Method("JumpKing.PauseMenu.MenuFactory:CreateOptionsMenu");
            var menuOptionsExtender = typeof(MenuOptions).GetMethod("ExtendOptions");
            harmony.Patch(jumpKingCreateOptionsMethod, postfix: new HarmonyMethod(menuOptionsExtender));
        }

        /// <summary>
        /// Helper to setup the MenuFactoryDrawablesAccessor Traverse instance so we can easily modify <see cref="JumpKing.PauseMenu.MenuFactory.m_drawables"/>
        /// </summary>
        /// <param name="__instance">The <see cref="JumpKing.PauseMenu.MenuFactory"/> instance to cache m_drawables from</param>
        private static void InitializeDrawables(object __instance)
        {
            MenuFactoryDrawablesAccessor = Traverse.Create(__instance).Field("m_drawables");
        }

        /// <summary>
        /// A helper to append a <see cref="JumpKing.Util.IDrawable"/> to the <see cref="JumpKing.PauseMenu.MenuFactory.m_drawables"/> list
        /// </summary>
        /// <param name="drawable">The <see cref="JumpKing.Util.IDrawable"/> to append</param>
        private static void AppendDrawable(JumpKing.Util.IDrawable drawable)
        {
            List<JumpKing.Util.IDrawable> Drawables = MenuFactoryDrawables;
            Drawables.Add(drawable);
            MenuFactoryDrawables = Drawables;
        }

        /// <summary>
        /// A helper to determine if a <see cref="MenuSelector"/> has a back button at the end of its Children list already and remove it if it does
        /// </summary>
        /// <param name="menuSelector">The <see cref="MenuSelector"/> instance to modify</param>
        private static void RemoveExistingBackButton(MenuSelector menuSelector)
        {
            IconButton menuLastIconButton = null;
            if (menuSelector.Children.Length > 0)
            {
                menuLastIconButton = menuSelector.Children[menuSelector.Children.Length - 1] as IconButton;
            }
            // If the last option isn't an icon button it's not a back button
            if (menuLastIconButton == null)
            {
                return;
            }

            // If the child on the icon button isn't 'MenuSelectorBack' it's not a back button
            MenuSelectorBack iconButtonChild = menuLastIconButton.Child as JumpKing.PauseMenu.BT.MenuSelectorBack;
            if (iconButtonChild == null)
            {
                return;
            }

            // This is a back button, remove it
            Traverse childrenAccessor = Traverse.Create(menuSelector).Field("m_children");
            List<IBTnode> childrenList = Enumerable.ToList<IBTnode>(childrenAccessor.GetValue<IBTnode[]>());
            childrenList[childrenList.Count - 1].OnDispose();
            childrenList.RemoveAt(childrenList.Count - 1);
            childrenAccessor.SetValue(childrenList.ToArray());
        }

        /// <summary>
        /// Called after <see cref="JumpKing.PauseMenu.MenuFactory.CreateIngameOptions"/> to add our punishment options to the in-game options menu
        /// </summary>
        public static void ExtendInGameOptions(GuiFormat p_format, GuiFormat p_sub_format, ref MenuSelector __result, object __instance)
        {
            InitializeDrawables(__instance);
            RemoveExistingBackButton(__result);

            __result.AddChild<TextButton>(new TextButton("Punishment Options", MenuOptions.CreatePunishmentOptions()));
            __result.Initialize(true);
        }

        /// <summary>
        /// Creates the punishment options menu
        /// </summary>
        public static MenuSelector CreatePunishmentOptions()
        {
            // We have too many options to just use a standard nested options menu, so we gotta roll our own-
            // basing this off the logic for the controls screen
            var font = Game1.instance.contentManager.font.MenuFontSmall;
            MenuSelector menuSelector = new MenuSelector(PunishmentOptionsGuiFormat);
            AppendDrawable(menuSelector);

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
        /// Called after <see cref="JumpKing.PauseMenu.MenuFactory.CreateOptionsMenu"/> to add our device options to the options menu
        /// </summary>
        public static void ExtendOptions(GuiFormat p_format, GuiFormat p_subformat, GuiFormat p_sub_sub_format, ref MenuSelector __result, object __instance)
        {
            InitializeDrawables(__instance);
            RemoveExistingBackButton(__result);

            __result.AddChild<TextButton>(new TextButton("Devices", MenuOptions.CreateDeviceOptions(p_subformat)));
            __result.Initialize(true);
        }

        /// <summary>
        /// Creates the device options menu
        /// </summary>
        public static MenuSelector CreateDeviceOptions(GuiFormat p_format)
        {
            MenuSelector menuSelector = new MenuSelector(p_format);
            AppendDrawable(menuSelector);   // This needs to happen before CreatePiShockOptions as the drawable order matters for rendering

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
            PunishmentFocusCompatableMenuSelector menuSelector = new PunishmentFocusCompatableMenuSelector(PunishmentOptionsGuiFormat);
            AppendDrawable(menuSelector);

            menuSelector.AddChild<PiShockUsernameOption>(new PiShockUsernameOption(font));
            menuSelector.AddChild<PiShockAPIKeyOption>(new PiShockAPIKeyOption(font));
            menuSelector.AddChild<PiShockShareCodeOption>(new PiShockShareCodeOption(font));
            menuSelector.AddChild<PiShockTestButton>(new PiShockTestButton(font));

            menuSelector.Initialize(true);

            return menuSelector;
        }
    }
}
