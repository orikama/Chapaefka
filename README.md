# Chapaefka

This is my implementation of Text To Speech service, which is meant to be used by streamers for voicing donations.

## TTS Server
Is a simple python [server scirpt](https://github.com/orikama/TTSServer.git) that receives text, converts it to audio and send it to TTS Client. It's based on [nvidia tacotron2 implementation](https://github.com/NVIDIA/tacotron2)

## TTS Client
C# client is a windows forms application that:
1. Listens for donation events from Streamlabs websocket and adds them to the queue.
2. Sends messages of donations to TTS Server.
3. Converts raw bytes of audio recieved from TTS Server to wav and plays it.

It depends on following NuGet packages:
* SocketIoClientDotNet - for communication with Streamlabs [Socket.IO API](https://dev.streamlabs.com/docs/socket-api)
* Newtonsoft.Json - i wanted to pick a library with better perfomance, but since SocketIoClientDotNet depends on it, decided not to

And till i move this project from .Net Framework to .Net Core it depends on:
* System.Threading.Channels
* System.Buffers - for ArrayPool

Application interface (wip)

![alt text](https://github.com/orikama/Chapaefka/blob/master/app.png "Current application interface")
