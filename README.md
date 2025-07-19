[![GitHub version](https://img.shields.io/github/v/release/Kurtnoise-zeus/megui)](https://github.com/Kurtnoise-zeus/megui/)
![MeGUI number of downloads](https://img.shields.io/github/downloads/kurtnoise-zeus/megui/latest/total.svg)
[![download latest release](https://img.shields.io/badge/Megui-download-green?style=flat)](https://github.com/Kurtnoise-zeus/megui/releases/latest) 


# MeGUI 6666 version

During summer 2022, I upgraded the [current megui codebase](https://sourceforge.net/p/megui/code/HEAD/tree/megui/trunk/) in order to fix some issues and add some new features. 

The current branch has been **tested only on x64 dev environment** (Avisynth+, Windows 11 Pro Edition).


## Compilation

Just open the MeGUI.sln file using Visual Studio. I'm using VS2022 Community Edition in order to compile the latest changes.

## What's new

Nothing really new comparing to the official release except :

- Audio transcoding in order to use channel mask values. This requires avisynth+ 3.7.3 or higher.
- Exhale Audio Encoder (https://gitlab.com/ecodis/exhale)
- FFmpeg DCA/EAC3/THD Audio Encoders (https://ffmpeg.org/)
- BestSource avisynth support (https://github.com/vapoursynth/bestsource/releases)
- FFV1 Lossless Video Encoder (https://ffmpeg.org/)
- SVT-AV1-PSY Video Encoder (https://github.com/BlueSwordM/svt-av1-psyex) 

## Release

From time to time, I upload directly on [github Release page](https://github.com/Kurtnoise-zeus/megui/releases) a package having latest changes and/or fixes. Use it at your own risk.

## Todo

- [ ] Add VVC Video Encoder (dunno which one yet)
- [ ] Add Bitrate Video Viewer
- [ ] Add Demuxers tool
- [ ] ...

## Repository Activity

![Repository Activities](https://repobeats.axiom.co/api/embed/c013ca5965300e42e4021ac7fb736d5b5cc9ed58.svg "Repobeats analytics image")
