using JumpKingPunishment.Devices;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

namespace JumpKingPunishment.Preferences
{
    /// <summary>
    /// An aggregate class of Punishment Preferences
    /// </summary>
    public class PunishmentPreferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Ctor for creating a <see cref="PunishmentPreferences"/>
        /// Initializes the properties to their default values for use when the user didn't have previously existing settings
        /// </summary>
        public PunishmentPreferences()
        {
            // Enable
            punishmentModEnabled = true;

            // General
            feedbackDevice = EFeedbackDevice.None;
            punishmentTestFeedbackDebugKey = Keys.F8;
            onScreenDisplayBehavior = EPunishmentOnScreenDisplayBehavior.FeedbackIntensityAndDuration;
            roundDurations = false;

            // Punishment
            EnabledPunishment = true;
            minPunishmentDuration = 1.0f;
            minPunishmentIntensity = 1.0f;
            maxPunishmentDuration = 1.0f;
            maxPunishmentIntensity = 15.0f;
            minFallDistance = 150.0f;
            maxFallDistance = 1000.0f;
            punishmentEasyMode = false;

            // Rewards
            enabledRewards = false;
            minRewardDuration = 1.0f;
            minRewardIntensity = 10.0f;
            maxRewardDuration = 1.0f;
            maxRewardIntensity = 100.0f;
            minRewardDistance = 0.0f;
            maxRewardDistance = 150.0f;
            rewardProgressOnlyMode = true;
        }

        /// <summary>
        /// Whether the Punishment mod is enabled or not
        /// </summary>
        public bool PunishmentModEnabled
        {
            get
            {
                return punishmentModEnabled;
            }
            set
            {
                if (punishmentModEnabled != value)
                {
                    punishmentModEnabled = value;
                    RaisePropertyChanged(nameof(PunishmentModEnabled));

                }
            }
        }
        private bool punishmentModEnabled;

        /// <summary>
        /// Which feedback device to use
        /// </summary>
        public EFeedbackDevice FeedbackDevice
        {
            get
            {
                return feedbackDevice;
            }
            set
            {
                if (feedbackDevice != value)
                {
                    feedbackDevice = value;
                    RaisePropertyChanged(nameof(FeedbackDevice));
                }
            }
        }
        private EFeedbackDevice feedbackDevice;

        /// <summary>
        /// The key to use to send a test feedback event to your feedback device
        /// </summary>
        public Keys PunishmentTestFeedbackDebugKey
        {
            get
            {
                return punishmentTestFeedbackDebugKey;
            }
            set
            {
                if (punishmentTestFeedbackDebugKey != value)
                {
                    punishmentTestFeedbackDebugKey = value;
                    RaisePropertyChanged(nameof(PunishmentTestFeedbackDebugKey));
                }
            }
        }
        private Keys punishmentTestFeedbackDebugKey;

        /// <summary>
        /// The on screen display behavior we want to use
        /// </summary>
        public EPunishmentOnScreenDisplayBehavior OnScreenDisplayBehavior
        {
            get
            {
                return onScreenDisplayBehavior;
            }
            set
            {
                if (onScreenDisplayBehavior != value)
                {
                    onScreenDisplayBehavior = value;
                    RaisePropertyChanged(nameof(OnScreenDisplayBehavior));
                }
            }
        }
        private EPunishmentOnScreenDisplayBehavior onScreenDisplayBehavior;

        /// <summary>
        /// Whether calculated durations should round to the nearest second
        /// </summary>
        public bool RoundDurations
        {
            get
            {
                return roundDurations;
            }
            set
            {
                if (roundDurations != value)
                {
                    roundDurations = value;
                    RaisePropertyChanged(nameof(RoundDurations));
                }
            }
        }
        private bool roundDurations;

        /// <summary>
        /// Whether punishments are enabled or not
        /// </summary>
        public bool EnabledPunishment
        {
            get
            {
                return enabledPunishment;
            }
            set
            {
                if (enabledPunishment != value)
                {
                    enabledPunishment = value;
                    RaisePropertyChanged(nameof(EnabledPunishment));
                }
            }
        }
        private bool enabledPunishment;

        /// <summary>
        /// The minimum punishment duration
        /// </summary>
        public float MinPunishmentDuration
        {
            get
            {
                return minPunishmentDuration;
            }
            set
            {
                if (minPunishmentDuration != value)
                {
                    minPunishmentDuration = value;
                    RaisePropertyChanged(nameof(MinPunishmentDuration));
                }
            }
        }
        private float minPunishmentDuration;

        /// <summary>
        /// The minimum punishment intensity
        /// </summary>
        public float MinPunishmentIntensity
        {
            get
            {
                return minPunishmentIntensity;
            }
            set
            {
                if (minPunishmentIntensity != value)
                {
                    minPunishmentIntensity = value;
                    RaisePropertyChanged(nameof(MinPunishmentIntensity));
                }
            }
        }
        private float minPunishmentIntensity;

        /// <summary>
        /// The maximum punishment duration
        /// </summary>
        public float MaxPunishmentDuration
        {
            get
            {
                return maxPunishmentDuration;
            }
            set
            {
                if (maxPunishmentDuration != value)
                {
                    maxPunishmentDuration = value;
                    RaisePropertyChanged(nameof(MaxPunishmentDuration));
                }
            }
        }
        private float maxPunishmentDuration;

