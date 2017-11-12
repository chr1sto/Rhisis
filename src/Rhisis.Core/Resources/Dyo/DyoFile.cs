using System;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Dyo
{
    public sealed class DyoFile : FileStream, IDisposable
    {
        private readonly ICollection<DyoElement> _elements;

        /// <summary>
        /// Gets the Dyo elements collection.
        /// </summary>
        public IReadOnlyCollection<DyoElement> Elements => this._elements as IReadOnlyCollection<DyoElement>;

        /// <summary>
        /// Creates a new <see cref="DyoFile"/> instance.
        /// </summary>
        /// <param name="dyoFilePath">Dyo file path</param>
        public DyoFile(string dyoFilePath)
            : base(dyoFilePath, FileMode.Open, FileAccess.Read)
        {
            this._elements = new List<DyoElement>();
            var memoryReader = new BinaryReader(this);

            while (true)
            {
                DyoElement rgnElement = null;
                int type = memoryReader.ReadInt32();

                if (type == -1)
                    break;

                if (type == 5)
                {
                    rgnElement = new NpcDyoElement();
                    rgnElement.Read(memoryReader);
                }

                if (rgnElement != null)
                    this._elements.Add(rgnElement);
            }
        }

        /// <summary>
        /// Dispose the <see cref="DyoFile"/> resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._elements.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
