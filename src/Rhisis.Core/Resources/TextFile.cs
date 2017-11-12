using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// FlyFF Text file.
    /// </summary>
    public class TextFile : IDisposable
    {
        private static readonly string SingleLineComment = "//";
        private static readonly string MultiLineCommentStart = "/*";
        private static readonly string MultiLineCommentEnd = "*/";
        private static readonly IEnumerable<char> Separators = new[] { ' ', '\t' };
        public static readonly IEnumerable<string> Extensions = new[] { ".txt" };

        private readonly string filePath;
        private readonly IDictionary<string, string> _texts;

        /// <summary>
        /// Gets the Texts dictionnary.
        /// </summary>
        public IReadOnlyDictionary<string, string> Texts => this._texts as IReadOnlyDictionary<string, string>;

        /// <summary>
        /// Creates a new <see cref="TextFile"/> instance.
        /// </summary>
        /// <param name="filePath">File path</param>
        public TextFile(string filePath)
        {
            this.filePath = filePath;
            this._texts = new Dictionary<string, string>();
        }

        /// <summary>
        /// Parse the file text.
        /// </summary>
        public void Parse()
        {
            using (var fileStream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (line.StartsWith(SingleLineComment))
                        continue;
                    if (line.StartsWith(MultiLineCommentStart))
                    {
                        while (!line.Contains(MultiLineCommentEnd))
                            line = reader.ReadLine();
                        continue;
                    }
                    if (line.Contains(SingleLineComment))
                        line = line.Remove(line.IndexOf('/'));

                    string[] texts = line.Split(Separators.ToArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (texts.Length >= 2)
                    {
                        string key = texts.First();
                        string value = line.Replace(key, string.Empty).Trim();

                        if (!this._texts.ContainsKey(key))
                            this._texts.Add(key, value);
                    }
                }
            }
        }

        public void Dispose()
        {
            this._texts.Clear();
        }
    }
}
