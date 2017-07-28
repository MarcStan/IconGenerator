# IconGenerator

Xamarin provides no way of automatically generating all icons in their various sizes and directories.

Some external tools try to fix this but they are incomplete (no UWP support) or require Xamarin studio.

This generator acts as a standalone executable that can generate all icons at once.

# Basics

There are three types of input files:

* App icon
* Splashscreen icon
* In app icons

The App and splashscreen icons should both be transparent except for the actual app logo.

The transparent part will be replaced by the specified app color.

The in app icons should all be single color (preferably black) with transparent backgrounds.

# Platform specifics

## UWP:

App logos and splashscreens need to be scaled at least 100%, 200% and 400%. Additionally they should be scaled 125% and 150%.

In app icons cannot be scaled and should jsut be 48x48.

The full list of required assets can be [found here](https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/tiles-and-notifications-app-assets).

At a minimum these are required:

App logo:

* Medium tile (150x150)
* Wide tale (310x150)
* App icon (44x44)
* Package logo (50x50)

Splashscreen: 620x300

## iOS:

iOS requires @3x for iPhone 7 Plus and iPhone 6s Plus and @2x for all other high-resolution devices.

Valid options are thus: @1x, @2x and @3x.

https://developer.apple.com/ios/human-interface-guidelines/graphics/image-size-and-resolution/

## Android:

The app store icon needs to be 512x512 pixels.

All other icons can be scaled using the various "\<x>dpi" folders.

Specifically app icons should be scaled to:

* ldpi    0.75x (36x36)
* mdpi    1x (48x48)
* hdpi    1.5x (72x72)
* xhdpi   2x (96x96)
* xxhdpi  3x (144x144)
* xxxhdpi 4x (192x192)

In app icons should be scaled slightly smaller at:

* ldpi 24x24
* mdpi 32x32
* hdpi 48x48
* xhdpi 64x64
* xxhdpi 96x96
* xxxhpdi 128x128

ldpi is no longer really required, all modern phones (720p+ screens) are fine with mdpi and bigger.