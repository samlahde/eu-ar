# AR Maps
The application should implement real-time or user-created maps and visualize content, such as moving vehicles, people (or animals), points of interest etc. For example: a tactical military sandbox, an interactive park map, a guide map of an airport, etc.


## Tools Used
- Unity Engine (2020.3.19f1 LTS)
- Visual Studio Community 2019 (16.11.5)
- Microsoft HoloLens 2
- HoloLens 2 Emulator (if the device is unavailable)

## Setup
### Installing Visual Studio Community 2019

When installing Visual Studio 2019, make sure you have the following workflow packages installed:

- Desktop development with C++
- Universal Windows Platform development
- Game development with Unity


### Installing MRTK for Unity
1. Download the **Mixed Reality Feature Tool**: https://www.microsoft.com/en-us/download/details.aspx?id=102778
2. Download and import the MRTK Unity packages: https://github.com/Microsoft/MixedRealityToolkit-Unity/releases
3. Import the MRTK Unity packages into a new 3D Unity project:
    1. *Microsoft.MixedReality.Toolkit.Unity.Foundation.2.7.2.unitypackage*
    2. *Microsoft.MixedReality.Toolkit.Unity.Extensions.2.7.2.unitypackage*
    3. *Microsoft.MixedReality.Toolkit.Unity.Examples.2.7.2.unitypackage*
    4. *Microsoft.MixedReality.Toolkit.Unity.Tools.2.7.2.unitypackage*
    Note: You can also import those packages using the Mixed Reality Feature Tool desktop application, then import them into the project using the Package Manager in Unity.
1. Go to **File → Build Settings** and switch the platform to “*Universal Windows Platform*”
2. Go to **Project Settings → XR Plug-in Management**, select the *Universal Windows Platform settings* tab and select the following options: 
    1. *OpenXR*
    2. *Microsoft HoloLens feature group*
    3. *Windows Mixed Reality* (if possible)
3. Go to **Project Settings → XR Plug-in Management → Open XR** and set the following options:
    1. **Render Mode** = “*Single Pass Instanced*”
    2. **Depth Submission Mode** = “*Depth 16-Bit*”
4. Go to **Project Settings → Player → Publishing Settings → Capabilities** and make sure the following are selected:
    1. *Microphone*
    2. *SpatialPerception*
    3. *InternetClient*
    4. *Webcam*
    5. *Location*
    6. *Objects3D*
    7. *GazeInput*
5. Go to **Project Settings → Player → Publishing Settings → Supported Device Families** and make sure the following are selected:
    1. *Mobile*
    2. *Holographic*


## Building and Deploying Apps 
### Building the Project (method 1: using MRTK)

Once you are ready to build the project, follow these steps:

1. Go to **File → Build Settings** and add all the required Scenes
2. Navigate to **Mixed Reality → Toolkit → Utilities → Build Window**
3. In ***Unity Build Options***:
    1. Set the **Target Device** to “*HoloLens*”
4. In ***Appx Build Options***:
    1. Set **Build Configuration** to the required type (Default: *Master*)
    2. Set **Build Platform** to “*ARM64*”
    3. Set **Platform Toolset** to “*Solution*”
    4. Change the **Version Number** if necessary
5. Click “***Build All***” once all the build options are set


### Building the Project (method 2: using Visual Studio 2019)

Once you are ready to build the project, follow these steps:

1. Go to **File → Build Settings** and add all the required Scenes
2. In ***Build Settings***:
    1. **Target Device** => “*HoloLens*”
    2. **Architecture** => “*ARM64*”
    3. **Build Type** => “*D3D Project*”
    4. **Target SDK version** => “*Latest installed*"
    5. **Minimum Platform Version** => choose the smallest value (usually the first on the list)
    6. **Visual Studio Version** => “*Latest installed*”
    7. **Build an Run on** => “*Local Machine*”
    8. **Build Configuration** => “*Release*” (or *Master*)
3. Click “***Build***” once all the build options are set
4. Once the project has been built, open the project solution (.sln) in Visual Studio 2019

On ***Visual Studio 2019***:

1. In the **Solution Explorer**, right-click on the Project **[AR-Maps (Universal Windows)] → Publish → Create App Packages…**
    1. Select distribution method => *Sideloading*
    2. Select signing method => *Yes, use the current certificate: …*
    3. Select and configure packages => choose *[ARM64, Release (ARM64)]*
2. Click “***Create***” to build and export the project into an Appx package.


### Deploying on the HoloLens 2
1. On the ***Windows Device Portal***, navigate to **Views → Apps → Deploy Apps** and choose the application file you wish to install on the HoloLens 2.
    1. The app package can be found with the file format (.appx) in the following directory: *<build-directory>\AppPackages\<AppName>\<AppName>_1.0.0.0_ARM64_Test\<AppName>.appx*
2. Click “***Install***” and it will be available in the HoloLens 2’s app menu


### Running the App on the HoloLens 2 through Unity 

To run the app on the HoloLens, you have to configure the following settings:
On ***HoloLens 2***:

1. Open the ***Holographic Remoting*** app (It can be downloaded from the **Windows Store** on the device)

In ***Unity***:

1. Go to **Mixed Reality → Remoting → Holographic Remoting for Play Mode**.
2. In **Remote Host Name**, add the *HoloLens 2 device’s IP address* which is displayed on the Holographic Remoting app.
3. Set the **Remote Host Port** to “*8265*”
4. Click on **Enable Holographic Remoting for Play Mode**.

Once you click ***Play***, the ***Holographic Remoting*** app on the HoloLens 2 should show “*Receiving...*” for a moment, and then display the app. 

- You may need to allow the permissions that pop up before running the app.

For more details, refer to this video: https://youtu.be/XlzM1uRSkns



## Potential Errors and Fixes
### ‘Description’ Error

For files that have an error related to this:

    error CS0104: 'Description' is an ambiguous reference between 'Mapbox.VectorTile.Geometry.DescriptionAttribute' and 'System.ComponentModel.DescriptionAttribute'

#### Solution
Comment out the following line:

    using Mapbox.VectorTile.Geometry; 

from the following files:

- Assets\Mapbox\Core\mapbox-sdk-cs\MapMatching\MapMatchingParameters.cs
- Assets\Mapbox\Core\mapbox-sdk-cs\Tokens\MapboxTokenApi.cs



### ‘MapboxAccounts’ Error

For errors related to the MapboxAccounts:

    Assets\Mapbox\Unity\MapboxAccess.cs(1,7): error CS0246: The type or namespace name 'MapboxAccountsUnity' could not be found (are you missing a using directive or an assembly reference?)
    
    Assets\Mapbox\Unity\MapboxAccess.cs(332,27): error CS0246: The type or namespace name 'MapboxAccounts' could not be found (are you missing a using directive or an assembly reference?)
    
    Error building Player because scripts had compiler errors

#### Solution:
Go to Assets/Mapbox/Core/Plugins/Mapbox/MapboxAccounts/net4x/MapboxAccountsUnity.dll in Unity and check the WSAPlayer checkbox in the Inspector and click on “Apply”.
