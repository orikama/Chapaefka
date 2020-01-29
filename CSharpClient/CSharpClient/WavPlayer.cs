using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Media;

using System.Runtime.InteropServices;

namespace CSharpClient.WavPlayer
{
    public class WavPlayer
    {
        public const int WavHeaderSize = 44;
        private WavHeader _wavHeader;

        private readonly SoundPlayer _player = new SoundPlayer();
        private CancellationTokenSource _cts = null;

        private bool _isPlaying = false;

        public bool IsPlaying => _isPlaying;

        public WavPlayer()
        {
            // TTS produce 22050Hz, 16 bit, mono wav audio
            const uint sampleRate = 22050u;
            const ushort bitsPerSample = 16;
            const ushort numChannels = 1;

            //_wavHeader.chunkSize - Depends on wav file size
            //_wavHeader.subchunk1Size = 16u;
            //_wavHeader.audioFormat = 1;
            _wavHeader.numChannels = numChannels;
            _wavHeader.sampleRate = sampleRate;
            _wavHeader.byteRate = sampleRate * numChannels * bitsPerSample / 8;
            _wavHeader.blockAlign = numChannels * bitsPerSample / 8;
            _wavHeader.bitsPerSample = bitsPerSample;
            //_wavHeader.subchunk2Id = 0x64617461;
            //_wavHeader.subchunk2Size - Depends on wav file size
        }

        public async Task PlayAsync(byte[] wav)
        {
            int wavDurationMs = InsertWavHeader(wav);

            using(_cts = new CancellationTokenSource())
            using(MemoryStream wavStream = new MemoryStream(wav))
            {
                _player.Stream = wavStream;
                _isPlaying = true;
                _player.Play();

                await Task.Delay(wavDurationMs, _cts.Token).ConfigureAwait(false);

                if(_cts.IsCancellationRequested)
                    _player.Stop();

                _isPlaying = false;
            }
        }

        public void Stop() => _cts.Cancel();

        private int InsertWavHeader(byte[] wav)
        {
            uint wavRawLength = (uint)wav.Length - 8;
            _wavHeader.chunkSize = wavRawLength;
            wavRawLength -= 36;
            _wavHeader.subchunk2Size = wavRawLength;

            IntPtr ptr = IntPtr.Zero;
            ptr = Marshal.AllocHGlobal(WavHeaderSize);
            Marshal.StructureToPtr(_wavHeader, ptr, true);
            Marshal.Copy(ptr, wav, 0, WavHeaderSize);
            Marshal.FreeHGlobal(ptr);

            return (int)(_wavHeader.subchunk2Size * 1000 / _wavHeader.byteRate); // don't know if i should compute it with floating point
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 44)]
    internal struct WavHeader
    {
        public const UInt32 chunkId = 0x52494646;       // "RIFF" - 0x52494646
        public UInt32 chunkSize;                        // fileSize minus size of ChunkId and ChunkSize => fileSize - 8 bytes
        public const UInt32 format = 0x57415645;        // "WAVE" - 0x57415645
        public const UInt32 subchunk1Id = 0x666d7420;   // "fmt " - 0x666d7420
        public const UInt32 subchunk1Size = 16u;        // 16 for PCM format
        public const UInt16 audioFormat = 1;            // 1 for PCM format
        public UInt16 numChannels;                      // mono=1, stereo=2 etc.
        public UInt32 sampleRate;                       // 22050Hz, 44100Hz etc.
        public UInt32 byteRate;                         // sampleRate * numChannels * bitsPerSample/8
        public UInt16 blockAlign;                       // numChannels * bitsPerSample/8
        public UInt16 bitsPerSample;                    // sound depth - 8 bit, 16 bit, etc.
        public const UInt32 subchunk2Id = 0x64617461;   // "data" - 0x64617461
        public UInt32 subchunk2Size;                    // file size - 44 bytes header
    }
}
