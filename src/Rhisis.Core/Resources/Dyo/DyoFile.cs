using Rhisis.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Dyo
{
    public sealed class DyoFile : FileStream, IDisposable
    {
        private readonly ICollection<DyoElement> _elements;

        /// <summary>
        /// Gets the Dyo elements collection.
        /// </summary>
        public IReadOnlyCollection<DyoElement> Elements => _elements as IReadOnlyCollection<DyoElement>;

        /// <summary>
        /// Creates a new <see cref="DyoFile"/> instance.
        /// </summary>
        /// <param name="dyoFilePath">Dyo file path</param>
        public DyoFile(string dyoFilePath)
            : base(dyoFilePath, FileMode.Open, FileAccess.Read)
        {
            _elements = new List<DyoElement>();
            var memoryReader = new BinaryReader(this);

            while (memoryReader.BaseStream.Position < memoryReader.BaseStream.Length)
            {
                DyoElement rgnElement = null;
                uint type = memoryReader.ReadUInt32();

                switch (type)
                {
                    case (int)WorldObjectType.Control:
                        rgnElement = new CommonControlDyoElement();
                        break;
                    case (int)WorldObjectType.Mover:
                        rgnElement = new NpcDyoElement();
                        break;
                    case (int)WorldObjectType.Object:
                    case (int)WorldObjectType.Item:
                    case (int)WorldObjectType.Ship:
                        rgnElement = new DyoElement();
                        break;
                }

                if (rgnElement == null)
                    break;

                rgnElement.ElementType = (int)type;
                rgnElement.Read(memoryReader);
                _elements.Add(rgnElement);
            }
        }
        
        /// <summary>
        /// Gets the specific elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetElements<T>() where T : DyoElement => _elements.Where(x => x is T).Select(x => x as T);

        /// <summary>
        /// Dispose the <see cref="DyoFile"/> resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _elements.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
