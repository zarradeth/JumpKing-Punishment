using System.ComponentModel;

namespace JumpKingPunishment.Devices.Preferences
{
    /// <summary>
    /// An aggregate class of PiShock Preferences
    /// </summary>
    public class PiShockPreferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Ctor for creating a <see cref="PiShockPreferences"/>
        /// Initializes the properties to their default values for use when the user didn't have previously existing settings
        /// </summary>
        public PiShockPreferences()
        {
            username = "";
            apiKey = "";
            shareCode = "";
        }

        /// <summary>
        /// The username to use with the PiShock API
        /// </summary>
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }
        private string username;

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
        /// The share code to use with the PiShock API
        /// </summary>
        public string ShareCode
        {
            get
            {
                return shareCode;
            }
            set
            {
                if (shareCode != value)
                {
                    shareCode = value;
                    RaisePropertyChanged("ShareCode");
                }
            }
        }
        private string shareCode;

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event
        /// </summary>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
