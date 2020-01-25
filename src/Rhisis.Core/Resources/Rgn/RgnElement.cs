using Rhisis.Core.Structures;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Command region element class.
    /// </summary>
    public class RgnElement
    {
        public int Type { get; protected set; }

        public Vector3 Position { get; protected set; }

        public int Left { get; protected set; }

        public int Top { get; protected set; }

        public int Right { get; protected set; }

        public int Bottom { get; protected set; }

        public int Width => Right - Left;

        public int Length => Bottom - Top;
    }
}
