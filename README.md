# Chapaefka

This is my implementation of Text To Speech service, which is meant to be used by streamers for voicing donations.

## TTS Server
Is a simple python [server scirpt](https://github.com/orikama/TTSServer.git) that receives text, converts it to audio and send it to TTS Client. It's based on [nvidia tacotron2 implementation](https://github.com/NVIDIA/tacotron2)

## TTS Client
C# client is a windows application that:
1. Listens for donation events from Streamlabs websocket and adds them to the queue.
2. Sends messages of donations to TTS Server.
3. Converts raw bytes of audio recieved from TTS Server to wav and plays it.
