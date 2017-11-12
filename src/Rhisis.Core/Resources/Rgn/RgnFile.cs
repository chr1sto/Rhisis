using System;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources
{
    public sealed class RgnFile : FileStream, IDisposable
    {
        private readonly ICollection<RgnElement> _elements;

        /// <summary>
        /// Gets the region elements collection.
        /// </summary>
        public IReadOnlyCollection<RgnElement> Elements => this._elements as IReadOnlyCollection<RgnElement>;

        /// <summary>
        /// Creates a new <see cref="RgnFile"/> instance.
        /// </summary>
        /// <param name="filePath"></param>
        public RgnFile(string filePath)
            : base(filePath, FileMode.Open, FileAccess.Read)
        {
            this._elements = new List<RgnElement>();
            this.Read();
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

                    this._elements.Add(new RgnRespawn7(data));
                }

                if (line.StartsWith("region3"))
                {
                    if (data.Length < 32)
                        continue;

                    this._elements.Add(new RgnRegion3(data));
                }

                // add more...
            }
        }

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
