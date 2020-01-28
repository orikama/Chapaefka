using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Buffers;
using System.Media;

namespace CSharpClient
{
    class WavPlayer
    {
        private SoundPlayer _player = new SoundPlayer();

        public void Test()
        {
            int size = 1323000;
            ArrayPool<byte> sizeAwarePool = ArrayPool<byte>.Create(size, 10);

            byte[] array = sizeAwarePool.Rent(size - 1);
            //pool.Return(array);
        }

        public void Play()
        {
            //playe
            _player.s
        }
    }
}
