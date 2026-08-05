using System;
using System.Runtime.InteropServices;

namespace Meta.XR.MRUtilityKit.Extensions
{
    public static class GuidExtensions
    {
        /// <summary>
        /// This uses FieldOffset to overlay a Guid on top of a uint.
        /// This lets you get the first 4 bytes of the Guid without allocating a byte array.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private readonly struct GuidOverlay
        {
            [FieldOffset(0)] public readonly Guid guid;

            [FieldOffset(0)] public readonly uint first;

            public GuidOverlay(Guid guid)
            {
                first = 0;
                this.guid = guid;
            }
        }

        public static uint FirstChunk(this Guid guid)
        {
            var overlay = new GuidOverlay(guid);

            return overlay.first;
        }

        /// <summary>
        /// Returns the first N characters of the Guid as a string
        /// Git uses 7 chars to uniquely identify a commit, we use that by default.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string PrefixString(this Guid guid, int chars = 7)
        {
            chars = Math.Clamp(chars, 0, 8);

            if (chars == 0)
            {
                return string.Empty;
            }

            var firstChunk = guid.FirstChunk();
            var prefixString = firstChunk.ToString("x8");
            return chars < 8 ? prefixString.Substring(0, chars) : prefixString;
        }
    }
}
