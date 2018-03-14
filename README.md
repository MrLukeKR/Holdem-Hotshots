# Hold'em Hotshots: Mixed Reality Poker

A mobile Texas Hold'em game that removes the need to carry around a pack of cards! Just use your smart device to connect to the table and play with friends & family.

## Warnings

When loading Xamarin with this project, it is important **NOT** to update any packages to do with UrhoSharp or Xamarin when prompted. This is because _some_ projects benefit from these updates, however since ours is multi-platform, it can actually break cross-compiling setups e.g. Android, as there are then compatibility issues between the iOS SDK, Android SDK and Xamarin verions - at this point, they are stable.

### Hardware Requirements
| Requirement | Why do I need this? |
|-------------|---------------------|
|OpenGL 2.0 or Higher|Displaying 3D graphics|
|Apple Mac*|Building iOS binaries|
|Android 4.0.3 Minimum|To be able to run the App on an Android device|
|iOS 8.0 Minimum|To be able to run the App on an Apple device|

_*Only necessary when building iOS binaries, execution can still be performed with Anrdoid devices/simluators with the Android SDK_
### Software Requirements
| Requirement | Why do I need this? | Website | Operating System | Notes |
|-------------|---------------------|---------|------------------|-------|
|Visual Studio*|IDE for working with C#|https://www.visualstudio.com/vs/| Windows ||
|Xamarin Studio**|IDE for working with C#|https://www.xamarin.com/download| Mac ||
|Xamarin SDK*|Cross-platform development| https://www.xamarin.com/download | Windows, Mac||
|UrhoSharp.Forms|3D Game Engine|https://www.nuget.org/packages/UrhoSharp| Windows, Mac, Linux |Should work anyway, as the package is included in the code|
|Java Development Kit 8|Android related Java dependencies|http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html| Windows, Mac, Linux |Will not compile with anything less than JDK8|
|Android SDK|Android Mobile Development|https://developer.android.com/studio/index.html|Windows, Mac, Linux|Should come installed with Xamarin|

_*Only neccessary for Windows_


_**Only necessary for Mac_

## Authors
* Luke K. Rose
* George R. Thomas
* Jack Nicholson
* Michael Uzoka
* Xinyi Li
* Yipin (Rick) Jin

## License
This project is Copyright Advantage Software Group 2016-17 - see the LICENSE.md file for details

## Acknowledgments
### Tools Used

|Name|Type|Used For|URL|
|----|----|--------|---|
|Urho3D-Blender|Plugin|Modelling in Blender and exporting to Urho3D's format|https://github.com/reattiva/Urho3D-Blender|
|Steinberg Cubase 7|Proprietary Software|Music Production|https://www.steinberg.net/en/products/cubase/start.html|
|Blender|Open-Source Software|Modelling/Texturing|http://www.blender.org|
|GIMP|Open-Source Software|Graphics Editing|http://www.gimp.org|


## Troubleshooting

|Problem|Reason|Solution|
|-------|-----------|--------|
|*"Even though I've changed the code, running it results in behaviour of old code"*|Visual Studio is still deploying existing binaries|Try deleting the obj and bin files of the relevent platform output folders|
||The app on the device isn't being replaced|Try uninstalling the app from the device|
||There are redundancies in the solution|Try cleaning/rebuilding the solution|
|_**Warning:** There were deployment issues_|The build hasn't been setup for that device|Try a different device/emulator|
||*There were differences in deployment action files*|Try cleaning/rebuilding the solution - **DO NOT** try to autmoatically fix this error as it tends to cause problems elsewhere. This error seems to only occur on certain machines, but works fine elsewhere.|
|*"None of my Simulators are working"*|Virtualisation is disabled|Enable "Virtualisation Technology" in your BIOS/UEFI menu|
||Your Android Virtual Device (AVD) is out-of-date|Update your AVD with the Android SDK update manager (Tools > Android > Android SDK Manager)|
|_**Warning:**The application could not be started. Ensure that the application has been installed to the target device and has a launchable activity._|The App is either not installed, or can't be found for some reason|Uninstall the app from the device, clean the solution and re-deploy|
|_**Message:**The application couldn't be started_|There are still old install files on the device|Go to App Manager on the device and ensure all traces of the app are uninstalled (especially versions old than the last deployed)|
||The build settings are incorrect|Try different build settings|
||There are old install files that you can't see|Go to Tools > Anroid > Android Adb Command Promt and type **adb shell pm uninstall -k TexasHoldemPoker.Droid**|
