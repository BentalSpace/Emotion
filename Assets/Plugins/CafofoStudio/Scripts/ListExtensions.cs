using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CafofoStudio
{
    public static class ListExtensions {

        public static void Shuffle<T>(this List<T> list)
        {
            var count = list.Count;
            var size = count - 1;
            for (var i = 0; i < size; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }
    }
}
