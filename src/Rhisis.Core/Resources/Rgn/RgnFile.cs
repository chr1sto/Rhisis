using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources
{
    public sealed class RgnFile : FileStream, IDisposable
    {
        private readonly ICollection<RgnElement> _elements;

        /// <summary>
        /// Gets the region elements collection.
        /// </summary>
        public IReadOnlyCollection<RgnElement> Elements => _elements as IReadOnlyCollection<RgnElement>;

        /// <summary>
        /// Creates a new <see cref="RgnFile"/> instance.
        /// </summary>
        /// <param name="filePath"></param>
        public RgnFile(string filePath)
            : base(filePath, FileMode.Open, FileAccess.Read)
        {
            _elements = new List<RgnElement>();
            Read();
        }

        /// <summary>
        /// Gets a list of <typeparamref name="T"/> that inherits from <see cref="RgnElement"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetElements<T>() where T : RgnElement
        {
            return from x in _elements
                   where x is T
                   select x as T;
        }

        /// <summary>
        /// Reads the <see cref="RgnFile"/> contents.
        /// </summary>
        private void Read()
        {
            var reader = new StreamReader(this);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                    continue;

                string[] data = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (line.StartsWith("respawn7"))
                {
                    if (data.Length < 24)
                        continue;

                    _elements.Add(new RgnRespawn7(data));
                }

                if (line.StartsWith("region3"))
                {
                    if (data.Length < 32)
                        continue;

                    _elements.Add(new RgnRegion3(data));
                }

                // add more...
            }
        }

        /// <inheritdoc />
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
