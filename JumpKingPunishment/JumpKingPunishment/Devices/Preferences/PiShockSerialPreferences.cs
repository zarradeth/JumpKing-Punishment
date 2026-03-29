using System.ComponentModel;

namespace JumpKingPunishment.Devices.Preferences
{
    /// <summary>
    /// An aggregate class of PiShockSerial Preferences
    /// </summary>
    public class PiShockSerialPreferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Ctor for creating a <see cref="PiShockSerialPreferences"/>
        /// Initializes the properties to their default values for use when the user didn't have previously existing settings
        /// </summary>
        public PiShockSerialPreferences()
        {
            deviceId = "";
        }

        /// <summary>
        /// The device ID to use with the PiShock Serial API
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
