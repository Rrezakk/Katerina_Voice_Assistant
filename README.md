![K A T E R I N A](https://user-images.githubusercontent.com/52713544/144716556-29ce4add-e1b7-4893-80a6-6fcceb361da9.png)

## This is an open-sourse voice assistant called KATERINA (K3NA)! 

> ## The mission of the project is to provide the community with a voice assistant with flexible customization

## Technologies:

- Yandex SpeechKit for speech recognition and synthesis 
- NAudio .Net library for catching audio from microphone
- Concentus (Opus) .Net for compressing audio
- Some mathematic and logical objects

## How to start?

1. Download repository

2. Launch solution (K3NA_Remastered_2.sln file)

3. In folder called "Configuration" create two files: 
   ![image-20211204193147500](https://user-images.githubusercontent.com/52713544/144717484-02df05ad-bb81-4173-a0d8-a49dbe7c16aa.png)


4. **defaultConfig.env**
```
FolderId=Your Yandex cloud folder id
OauthToken=Your token Yandex cloud 
OAuthTolenIamAPIKey=default
```
get oauth : https://oauth.yandex.ru/authorize?response_type=token
5. To get keys, google... now sorry, didn't explain

6. **srmConfig.env**
```
SampleRate=48000
BitResolution=16
Channels=1
WaveInputBuffer=100
SoundDevice=0
Treshold=1400
PreRecordBytes=4800
AudistEmaAlpha=0.9
DefaultEmaAlpha=0.3
NoiseMeterAlpha=0.08
```

7. Then build and run the project!

## There's a way to create "protocols":

1. Go to **Core\StandardProtocols** directory
2. Create file with name of protocol for better experience
3. Fill the file according to syntax

## Syntax will be described here later:

You can get examples in HelloWorld.txt file in **Core\StandardProtocols** directory

<...>

