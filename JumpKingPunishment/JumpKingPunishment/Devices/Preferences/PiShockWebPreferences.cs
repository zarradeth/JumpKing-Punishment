using System.ComponentModel;

namespace JumpKingPunishment.Devices.Preferences
{
    /// <summary>
    /// An aggregate class of PiShockWeb Preferences
    /// </summary>
    public class PiShockWebPreferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Ctor for creating a <see cref="PiShockWebPreferences"/>
        /// Initializes the properties to their default values for use when the user didn't have previously existing settings
        /// </summary>
        public PiShockWebPreferences()
        {
            apiKey = "";
            deviceId = "";
        }

        /// <summary>
        /// The API Key to use with the PiShock API
        /// </summary>
        public string APIKey
        {
            get
            {
                return apiKey;
            }
            set
            {
                if (apiKey != value)
                {
                    apiKey = value;
                    RaisePropertyChanged("APIKey");
                }
            }
        }
        private string apiKey;

        /// <summary>
        /// The device ID to use with the PiShock API
        /// </summary>
        public string DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                if (deviceId != value)
                {
                    deviceId = value;
                    RaisePropertyChanged("DeviceId");
                }
            }
        }
        private string deviceId;

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event
        /// </summary>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
