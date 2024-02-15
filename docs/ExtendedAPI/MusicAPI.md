# Music API
## Playing Custom Music
```cs
# Filename is "Tomorrow.wav"
PlayCustomMusic("Tomorrow")
```
`PlayCustomMusic(string filename)`

Works similar to `PlaySFX`, but allows you to play an audio file that will loop. Intended for music.

Place any music files you'd like to use (must be WAV!) in the `Audio/` folder of your campaign, similar to SFX.

## Resetting BGM
```cs
ResetBGM()
```
`ResetBGM()`

This allows you to set the music back to the default music for the network (aka `TENSION`).