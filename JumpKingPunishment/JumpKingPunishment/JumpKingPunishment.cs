using HarmonyLib;
using JumpKing.Mods;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;
using JumpKingPunishment.Devices;
using JumpKingPunishment.Models;
using JumpKingPunishment.Preferences;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace JumpKingPunishment
{
    /// <summary>
    /// Contains the main entry points for the mod
    /// </summary>
    [JumpKingMod("Zarradeth.JumpKingPunishment")]
    public static class JumpKingPunishment
    {
        private static readonly string PunishementPreferencesFile = "JKPM-Punishment.Preferences.xml";
        public static PunishmentPreferences PunishmentPreferences;

        private static string AssemblyPath;

        /// <summary>
        /// Called during Jump King startup and used to set up most of the mod
        /// </summary>
        [BeforeLevelLoad]
        public static void Setup()
        {
            AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Load/setup the general punishment settings
            try
            {
                PunishmentPreferences = XmlSerializerHelper.Deserialize<PunishmentPreferences>(Path.Combine(AssemblyPath, PunishementPreferencesFile));
                // Because the player can manually edit the XML we would maybe want to run validation that the settings are within range/valid.
                // HOWEVER, because of how the slider settings are setup (the only thing we would need to validate) we will actually clamp/fix
                // any bad settings when the option entries initialize
            }
            catch
            {
                PunishmentPreferences = new PunishmentPreferences();
            }

            PunishmentPreferences.PropertyChanged += SavePunishmentSettings;

            // Load/setup device settings
            DeviceManager.InitializeAllDeviceSettings();

            // Actually kick off the mod
            Harmony harmony = new Harmony("Zarradeth.JumpKingPunishment.Harmony");
            PunishmentManager.Initialize(harmony);
        }

        /// <summary>
        /// Called when Jump King starts a level and used to let the Punishment manager know
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            PunishmentManager.OnLevelStart();
        }

        /// <summary>
        /// Called when Jump King ends a level and used to let the Punishment manager know
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            PunishmentManager.OnLevelEnd();
        }

        /// <summary>
        /// Creates the mod settings menu item for opening the punishment options submenu
        /// </summary>
        [OnLevelEnd]
        [PauseMenuItemSetting]
        [MainMenuItemSetting]
        public static TextButton PunishemntOptions(object factory, GuiFormat format)
        {
            return new TextButton("Punishment Options", MenuOptions.CreatePunishmentOptions());
        }

        /// <summary>
        /// Creates the mod settings menu item for opening the device options submenu
        /// </summary>
        [PauseMenuItemSetting]
        [MainMenuItemSetting]
        public static TextButton DeviceOptions(object factory, GuiFormat format)
        {
            return new TextButton("Device Options", MenuOptions.CreateDeviceOptions());
        }

        /// <summary>
        /// A helper to save the punishment settings to disk when they are modified
        /// </summary>
        private static void SavePunishmentSettings(object sender, PropertyChangedEventArgs args)
        {
            try
            {
                XmlSerializerHelper.Serialize(Path.Combine(AssemblyPath, PunishementPreferencesFile), PunishmentPreferences);
            }
            catch
            {
            }
        }
    }
}
