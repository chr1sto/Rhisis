using Rhisis.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// FlyFF Text file.
    /// </summary>
    public class TextFile : FileStream, IDisposable
    {
        private static readonly IEnumerable<char> Separators = new[] { ' ', '\t' };
        public static readonly IEnumerable<string> Extensions = new[] { ".txt" };
        
        private readonly IDictionary<string, string> _texts;

        /// <summary>
        /// Gets the Texts dictionnary.
        /// </summary>
        public IReadOnlyDictionary<string, string> Texts => this._texts as IReadOnlyDictionary<string, string>;

        /// <summary>
        /// Gets the text by his key.
        /// </summary>
        /// <param name="key">Text key</param>
        /// <returns></returns>
        public string this[string key] => this.GetText(key);

        /// <summary>
        /// Gets the number of texts in the <see cref="TextFile"/>.
        /// </summary>
        public int Count => this._texts.Count;

        /// <summary>
        /// Creates a new <see cref="TextFile"/> instance.
        /// </summary>
        /// <param name="filePath">File path</param>
        public TextFile(string filePath)
            : base(filePath, FileMode.Open, FileAccess.Read)
        {
            this._texts = new Dictionary<string, string>();
            this.Read();
        }

        /// <summary>
        /// Gets the text by the key.
        /// </summary>
        /// <param name="key">Text key</param>
        /// <returns></returns>
        public string GetText(string key)
        {
            if (this._texts.TryGetValue(key, out string value))
                return value;

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Parse the file text.
        /// </summary>
        private void Read()
        {
            var reader = new StreamReader(this);
            var separators = Separators.ToArray();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string lineTrim = line.Trim();

                if (lineTrim.StartsWith(FileTokenScanner.SingleLineComment))
                    continue;
                if (lineTrim.StartsWith(FileTokenScanner.MultiLineCommentBegin))
                {
                    while (!line.Contains(FileTokenScanner.MultiLineCommentEnd))
                        line = reader.ReadLine();
                    continue;
                }
                if (line.Contains(FileTokenScanner.SingleLineComment))
                    line = line.Remove(line.IndexOf(FileTokenScanner.SingleLineComment));

                string[] texts = line.Split(separators, StringSplitOptions.None);

                if (texts.Length >= 2)
                {
                    string key = texts.First().Trim();
                    
                    if (!string.IsNullOrEmpty(key) && !this._texts.ContainsKey(key))
                        this._texts.Add(key, line.Replace(key, string.Empty).Trim());
                }
            }
        }

        /// <summary>
        /// Dispose the TextFile resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            this._texts.Clear();
            base.Dispose(disposing);
        }
    }
}