        /// <summary>
        /// The maximum punishment intensity
        /// </summary>
        public float MaxPunishmentIntensity
        {
            get
            {
                return maxPunishmentIntensity;
            }
            set
            {
                if (maxPunishmentIntensity != value)
                {
                    maxPunishmentIntensity = value;
                    RaisePropertyChanged(nameof(MaxPunishmentIntensity));
                }
            }
        }
        private float maxPunishmentIntensity;

        /// <summary>
        /// The minimum fall distance to receive a punishment
        /// </summary>
        public float MinFallDistance
        {
            get
            {
                return minFallDistance;
            }
            set
            {
                if (minFallDistance != value)
                {
                    minFallDistance = value;
                    RaisePropertyChanged(nameof(MinFallDistance));
                }
            }
        }
        private float minFallDistance;

        /// <summary>
        /// The fall distance at which you will receive the maximum punishment
        /// </summary>
        public float MaxFallDistance
        {
            get
            {
                return maxFallDistance;
            }
            set
            {
                if (maxFallDistance != value)
                {
                    maxFallDistance = value;
                    RaisePropertyChanged(nameof(MaxFallDistance));
                }
            }
        }
        private float maxFallDistance;

        /// <summary>
        /// Whether easy mode punishments are enabled (vibrate instead of shock)
        /// </summary>
        public bool PunishmentEasyMode
        {
            get
            {
                return punishmentEasyMode;
            }
            set
            {
                if (punishmentEasyMode != value)
                {
                    punishmentEasyMode = value;
                    RaisePropertyChanged(nameof(PunishmentEasyMode));
                }
            }
        }
        private bool punishmentEasyMode;

        /// <summary>
        /// Whether rewards are enabled or not
        /// </summary>
        public bool EnabledRewards
        {
            get
            {
                return enabledRewards;
            }
            set
            {
                if (enabledRewards != value)
                {
                    enabledRewards = value;
                    RaisePropertyChanged(nameof(EnabledRewards));
                }
            }
        }
        private bool enabledRewards;

        /// <summary>
        /// The minimum shock duration when receiving a reward
        /// </summary>
        public float MinRewardDuration
        {
            get
            {
                return minRewardDuration;
            }
            set
            {
                if (minRewardDuration != value)
                {
                    minRewardDuration = value;
                    RaisePropertyChanged(nameof(MinRewardDuration));
                }
            }
        }
        private float minRewardDuration;

        /// <summary>
        /// The minimum shock intensity when receiving a reward
        /// </summary>
        public float MinRewardIntensity
        {
            get
            {
                return minRewardIntensity;
            }
            set
            {
                if (minRewardIntensity != value)
                {
                    minRewardIntensity = value;
                    RaisePropertyChanged(nameof(MinRewardIntensity));
                }
            }
        }
        private float minRewardIntensity;

        /// <summary>
        /// The maximum shock duration when receiving a reward
        /// </summary>
        public float MaxRewardDuration
        {
            get
            {
                return maxRewardDuration;
            }
            set
            {
                if (maxRewardDuration != value)
                {
                    maxRewardDuration = value;
                    RaisePropertyChanged(nameof(MaxRewardDuration));
                }
            }
        }
        private float maxRewardDuration;

        /// <summary>
        /// The maximum shock intensity when receiving a reward
        /// </summary>
        public float MaxRewardIntensity
        {
            get
            {
                return maxRewardIntensity;
            }
            set
            {
                if (maxRewardIntensity != value)
                {
                    maxRewardIntensity = value;
                    RaisePropertyChanged(nameof(MaxRewardIntensity));
                }
            }
        }
        private float maxRewardIntensity;

        /// <summary>
        /// The minimum progress amount to receive a reward
        /// </summary>
        public float MinRewardDistance
        {
            get
            {
                return minRewardDistance;
            }
            set
            {
                if (minRewardDistance != value)
                {
                    minRewardDistance = value;
                    RaisePropertyChanged(nameof(MinRewardDistance));
                }
            }
        }
        private float minRewardDistance;

        /// <summary>
        /// The progress amount at which you will receive the maximum reward
        /// </summary>
        public float MaxRewardDistance
        {
            get
            {
                return maxRewardDistance;
            }
            set
            {
                if (maxRewardDistance != value)
                {
                    maxRewardDistance = value;
                    RaisePropertyChanged(nameof(MaxRewardDistance));
                }
            }
        }
        private float maxRewardDistance;

        /// <summary>
        /// Whether progress only mode is enabled for rewards (only rewarded when getting new progress)
        /// </summary>
        public bool RewardProgressOnlyMode
        {
            get
            {
                return rewardProgressOnlyMode;
            }
            set
            {
                if (rewardProgressOnlyMode != value)
                {
                    rewardProgressOnlyMode = value;
                    RaisePropertyChanged(nameof(RewardProgressOnlyMode));
                }
            }
        }
        private bool rewardProgressOnlyMode;

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event
        /// </summary>
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
