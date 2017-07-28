# Icon generator for all Xamarin mobile platforms

Generates app logos, splashscreens and in app icons for
* UWP
* iOS
* Android

Android and iOS get automatic icon scaling (via their resource naming xhdpi and @2x).

UWP doesn't support scaling of in app icons yet (only splashscreen and logo).

## Usage

Call this tool on any textfile:

> icogen -i <input file>

Will read the input file and execute its tasks.

## Input files

Any text base file can be used, there are only 2 valid types of lines:

**Shortcut definitions**

At the very top of the file you can use

> myPathVar = C:\my\very\long\path\that\I\dont\want\to\enter\each\time

you can define as many variables as you want, left side is name, right side is value

If any variables are defined, they must be seperated by a line with "---" from the tasks.

**Tasks**

Each line afterwards represents a task and has the form:

> [input file] -> [output path] (settings)

E.g.
> filename.png -> %myPathVar%\logo.png (48x48)

Note that variables are used with surrounding %% and and can only be used in the output file path.

Both input file and output files are resolved **relative to the textfile** (and not the exe location).

### Settings

Each line must contain at least ("int width"x"int height") as a settings specifying the output file size.

Ideally the size is uniform to the input file (otherwise cropping or downscaling may occur).

Additional (optional) settings are:

* margin (<int>%)

E.g.

> filename.png -> %myPathVar%\logo.png (48x48, 10%)
> filename.png -> %myPathVar%\logo.png (10%, 48x48)

both create 48x48 pixels sized icon with a 10% margin applied **to each side**.

### Asteriks

Multiple files can be converted at once using the * notation *on both sides*:

> in app icons\*.png -> %myPathVar%\*.png (48x48)

# Examples

## Android multi res icons:

> in app icons\*.png -> %androidRes%\*.png (32x32)
> in app icons\*.png -> %androidRes%-hdpi\*.png (48x48)
> in app icons\*.png -> %androidRes%-xxhdpi\*.png (96x96)