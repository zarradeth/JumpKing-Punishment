# Feedback Device Setup

Depending on the force feedback device you are using the Punishment Mod will need information to allow external control of/iteraction with the device. The settings for these devices can be found in the 'Device Options' menu option after selecting the installed mod in your mod list.

![image](https://github.com/user-attachments/assets/381c1a15-ce46-4e13-b2b0-1ac5a2391bac)

There are various ways in game to test if your device is properly configured and enabled:
1. Use the test button in the device's settings menu, if present, to trigger a test feedback event.
2. When starting Jump King, if you have already properly configured the device, a test feedback event should automatically trigger.
3. You can press 'F8' at any time while Jump King is running with the mod installed to trigger a test feedback event to your selected device.

The following devices are currently supported:
1. [PiShockWeb](FeedbackDeviceSetup.md#pishockweb-setup)
2. [PiShockSerial](FeedbackDeviceSetup.md#pishockserial-setup)

## PiShockWeb Setup
This 'device' allows controlling a PiShock device over the internet using the PiShock web API- this option will have latency versus the PiShockSerial 'device' but depending on your setup this may be preferrable over Serial.

> [!CAUTION]
> You should generally keep the information needed to setup the PiShock private as it may allow others to control these devices as well. If you accidentally leak this information the tools linked to below should allow deleting/regenerating keys/codes such that the old ones no longer work.

> [!IMPORTANT]
> If you haven't updated the firmware on your PiShock in a long time you may need to udpate it so it can work with the latest APIs. If you have having trouble you can update/check if you have the latest firmware using hte tools/guide provided here: [https://docs.pishock.com/pishock/pishock-v3-documentation.html](https://docs.pishock.com/pishock/pishock-v3-documentation.html#Reflashing_0).

This guide assumes you already have the PiShock device generally setup, if you do not follow guides on the [PiShock website](https://pishock.com/) to do this. You should confirm you can manually control your device when logged in (meaning everything is setup properly) [here](https://pishock.com/#/control).  
![image](https://github.com/user-attachments/assets/b6727790-2872-4709-9969-34fbe0202d3f)

There are 2 unique pieces of information needed for the Puishment Mod to control the PiShock over the web:  
- An API Key generated on the PiShock website (generated from the [accounts page](https://login.pishock.com/account)).
  - Note: Especially old API keys may not work with the newer PiShock web APIs, if your key is old consider trying to generate a new one.
  - ![image](https://github.com/user-attachments/assets/0098378b-a116-4d62-b6de-9a5ae69f5037)
- The Device ID of the shocker you wish to use- this can most easily be found on the [control page](https://pishock.com/#/control).
  - ![image](https://github.com/user-attachments/assets/060259ff-551b-4d40-85a6-371b9d4b87b4)
  - ![image](https://github.com/user-attachments/assets/548a6774-81e4-4db0-9cd8-abc85426a0a7)

By selecting an option in the PiShockWeb settings (select the setting and press 'confirm' to start editing it, the option name will turn yellow) you should be able to copy and paste (or manually type if you are crazy) these values into the input boxes. Note that when you are not actively editing these settings the current value of the option will be masked to help prevent leaking it.

Once you have configured the device settings you can select the 'Test' option to send a beep to the PiShock to confirm it's properly configured and working. Since requests to the device needs to make a round trip over the internet it may take a bit fo the device to respond even if it is configured correctly.
![image](https://github.com/user-attachments/assets/fe07dbd7-4229-4699-949d-1880b6e64ccd)

## PiShockSerial Setup
This 'device' allows controlling a PiShock device via the HUB *connected directly to your PC via USB*. This eliminates the lag encountered when using the Web API and is the preferred way to use the PiShock.

> [!CAUTION]
> You should generally keep the information needed to setup the PiShock private as it may allow others to control these devices as well. If you accidentally leak this information the tools linked to below should allow deleting/regenerating keys/codes such that the old ones no longer work.

> [!IMPORTANT]
> If you haven't updated the firmware on your PiShock in a long time you may need to udpate it so it can work with the latest APIs. If you have having trouble you can update/check if you have the latest firmware using hte tools/guide provided here: [https://docs.pishock.com/pishock/pishock-v3-documentation.html](https://docs.pishock.com/pishock/pishock-v3-documentation.html#Reflashing_0).
> 
> If you do use PiShock Tools to update your firmware/interact with the device you should close it when finished/before using the mod to ensure the mod can communicate with the PiShock HUB's COM port (only one application can use a COM port at a time).

This guide assumes you already have the PiShock device generally setup, if you do not follow guides on the [PiShock website](https://pishock.com/) to do this. You should confirm you can manually control your device when logged in (meaning everything is setup properly) [here](https://pishock.com/#/control). Even though the device will be used locally it must have already been properly setup to work.
![image](https://github.com/user-attachments/assets/9cc425b3-77e5-4553-83e9-3635baa71a8c)

There is only one piece of information needed for the Puishment Mod to control the PiShock over Serial:  
- The Device ID of the shocker you wish to use- this can most easily be found on the [control page](https://pishock.com/#/control).
  - ![image](https://github.com/user-attachments/assets/060259ff-551b-4d40-85a6-371b9d4b87b4)
  - ![image](https://github.com/user-attachments/assets/548a6774-81e4-4db0-9cd8-abc85426a0a7)

By selecting an option in the PiShockSerial settings (select the setting and press 'confirm' to start editing it, the option name will turn yellow) you should be able to copy and paste (or manually type if you are crazy) these values into the input boxes. Note that when you are not actively editing these settings the current value of the option will be masked to help prevent leaking it.

Once you have configured the device settings you can select the 'Test' option to send a beep to the PiShock to confirm it's properly configured and working. With Serial the device should respond almost instantly- if the device does not respond try a couple more times (sometimes the HUB can be a little finicky when connected to a PC). If it still doesn't work make sure you don't have anything like PiShock Tools running that may be using the device's COM port itself (maybe try restarting your PC).
![image](https://github.com/user-attachments/assets/acefc4ec-9cf5-4f4a-b141-ae8400b24a58)
