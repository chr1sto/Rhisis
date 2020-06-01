using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rhisis.Core.IO
{
    public class FileTokenScanner : IDisposable
    {
        public static readonly string SingleLineComment = "//";
        public static readonly string MultiLineCommentBegin = "/*";
        public static readonly string MultiLineCommentEnd = "*/";
        private static readonly char[] SplitCharacters = { '\n', '\r' };

        private readonly string _filePath;
        private readonly string _splitRegex;
        private int _currentTokenIndex;
        private string[] _tokens;
        private string[] _comments;

        /// <summary>
        /// Creates a new <see cref="FileTokenScanner"/> instance.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="splitRegex"></param>
        public FileTokenScanner(string filePath, string splitRegex)
        {
            _currentTokenIndex = 0;
            _filePath = filePath;
            _splitRegex = splitRegex;
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        public void Read()
        {
            _currentTokenIndex = 0;

            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                string fileContent = reader.ReadToEnd();
                string[] splitFileContent = fileContent.Split(SplitCharacters, StringSplitOptions.RemoveEmptyEntries);
                _comments = new string[splitFileContent.Length];

                for (var i = 0; i < splitFileContent.Length; ++i)
                {
                    string line = splitFileContent[i];

                    if (string.IsNullOrEmpty(line) || line.StartsWith(SingleLineComment))
                    {
                        _comments[i] = splitFileContent[i];
                        splitFileContent[i] = string.Empty;
                        continue;
                    }

                    if (line.Contains(SingleLineComment))
                        splitFileContent[i] = line.Remove(line.IndexOf(SingleLineComment, StringComparison.Ordinal)).Trim();

                    if (line.Contains(MultiLineCommentBegin))
                    {
                        // Special case if the end of multi comment is on the same line
                        if (line.Contains(MultiLineCommentEnd))
                        {
                            splitFileContent[i] = line.Remove(line.IndexOf(MultiLineCommentBegin, StringComparison.Ordinal), line.IndexOf(MultiLineCommentEnd, StringComparison.Ordinal)).Trim();
                            continue;
                        }
                        splitFileContent[i] = line.Remove(line.IndexOf(MultiLineCommentBegin, StringComparison.Ordinal)).Trim();
                        while (!splitFileContent[i].Contains(MultiLineCommentEnd))
                        {
                            splitFileContent[i] = string.Empty;
                            i++;
                        }
                        int removeStartIndex = splitFileContent[i].IndexOf(MultiLineCommentEnd, StringComparison.Ordinal) + MultiLineCommentEnd.Length;
                        splitFileContent[i] = splitFileContent[i].Substring(removeStartIndex).Trim();
                    }
                }

                IEnumerable<string> tokens = from x in splitFileContent
                                             where !string.IsNullOrEmpty(x)
                                             select x;
                
                fileContent = string.Join(" ", tokens);

                string[] parts = Regex.Split(fileContent, _splitRegex);

                _tokens = (from x in parts
                                let y = x.Trim()
                                where !string.IsNullOrEmpty(y)
                                select y).ToArray();
            }
        }

        /// <summary>
        /// Gets the token and move on to the next one.
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            return _currentTokenIndex + 1 > _tokens.Count() ? null : _tokens[_currentTokenIndex++];
        }

        /// <summary>
        /// Gets the previous token.
        /// </summary>
        /// <returns></returns>
        public string GetPreviousToken()
        {
            return _currentTokenIndex <= 0 ? null : _tokens[_currentTokenIndex - 2];
        }

        /// <summary>
        /// Check if next token is equal to the token passed as parameter.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool NextTokenIs(string token)
        {
            if (_currentTokenIndex > _tokens.Count())
                return false;

            return string.Equals(_tokens[_currentTokenIndex + 1], token, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if current token is equal to the token passed as parameter.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CurrentTokenIs(string token)
        {
            return _tokens[_currentTokenIndex] == token;
        }

        /// <summary>
        /// Gets the comment at the specified position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string GetCommentAtLine(int position)
        {
            if (position >= 0 && position < _comments.Length)
                return _comments[position];

            return null;
        }

        /// <summary>
        /// Dispose the Token scanner instance.
        /// </summary>
        public void Dispose()
        {
            _tokens = null;
        }
    }
}
