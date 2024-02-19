using JumpKingPunishment.Devices.Preferences;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace JumpKingPunishment.Devices
{
    /// <summary>
    /// A central class for managing and creating feedback devices, this exists mostly to centralize code for devices to make adding new ones easier
    /// </summary>
    public static class DeviceManager
    {
        private static string AssemblyPath;

        private const string PiShockPreferencesFile = "JKPM-PiShock.Preferences.xml";
        public static PiShockPreferences PiShockPreferences;
        // To add another device add a preferences file and class instance here

        /// <summary>
        /// Initializes the settings for all supported devices, this will load the settings from disk if they exist and setup modified callbacks for saving them
        /// </summary>
        public static void InitializeAllDeviceSettings()
        {
            AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // We want to initialize settings for all devices since the player could swap them during gameplay
            InitializePreferences<PiShockPreferences>(ref PiShockPreferences, PiShockPreferencesFile, SavePiShockPreferences);

            // To add another device initialize the settings here (and add a 'Save' method for it)
        }

        /// <summary>
        /// Creates a device of the specified type
        /// </summary>
        /// <param name="deviceType">A <see cref="EFeedbackDevice"/> for the type of device you would like constructed.</param>
        /// <returns>A <see cref="IPunishmentDevice"/> instance that implements the requested device type, you should try to keep and reuse this instance</returns>
        public static IPunishmentDevice CreateDevice(EFeedbackDevice deviceType)
        {
            switch (deviceType)
            {
                case EFeedbackDevice.PiShock:
                    return new PiShockDevice();
                // To add another device add a case here
                default:
                    return null;
            }
        }

        /// <summary>
        /// Saves the PiShock preferences to disk
        /// </summary>
        private static void SavePiShockPreferences(object sender, PropertyChangedEventArgs args)
        {
            SavePreferences(ref PiShockPreferences, PiShockPreferencesFile);
        }

        /// <summary>
        /// A generic helper for preferencse initialization
        /// </summary>
        /// <typeparam name="T">The preferences class that we should setup/create</typeparam>
        /// <param name="preferences">A reference to the preferences class that will contain the loaded/created preferences</param>
        /// <param name="preferencesFileName">The filename that should contain the preferences on disk</param>
        /// <param name="savePreferencesMethod">The method that should be called to save the preferences to disk (trigged via INotifyPropertyChanged) </param>
        private static void InitializePreferences<T>(ref T preferences, string preferencesFileName, PropertyChangedEventHandler savePreferencesMethod) where T : INotifyPropertyChanged, new()
        {
            try
            {
                preferences = XmlSerializerHelper.Deserialize<T>(Path.Combine(AssemblyPath, preferencesFileName));
            }
            catch
            {
                preferences = new T();
            }

            preferences.PropertyChanged += savePreferencesMethod;
        }

        /// <summary>
        /// A generic helper for saving preferences to disk
        /// </summary>
        /// <typeparam name="T">The preferences class that we should save</typeparam>
        /// <param name="Preferences">A reference to the preferences class that will be saved</param>
        /// <param name="PreferencesName">The filename that will contain the preferences on disk</param>
        private static void SavePreferences<T>(ref T Preferences, string PreferencesName)
        {
            try
            {
                XmlSerializerHelper.Serialize(Path.Combine(AssemblyPath, PreferencesName), Preferences);
            }
            catch
            {
            }
        }
    }
}
