using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.Utils.Shuffle
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<SongDTO> Shuffle<SongDTO>(this IEnumerable<SongDTO> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<SongDTO> Shuffle<SongDTO>(this IEnumerable<SongDTO> source, Random random)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            return source.ShuffleIterator(random);
        }

        private static IEnumerable<SongDTO> ShuffleIterator<SongDTO>(
            this IEnumerable<SongDTO> source, Random random)
        {
            var buffer = source.ToList();
            for (var i = 0; i < buffer.Count; i++)
            {
                var j = random.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}