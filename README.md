# Mixed Reality Poker

A mobile Texas Hold'em game that removes the need to carry around a pack of cards! Just use your smart device to connect to the table and play with friends & family.

## Warnings

When loading Xamarin with this project, it is important **NOT** to update any packages to do with UrhoSharp or Xamarin when prompted. This is because _some_ projects benefit from these updates, however since ours is multi-platform, it can actually break cross-compiling setups e.g. Android, as there are then compatibility issues between the iOS SDK, Android SDK and Xamarin verions - at this point, they are stable.

## Project Links

If an invite to view these links is required, please e-mail Luke (psylr5) to get an invite

|Name|Used For|URL|
|----|--------|---|
|Trello|Task Management|https://trello.com/team5poker|
|OneDrive|Document Hosting|[View Documents](https://uniofnottm-my.sharepoint.com/personal/psylr5_ad_nottingham_ac_uk/_layouts/15/guestaccess.aspx?folderid=0588baa14fc9e46609ae9dc704eab6549&authkey=AUF28Cs0lEmeiGm_7y39aCg) (Guest Access) - [Edit Documents](https://uniofnottm-my.sharepoint.com/personal/psylr5_ad_nottingham_ac_uk/Documents/Advantage%20Software%20Group) (Advantage Software Group ONLY - Login Required)|
|Slack|Team Communication|https://g52grp5.slack.com|

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

1. Load Visual Studio
2. Under the **Team Explorer** tab, select **Manage Connections**
3. Under the **Local Git Repositories** menu, click **Clone**
4. For the URL, enter **https://projects.cs.nott.ac.uk/G52GRP_TEAM05_2016_Advantage_Software_Group/holden.git**, login, set a directory for your Local Repository and click **Clone**
5. Double-click on the newly created Repository or **navigate to Home**
6. Click **Branches**, then **remotes/origin** and double-click the required branch
7. **Checkout** the branch as a **Local Branch**
8. Navigate back to **Home**
9. Under the **Solutions** menu, double-click **'TexasHoldemPoker.sln'**
10. Plug in a device, or setup a virtual one and hit **Run**!


### Hardware Requirements
| Requirement | Why do I need this? |
|-------------|---------------------|
|OpenGL 2.0 or Higher|Displaying 3D graphics|
|Apple Mac*|Building iOS binaries|
|Android 4.0.3 Minimum|To be able to run the App on an Android device|
|_iOS x.y.z_ **To be confirmed**|To be able to run the App on an Apple device|

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

### Installing
#### Android
- Request an APK from the Team Leader
- Copy the file to your Android device
- Open the file from a file manager and follow the on-screen instructions

#### iOS
- Open Xcode
- Create a project with the same name as the source code bundle identifier: *advantagesoftwaregroup.hold-em-hotshots*
- Tick "Automatically manage signing"
- In Xamarin Studio, right-click the iOS project
- Go to iOS signing
- Under certificate, select the newly created certificate from Xcode


## Deployment
To get the App setup on your device, simply follow these steps:
### Visual Studio 
#### Debug Deployment
1. Plug in your device
2. Ensure you have debugging enabled on your phone (Go to **Settings > About Phone** and keep tapping _Build Number_ until it says "You are now a developer")
3. Allow debugging on your device when prompted
4. When your device's name shows up next to the play button in the toolbar, press it

#### Official Release

- Currently due to be shown off a group project open day 17th May 2017!

## Authors
* Luke Kevin Rose
* George Robert Thomas
* Jack Nicholson
* Michael Uzoka
* Xinyi Li
* Yipin (Rick) Jin

See also the list of contributors who participated in this project.
## License
This project is Copyright Advantage Software Group 2016 - see the LICENSE.md file for details

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