using Rhisis.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Represents a C/C++ define file.
    /// </summary>
    public sealed class DefineFile : IDisposable
    {
        public static readonly IEnumerable<string> Extensions = new[] { ".h", ".hh", ".hpp" };
        private static readonly string DwordCast = "(DWORD)";
        private static readonly string WordCast = "(WORD)";
        private static readonly string ByteCast = "(BYTE)";

        private readonly FileTokenScanner _scanner;
        private readonly IDictionary<string, object> _defines;

        /// <summary>
        /// Gets the value of a define directive.
        /// </summary>
        /// <param name="define"></param>
        /// <returns></returns>
        public object this[string define] => this.GetValue<object>(define);

        /// <summary>
        /// Gets the define directive.
        /// </summary>
        public IReadOnlyDictionary<string, object> Defines => this._defines as IReadOnlyDictionary<string, object>;

        /// <summary>
        /// Gets the number of definitions.
        /// </summary>
        public int Count => this._defines.Count;

        /// <summary>
        /// Creates a new DefineFile instance.
        /// </summary>
        /// <param name="filePath">Define file path</param>
        public DefineFile(string filePath)
        {
            this._scanner = new FileTokenScanner(filePath, @"([\t# ])");
            this._defines = new Dictionary<string, object>();

            this.Read();
        }

        /// <summary>
        /// Get the value of a define key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defineKey"></param>
        /// <returns></returns>
        public T GetValue<T>(string defineKey)
        {
            if (this._defines.TryGetValue(defineKey, out object value))
                return (T)Convert.ChangeType(value, typeof(T));

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Reads the define file.
        /// </summary>
        private void Read()
        {
            this._scanner.Read();

            string token = null;
            while ((token = this._scanner.GetToken()) != null)
            {
                if (token == "#" && this._scanner.GetToken() == "define")
                {
                    string defineName = this._scanner.GetToken();

                    if (this._scanner.CurrentTokenIs("#"))
                        continue;

                    var defineValueToken = this._scanner.GetToken();

                    if (defineValueToken is null)
                        continue;

                    object defineValue = this.ParseDefineValue(defineValueToken);

                    if (this._defines.ContainsKey(defineName))
                        this._defines[defineName] = defineValue;
                    else
                        this._defines.Add(defineName, defineValue);
                }
            }
        }

        /// <summary>
        /// Dispose the define file resources.
        /// </summary>
        public void Dispose()
        {
            if (this._defines.Any())
                this._defines.Clear();
            this._scanner.Dispose();
        }

        /// <summary>
        /// Parse the define value.
        /// </summary>
        /// <param name="defineValue">string define value</param>
        /// <returns></returns>
        private object ParseDefineValue(string defineValue)
        {
            object newDefineValue = null;

            try
            {
                if (this._defines.ContainsKey(defineValue))
                {
                    newDefineValue = this._defines[defineValue];
                }
                else if (defineValue.StartsWith(DwordCast, StringComparison.OrdinalIgnoreCase))
                {
                    newDefineValue = Convert.ToUInt32(defineValue.Replace(DwordCast, string.Empty), defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.StartsWith(WordCast, StringComparison.OrdinalIgnoreCase))
                {
                    newDefineValue = Convert.ToUInt16(defineValue.Replace(WordCast, string.Empty), defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.StartsWith(ByteCast, StringComparison.OrdinalIgnoreCase))
                {
                    newDefineValue = Convert.ToByte(defineValue.Replace(ByteCast, string.Empty), defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.EndsWith("L", StringComparison.OrdinalIgnoreCase))
                {
                    newDefineValue = Convert.ToInt64(defineValue.Replace("L", string.Empty), defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.StartsWith("\"") && defineValue.EndsWith("\""))
                {
                    newDefineValue = defineValue.Trim('"');
                }
                else
                {
                    newDefineValue = Convert.ToInt32(defineValue, defineValue.StartsWith("0x") ? 16 : 10);
                }
            }
            catch
            {
                newDefineValue = 0;
            }

            return newDefineValue;
        }
    }
}
