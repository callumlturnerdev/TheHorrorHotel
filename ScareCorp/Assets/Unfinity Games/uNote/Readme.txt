-------------------------------------------------------------------------
uNote
Copyright © 2015-2017 Unfinity Games

Last updated March 4th, 2017
-------------------------------------------------------------------------

Welcome to uNote, and thank you for purchasing!  
This readme is split into 6 sections, Installation, Updating, Settings, Examples, Help, and the Changelog.

-------------------------------------------------------------------------

1. INSTALLATION FROM UNITY ASSET STORE

Simply download and import the package through Unity and everything should be good-to-go.  

If everything imported correctly, you should be ready to use uNote!

-------------------------------------------------------------------------

2. UPDATING FROM AN OLDER VERSION

There are some breaking changes from v1.11 to any newer version.
If you're upgrading, there are some steps you need to follow.

(Optional) Backup your project.  This isn't required, but it's highly recommended due to how Unity imports assets.
A. Delete the Unfinity Games/Shared and Unfinity Games/uNote folders.
B. Download and import the new version from the Unity Asset store (using the instructions above, if needed).
C. That's it!  You should be good to go!

-------------------------------------------------------------------------

3. SETTINGS

The uNote Settings are located under Edit->Preferences...->uNote.  Additionally, there are clickable menu
buttons for our forums (great for getting help!), an "About" popup that contains some useful info,
and an updater utility (to see if a new version is available, and to read its changelog if there's an update).
Additionally, the Note component can be found under Component->Unfinity Games->uNote->Note.

There are many options that you can configure, and all of them should be pretty self-explanatory.
If you have questions, though, feel free to contact us.

-------------------------------------------------------------------------

3. EXAMPLES

uNote comes with an Examples folder that contains a script (uNoteHeroExample) designed to help show
how to add uNote notes to custom scripts.

The script file contains a lot of information regarding the process of adding uNote notes to scripts, and much more.
Be sure to take a look and see how the information in the script file translates to an actual component!  (Just add
the script to any object in your scene.)

If the examples aren't enough, and you feel like you still require assistance, see the below section on how to contact
us to receive help.

-------------------------------------------------------------------------

5. HELP

Help is available either via email, or by the use of the Unfinity Games Forums.
When seeking help, we would prefer the forums as any questions posted there will help others with the 
same questions, but ultimately, if you need help either avenue is absolutely fine.

Forums: http://www.unfinitygames.com/interact
Email: http://www.unfinitygames.com/contact-us/

-------------------------------------------------------------------------

6. CHANGELOG

November 2017: v1.17
- Fixed a case where Unity could load some textures for the Unfinity Games Updater incorrectly, resulting in color-shifted images.
- Fixed a potential case where the uNote Property Drawer could retrieve an incorrect inspector width from Unity, leading to an extra-wide Note drawer.
- Updated the Shared code to the latest version.

March 2017: v1.16
- Fixed a case where Unity could load our Editor textures incorrectly, resulting in color-shifted images.
- Updated the Shared code to the latest version.

November 2016: v1.15
- Updated the Shared code to be more in-line with our new plugin, uTemplate.
- Removed the forced fixed-width of the Preferences menu.  It'll now do its best to scale to the width of the window.
- Hooked uNote up to the new Unfinity Games Updater.
- uNote has been moved to a DLL.  This is not only easier to import (less chances for errors, or duplicated scripts), but it makes it easier to move around within your project.
- Improved the speed of various internal method calls.
- Fixed a few typos.
- A few other small fixes/adjustments.

June 2015: v1.11
- Resolved a rare, but critical error relating to building for certain platforms.

June 2015: v1.10
- Resolved a critical error relating to custom script integrations.
- Resolved a critical error relating to adding uNote notes to custom scripts inside of prefabs.
- Reworked a lot of the display code for notes to better support both the Note component, as well as the standalone PropertyDrawer.
- The Note component and PropertyDrawer better respond to the resizing of the inspector panel.
- Enabled wordwrapping for note titles.  This affects edit mode (while you're typing the title in) as well as display mode (when you're viewing the finished note).
- Added an Examples folder that contains a script (uNoteHeroExample.cs) that should help outline how to integrate uNote notes into your custom scripts. 
- Added a new feature: Parent Notes.  When enabled, uNote Notes can search their parent object(s) for notes, and display them in the scene along with their own note(s).
- Added two new preferences: Display Parent Notes and Only Search First Parent.  Both preferences are used in conjunction with the new Parent Notes feature.
- Fixed a class name that referenced our other plugin, U2DEX, rather than uNote.
- Added some preventative measures to ensure that some note array indexes won't go out-of-bounds in certain circumstances.
- The SceneView note box is much better at correctly handling the resizing of the note array (adding or removing notes).
- Enabled wordwrapping for note titles in the SceneView note box.
- Reworked how the SceneView note box is displayed.  It's much more responsive to changes to the SceneView window size.
- Improved SceneView Note rendering with the Light skin in Unity versions earlier than Unity 5.
- Added a bit of space near the top of the Note PropertyDrawer in Unity versions earlier than Unity 5 (to keep styling more consistent across all versions).
- Updated the Readme with an Examples section.

April 2015: v1.00
- Initial Release

-------------------------------------------------------------------------