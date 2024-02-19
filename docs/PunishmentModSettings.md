# Punishment Mod Settings

The settings for the punishment mod can be found in the 'Options' -> 'InGame' -> 'Punishment Options' menu if the mod is properly installed.

![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/e6ed3acf-d344-4b74-bb26-dc43c990b400)  

> [!CAUTION]
> You should follow any warnings/recommendations related to the usage of your device (such as placement/intensity/usage warnings) so that you do not potentially injure yourself!
>
> It's **highly** recommended you tune the Punishment mod settings to ensure you are comfortable playing the mod.  
> If you are unfamiliar with your device it's, again, highly recommended you experiment with the device before configuring/using this mod to help gauge what intensity, duration, and distance values you would be comfortable with!

## Settings

![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/8745b41e-d8cb-4879-98a6-93b018a9871d)

- **Mod Enabled:** Allows enabling and disabling the functionality of the mod as a whole.
  - **Note:** This will also clear your 'max progress' tracked by the mod for the **'Reward on New Progress Only'** setting, meaning you can get rewarded for progress you have already made again if you toggle the mod off and back on.
- **Feedback Device:** What feedback device the mod will use.
- **On Screen Display Behavior:** Controls how the mod displays messages on screen about punishments and rewards, see sections below for more information.
- **Round Durations:** Ticking this causes feedback event durations to be rounded when calculated.
  - This is useful when using the PiShock as it only supports durations that are whole seconds (no fractions)- you do not **need** to enable this as without this the duration sent to the PiShock API will still be rounded and it will still work, but enabling this makes any information displayed on screen match with your actual punishment/reward.
- **Punishment Enabled:** Ticking this enables the generation of punishment feedback events.
  - **Min Punishment Duration:** The minimum duration (in seconds) for a punishment feedback event (the amount triggered after falling your minimum fall distance).
  - **Max Punishment Duration:** The maximum duration (in seconds) for a punishment feedback event (the amount triggered after falling your maximum fall distance).
  - **Min Punishment Intensity:** The minimum intensity (0-100) for a punishment feedback event (the amount triggered after falling your minimum fall distance).
  - **Max Punishment Intensity:** The maximum intensity (0-100) for a punishment feedback event (the amount triggered after falling your maximum fall distance).
  - **Min Fall Distance:** The minimum fall distance you must fall in order to receive a punishment feedback event.
    - For more information about distances/what might be good values see sections below.
  - **Max Fall Distance:** The fall distance at which you will receive a maximum stength/duration punishment feedback event.
  - **Punishment Easy Mode:** Ticking this enables 'easy mode' for punishments, changing the type (or potentially strength/duration) of feedback generated for punishments.
    - For the PiShock this causes vibrations to be sent instead of shocks.  
- **Rewards Enabled:** Ticking this enables the generation of reward feedback events.
  - **Min Reward Duration:** The minimum duration (in seconds) for a reward feedback event (the amount triggered after gaining your minimum reward progress distance).
  - **Max Reward Duration:** The maximum duration (in seconds) for a reward feedback event (the amount triggered after gaining your maximum reward progress distance).
  - **Min Reward Intensity:** The minimum intensity (0-100) for a reward feedback event (the amount triggered after gaining your minimum reward progress distance).
  - **Max Reward Intensity:** The maximum intensity (0-100) for a punishment feedback event (the amount triggered after gaining your maximum reward progress distance).
  - **Min Reward Distance:** The minimum amount of progress you must gain in order to receive a reward feedback event.
  - **Max Reward Distance:** The progress distance at which you will receive a maximum strength/duration reward feedback event.
  - **Reward On New Progress Only:** Ticking this means you will only be rewarded the first time you make progress (based on height, not any displayed progress percentage). If enabled falling and re-making progress will not generate rewards.
    - **Note:** This value is not saved- restarting the game, toggling the mod at runtime, or teleports from other mods will reset this value and you may be rewarded for progress you technically previously made already.

 ### On Screen Display Behaviors

