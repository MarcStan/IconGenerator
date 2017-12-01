# Icon generator for all Xamarin mobile platforms

Generates app logos, splashscreens and in app icons for
* UWP
* iOS
* Android

Android and iOS get automatic icon scaling (via their resource naming xhdpi and @2x).

UWP doesn't support scaling of in app icons yet (only splashscreen and logo).

# Motivation

Reduce the number of asset files stored  in the git repository

Automate icon scaling and prevent human error when editing icons (e.g. forgetting to update one or two icons).

Currently any single icon is needed at the very least 9 times (1x UWP, 1x iOS and 1x Android) but more likely 10x times (1x UWP, 3x scaling for iOS at @1x, @2x and @3x and 6x in android: ldpi, mdpi, hdpi, xhdpi, xxhdpi) on top of the original asset file.

Using this tool only the original high res asset file needs to be checked in and the other files can be generated as part of the build process.

## Usage

Call this tool on any textfile (recommended file extension: .icogen):

> icogen -i \<input file>

Will read the input file and execute its tasks.

## Input files

Any text base file can be used, there are only 2 valid types of lines:

**Shortcut definitions**

At the very top of the file you can use

> myPathVar = C:\my\very\long\path\that\I\dont\want\to\enter\each\time

you can define as many variables as you want, left side is name, right side is value - seperated by "=" sign.

If any variables are defined, they must be seperated by a line with "---" from the tasks.

**Tasks**

Each line after the "---" (or if no "---" line is found all lines are considered tasks) represents a task and has the form:

> [input file] -> [output path] (settings)

E.g.
> filename.png -> %myPathVar%\logo.png (48x48)

Note that variables are used with surrounding %% and and can only be used in the output file path (right side of the "->" operator).

Both input file and output files are resolved **relative to the textfile** (and not the exe location).

### Settings

Each line must contain at least ("int width"x"int height") as a settings specifying the output file size.

Ideally the size is uniform to the input file (otherwise cropping or downscaling may occur).

Additional (optional) settings are:

* margin (\<int>%)

E.g.

> filename.png -> %myPathVar%\logo.png (48x48, 10%)
> 
> filename.png -> %myPathVar%\logo.png (10%, 48x48)

both create 48x48 pixels sized icon with a 10% margin applied **to each side**.

### Asteriks

Multiple files can be converted at once using the * notation **on both sides**:

> in app icons\*.png -> %myPathVar%\*.png (48x48)

Copies all .pngs from folder "in app icons" to destination "%myPathVar%" and converts each image to be 48x48 pixels.

# Examples

## Android multi res icons:

> in app icons\*.png -> %androidRes%\*.png (32x32)
> in app icons\*.png -> %androidRes%-hdpi\*.png (48x48)
> in app icons\*.png -> %androidRes%-xxhdpi\*.png (96x96)

Creates all in app icons in the respective sizes, allowing android to use auto. scaling.

## Recommendation

Write a "genico.cmd" file or similar that executes all txt files:

```
@echo OFF
REM requires icogen in path

icogen.exe -i icons\android.icogen
icogen.exe -i icons\uwp.icogen
icogen.exe -i icons\iOS.icogen

```

and put the android/iOS and UWP outputs in gitignore like so:

```
# .gitignore excerpt

**/src/**/*.png
iTunesArtwork
iTunesArtwork@2x

```

Finally write the .icogens to look something like this:

Android:
```
androidRes=..\src\<MyApp>.Android\Resources\drawable
---
logo.png -> %androidRes%\logo.png (48x48)
logo.png -> %androidRes%-hdpi\logo.png (72x72)
logo.png -> %androidRes%-xxhdpi\logo.png (144x144)

logo.png -> %androidRes%\splashscreen.png (310x150)
logo.png -> %androidRes%-xhdpi\splashscreen.png (620x300)

in app icons\*.png -> %androidRes%\*.png (32x32)
in app icons\*.png -> %androidRes%-hdpi\*.png (48x48)
in app icons\*.png -> %androidRes%-xxhdpi\*.png (96x96)

```

