# MeGUI 6666 version

During summer 2022, I upgraded the [current megui codebase](https://sourceforge.net/p/megui/code/HEAD/tree/megui/trunk/) in order to fix some issues and add some new features. 

The current branch has been **tested only on x64 dev environment** (Avisynth+, Windows 11 Pro Edition).


## Compilation

Just open the MeGUI.sln file using Visual Studio. I'm using VS2022 Community Edition in order to compile the latest changes.

## What's new

Nothing really new comparing to the official release except :
- Audio transcoding in order to use channel mask values. This requires avisynth+ 3.7.3 or higher.

## Release

From time to time, I upload directly on [github Release page](https://github.com/Kurtnoise-zeus/megui/releases) a package having latest changes and/or fixes. Use it at your own risk.

## Todo

- [ ] Add AV1 Video Encoder (probably [svt-av1-psy](https://github.com/gianni-rosato/svt-av1-psy))
- [ ] Add VVC Video Encoder (dunno which one yet)
- [ ] Add Exhale Audio Encoder (https://gitlab.com/ecodis/exhale)
- [ ] Add Bitrate Video Viewer
- [ ] Add Demuxers tool
- [ ] ...