- **None:** No on screen information will be displayed by the mod.
- **MessageOnly:** Only a message indication there is an incoming punishment, or that a punishment/reward was triggered will be displayed.
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/093211e9-acd2-4a7e-8e9e-e14189cdf592)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/87e32a37-6022-44cd-8532-9b297cd297ad)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/2824ca3f-94b6-4abc-ad4e-fba82859632b)
- **DistanceBasedPercentage:** An on screen message along with a percentage indicating the strength of the punishment/reward (based on your min/max distance settings) will be displayed. This means someone cannot know the exact strength/duration of the feedback triggered on your device unless they know your settings.
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/368d16a0-0b51-4442-9de6-e980194e188e)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/d88085af-853e-4a6d-9cad-99e2d927cd48)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/7b50d290-3803-4f59-8043-6102eb47df23)
- **FeedbackIntensityAndDuration:** An on screen message along with the exact strength percentage and second duration of the punishment/reward (based on your settings) will be displayed on screen.
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/bf937f2f-0844-4677-8930-ff4a1d4d36ed)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/be47a9e3-8cbf-42a4-9a0b-4689a6cb933a)
  - ![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/1b8e1099-7c89-4088-9af3-c542319b256c)

## Distance Settings Tips

Determining what distance settings are right for you might take some tuning (and potentially an understanding of how the settings work), this section is here to help.

### How are distances calculated?

Distances are calculated based on the Y location (verticle position) of the player (the King himself). They are based on your last known position on the ground (sand and ice is considered ground, even if you are moving/sliding) versus your new location on the ground when landing- it is **not** based on splatting, the height you reach when jumping, or any kind of progress percentage the game tracks.

For example if the King jumps from the top location to the bottom location in the following image:  
![image](https://github.com/zarradeth/JumpKing-Punishment/assets/20621507/731aa54c-5d37-42cc-850d-b97687c6134e)
The fall distance used for the punishment calculation is represented by the red arrow (just the difference in the verticle position, not the actual length of the arrow here)- in this particular example the fall distance is 88 units.

Rewards are calculated in the exact same way, but are caused by upward progress.  
Note that for the 'Reward On New Progress Only' setting the highest Y value you reach is used to calculate your max progress. This means if you are in an area that forces a fall and you need to work your way back up (even if it's through new content) you will not receive new rewards until you are higher than you were before the fall.

#### What about teleports, user maps, or other mods?

Sometimes when changing between screens in Jump King the player is teleported as the map physically cannot exist as it's laid out. This **should be supported and work correctly** (at least with teleports done in the original game/DLC). If user maps use the same functionality to do teleports as the original game they should be supported as well (note however only basic testing has been done with user maps, you may want to test stuff yourself before committing to anything).

Other mods will likely work in tandem with the punishment mod, however you should be careful with mods that move/teleport the player- the punishment mod will attempt to detect and as properly as possible handle teleports but its detection may not be perfect. Note that when a teleport is detected your 'max progress' used for the **'Reward on New Progress Only'** setting will be reset (due to technical limitations). If you would like to use other mods in tandem with the Punishment mod you should do some basic testing with them before using them.

### What can I do to find distances that work for me?

One screen in Jump King should be 360 units tall, this means that if you wanted a distance setting of 3 screens you would use a distance of 1080 (3 * 360). This is a good rough guide for most players to work with.

If you would like to experiment more you can also run the mod with no device type selected in the options- this causes the mod to still display reward/punishment numbers but nothing will actually trigger. Similarly you can enable 'Punishment Easy Mode' to do testing as this will allow the mod to work, including with a configured device, but punishment feedback events will be less... painful.

### How does the min/max distance relate to the reward/punishment?

When you fall (or progress) your set minimum distance a punishment (or reward) will be triggered at your defined minimum duration/intensity.  
When you fall (or progress) your set maximum distance (or beyond) a punishment (or reward) will be triggered at your defined maximum duration/intensity.

If you fall (or progress) any amount in between your minimum and maximum distance a feedback event between your minimum and maximum settings will be calculated and triggered based on your settings and the distance.  
For example if you have a minimum fall distance of 0, a maximum fall distance of 1000, and fall 500 units a punishment will trigger half way between your minimum and maximum duration/intensity settings. If in this case your minimum punshiment intensity is 10, and your maximum punsihment intensity is 100, a punishment of intensity 55 (((100 - 10) * .5) + 10) would be triggered.

### What if I don't want any variation in the punishment/reward?

If you do not want any variation in the punishment/reward you can set the min and max fall/reward distance to the same value (the value at which a feedback event should trigger)- in this case **the minimum duration/intensity settings will always trigger** (the maximum duration/intensity is effectively unused).
