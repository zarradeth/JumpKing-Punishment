# Feedback Device Setup

Depending on the force feedback device you are using the Punishment Mod will need information to allow external control of/iteraction with the device. The settings for these devices can be found in the 'Device Options' menu opeion after selecting the installed mod in your mod list.

![image](https://github.com/user-attachments/assets/29ea525e-3a92-435b-afdc-e10830fefb70)

There are various ways in game to test if your device is properly configured and enabled:
1. Use the test button in the device's settings menu, if present, to trigger a test feedback event.
2. When starting Jump King, if you have already properly configured the device, a test feedback event should automatically trigger.
3. You can press 'F8' at any time while Jump King is running with the mod installed to trigger a test feedback event to your selected device.

The following devices are currently supported:
1. [PiShock](FeedbackDeviceSetup.md#pishock-setup)

## PiShock Setup

> [!CAUTION]
> You should generally keep the information needed to setup the PiShock private as it may allow others to control these devices as well. If you accidentally leak this information the tools linked to below should allow deleting/regenerating keys/codes such that the old ones no longer work.

This guide assumes you already have the PiShock device generally setup, if you do not follow guides on the [PiShock website](https://pishock.com/) to do this. You should confirm you can manually control your device when logged in (meaning everything is setup properly) [here](https://pishock.com/#/control).  
![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/058413f3-6291-42d9-8cf8-0030f342c498)

There are 3 unique pieces of information needed for the Puishment Mod to control the PiShock:  
- Your username on the PiShock website, [visible here if logged in](https://pishock.com/#/account).
- An API Key generated on the PiShock website (generated from the [accounts page](https://pishock.com/#/account)).
  - Note: If you have previously generated an API Key you can/should re-use it, your account can only have one active API Key and generating a new one will invalidate the old one (also useful if you accidentally share the key).
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/715a484e-0823-4124-bc8f-e2a304b8061b)
- A share code for the shocker you wish to use- generated from the [control page](https://pishock.com/#/control).
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/6b150e35-56e5-413b-a12f-f98be5c60829)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/67d31e34-5280-42e6-8981-e48b60e9dfd3)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/ed733254-442c-46fd-a39d-2f30469f0f56)
    - Note you can also delete previously generated codes (so they can no longer be used), limit the duration/intensity that can be triggered via this share code, or pause its functionality from here as well.

By selecting an option in the PiShock settings (select the setting and press 'confirm' to start editing it, the option name will turn yellow) you should be able to copy and paste (or manually type if you are crazy) these values into the input boxes. Note that when you are not actively editing these settings the current value of the option will be masked to help prevent leaking it.

Once you have configured the device settings you can select the 'Test' option to send a beep to the PiShock to confirm it's properly configured and working.  
![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/586b2418-355f-4749-9c45-a04a8adf415b)