iOS:
```
iOSRes=..\src\BarcodeScanner.iOS\Resources
iOSPath=..\src\BarcodeScanner.iOS
---
logo.png -> %iOSPath%\iTunesArtwork (512x512)
logo.png -> %iOSPath%\iTunesArtwork@2x (1024x1024)

logo.png -> %iOSRes%\Icon-76.png (76x76)
logo.png -> %iOSRes%\Icon-76@2x.png (152x152)
logo.png -> %iOSRes%\Icon-76@3x.png (228x228)

logo.png -> %iOSRes%\TransparentIcon-76.png (76x76)
logo.png -> %iOSRes%\TransparentIcon-76@2x.png (152x152)
logo.png -> %iOSRes%\TransparentIcon-76@3x.png (228x228)

in app icons\*.png -> %iOSRes%\*.png (30x30)
in app icons\*.png -> %iOSRes%\*@2x.png (60x60)
in app icons\*.png -> %iOSRes%\*@3x.png (90x90)

```

UWP:
```
uwpAssets=..\src\BarcodeScanner.UWP\Assets
uwpPath=..\src\BarcodeScanner.UWP
---
logo.png -> %uwpAssets%\StoreLogo.scale-100.png (50x50, 10%)
logo.png -> %uwpAssets%\StoreLogo.scale-125.png (63x63, 10%)
logo.png -> %uwpAssets%\StoreLogo.scale-150.png (75x75, 10%)
logo.png -> %uwpAssets%\StoreLogo.scale-200.png (100x100, 10%)
logo.png -> %uwpAssets%\StoreLogo.scale-400.png (200x200, 10%)

logo.png -> %uwpAssets%\Square44x44Logo.scale-100.png (44x44, 10%)
logo.png -> %uwpAssets%\Square44x44Logo.scale-125.png (55x55, 10%)
logo.png -> %uwpAssets%\Square44x44Logo.scale-150.png (66x66, 10%)
logo.png -> %uwpAssets%\Square44x44Logo.scale-200.png (88x88, 10%)
logo.png -> %uwpAssets%\Square44x44Logo.scale-400.png (176x176, 10%)

logo.png -> %uwpAssets%\SmallTile.scale-100.png (71x71, 25%)
logo.png -> %uwpAssets%\SmallTile.scale-125.png (89x89, 25%)
logo.png -> %uwpAssets%\SmallTile.scale-150.png (107x107, 25%)
logo.png -> %uwpAssets%\SmallTile.scale-200.png (142x142, 25%)
logo.png -> %uwpAssets%\SmallTile.scale-400.png (284x284, 25%)

logo.png -> %uwpAssets%\Square150x150Logo.scale-100.png (150x150, 25%)
logo.png -> %uwpAssets%\Square150x150Logo.scale-125.png (188x188, 25%)
logo.png -> %uwpAssets%\Square150x150Logo.scale-150.png (225x225, 25%)
logo.png -> %uwpAssets%\Square150x150Logo.scale-200.png (300x300, 25%)
logo.png -> %uwpAssets%\Square150x150Logo.scale-400.png (600x600, 25%)

logo.png -> %uwpAssets%\Square310x310Logo.scale-100.png (310x310, 25%)
logo.png -> %uwpAssets%\Square310x310Logo.scale-125.png (388x388, 25%)
logo.png -> %uwpAssets%\Square310x310Logo.scale-150.png (465x465, 25%)
logo.png -> %uwpAssets%\Square310x310Logo.scale-200.png (620x620, 25%)
logo.png -> %uwpAssets%\Square310x310Logo.scale-400.png (1240x1240, 25%)

logo.png -> %uwpAssets%\Wide310x150Logo.scale-100.png (310x150, 25%)
logo.png -> %uwpAssets%\Wide310x150Logo.scale-125.png (388x188, 25%)
logo.png -> %uwpAssets%\Wide310x150Logo.scale-150.png (465x225, 25%)
logo.png -> %uwpAssets%\Wide310x150Logo.scale-200.png (620x300, 25%)
logo.png -> %uwpAssets%\Wide310x150Logo.scale-400.png (1240x600, 25%)

logo.png -> %uwpAssets%\SplashScreen.scale-100.png (620x300)
logo.png -> %uwpAssets%\SplashScreen.scale-125.png (775x375)
logo.png -> %uwpAssets%\SplashScreen.scale-150.png (930x450)
logo.png -> %uwpAssets%\SplashScreen.scale-200.png (1240x600)
logo.png -> %uwpAssets%\SplashScreen.scale-400.png (2480x1200)

in app icons\*.png -> %uwpPath%\*.png (48x48)

```