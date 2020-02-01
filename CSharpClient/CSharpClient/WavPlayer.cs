using System;
using System.Threading;
using System.Threading.Tasks;
using System.Media;
using System.Runtime.InteropServices;

#if DEBUG
using System.IO;
#endif

namespace CSharpClient.WavPlayer
{
    public class WavPlayer
    {
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct WavHeader
        {
            public UInt32 chunkId;          // "RIFF" - 0x52494646
            public UInt32 chunkSize;        // fileSize minus size of ChunkId and ChunkSize => fileSize - 8 bytes
            public UInt32 format;           // "WAVE" - 0x57415645
            public UInt32 subchunk1Id;      // "fmt " - 0x666d7420
            public UInt32 subchunk1Size;    // 16 for PCM format
            public UInt16 audioFormat;      // 1 for PCM format
            public UInt16 numChannels;      // mono=1, stereo=2 etc.
            public UInt32 sampleRate;       // 22050Hz, 44100Hz etc.
            public UInt32 byteRate;         // sampleRate * numChannels * bitsPerSample/8
            public UInt16 blockAlign;       // numChannels * bitsPerSample/8
            public UInt16 bitsPerSample;    // sound depth - 8 bit, 16 bit, etc.
            public UInt32 subchunk2Id;      // "data" - 0x64617461
            public UInt32 subchunk2Size;    // file size - 44 bytes header
        }

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

            _wavHeader.chunkId = 0x4646_4952;
            //_wavHeader.chunkSize - Depends on wav file size
            _wavHeader.format = 0x4556_4157;
            _wavHeader.subchunk1Id = 0x2074_6d66;
            _wavHeader.subchunk1Size = 16;
            _wavHeader.audioFormat = 1;
            _wavHeader.numChannels = numChannels;
            _wavHeader.sampleRate = sampleRate;
            _wavHeader.byteRate = sampleRate * numChannels * bitsPerSample / 8;
            _wavHeader.blockAlign = numChannels * bitsPerSample / 8;
            _wavHeader.bitsPerSample = bitsPerSample;
            _wavHeader.subchunk2Id = 0x6174_6164;
            //_wavHeader.subchunk2Size - Depends on wav file size
        }

        public async Task PlayAsync(Tuple<int, byte[]> wav)
        {
            if (wav == null)
                throw new ArgumentNullException("WavPlayer.PlayAsync rawWav bytes is null");

            int wavDurationMs = InsertWavHeader(wav);

            using (_cts = new CancellationTokenSource())
            using (MemoryStream wavStream = new MemoryStream(wav.Item2))
            {
#if DEBUG
                using (BinaryWriter sw = new BinaryWriter(File.OpenWrite("kek.wav")))
                    sw.Write(wav.Item2, 0, wav.Item1);
#endif

                _player.Stream = wavStream;
                _isPlaying = true;
                _player.Play();

                await Task.Delay(wavDurationMs, _cts.Token).ConfigureAwait(false);

                if (_cts.IsCancellationRequested)
                    _player.Stop();

                _isPlaying = false;
            }
        }

        public void Stop() => _cts.Cancel();

        private int InsertWavHeader(Tuple<int, byte[]> rawWav)
        {
            uint wavRawLength = (uint)rawWav.Item1 - 8;
            _wavHeader.chunkSize = wavRawLength;
            wavRawLength -= 36;
            _wavHeader.subchunk2Size = wavRawLength;

            IntPtr ptr = IntPtr.Zero;
            ptr = Marshal.AllocHGlobal(WavHeaderSize);
            Marshal.StructureToPtr(_wavHeader, ptr, true);
            Marshal.Copy(ptr, rawWav.Item2, 0, WavHeaderSize);
            Marshal.FreeHGlobal(ptr);

            return (int)(_wavHeader.subchunk2Size * 1000 / _wavHeader.byteRate); // don't know if i should cast it to floating point
        }

    }

}
