# Mixed Reality Poker

A mobile Texas Hold'em game that removes the need to carry around a pack of cards! Just use your smart device to connect to the table and play - with friends, family or train by yourself.
## Warnings
* When loading Xamarin with this project, it is important **NOT** to update any packages to do with UrhoSharp or Xamarin when prompted. This is because _some_ projects benefit from these updates, however since ours is multi-platform, it can actually break cross-compiling setups e.g. Android, as there are then compatibility issues between the iOS SDK, Android SDK and Xamarin verions - at this point, they are stable.

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
A step by step series of examples that tell you have to get a development env running
/* TODO */

## Running the tests

Explain how to run the automated tests for this system
### Break down into end to end tests
Explain what these tests test and why
Give an example
/* TODO */

## Deployment
Add additional notes about how to deploy this on a live system
/* TODO */

## Built With

    Xamarin	- Cross-platform development via Shared Class Libraries
    UrhoSharp	- 3D Game Engine

## Versioning

/* TODO */

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
/* TODO */


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