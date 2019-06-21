using Rhisis.Core.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Represents the FlyFF Resource table parser (files such as propItem.txt, propMover.txt, etc...).
    /// </summary>
    public class ResourceTableFile : FileStream, IDisposable
    {
        private const string UndefinedValue = "NULL";
        private static readonly char[] DefaultSeparator = new char[] { '\t' };
        private readonly IDictionary<string, int> _defines;
        private readonly IDictionary<string, string> _texts;
        private readonly IList<string> _headers;
        private readonly IList<IEnumerable<string>> _datas;
        private readonly StreamReader _reader;

        private readonly int _headerLineIndex;
        private readonly bool _ignoreHeader;
        private readonly char[] _separators;

        /// <summary>
        /// Gets the amount of valid data within the <see cref="ResourceTableFile"/>.
        /// </summary>
        public int Count => this._datas.Count;

        /// <summary>
        /// Creates a new <see cref="ResourceTableFile"/> instance.
        /// </summary>
        /// <param name="path">Resource path</param>
        public ResourceTableFile(string path)
            : this(path, 0, null, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTableFile"/> instance.
        /// </summary>
        /// <param name="path">Resource path</param>
        /// <param name="headerIndex">Header index in file</param>
        public ResourceTableFile(string path, int headerIndex)
            : this(path, headerIndex, null, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTableFile"/> instance.
        /// </summary>
        /// <param name="path">Resource file path</param>
        /// <param name="headerLineIndex">Header index line in file.</param>
        public ResourceTableFile(string path, int headerLineIndex, char[] separators)
            : this(path, headerLineIndex, separators, null, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTableFile"/> instance.
        /// </summary>
        /// <param name="path">Resource file path</param>
        /// <param name="headerLineIndex">Header index line in file.</param>
        /// <param name="defines">Defines used to transform.</param>
        /// <param name="texts">Texts used to transform.</param>
        public ResourceTableFile(string path, int headerLineIndex, IDictionary<string, int> defines, IDictionary<string, string> texts)
            : this(path, headerLineIndex, DefaultSeparator, defines, texts)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTableFile"/> instance.
        /// </summary>
        /// <param name="path">Resource file path</param>
        /// <param name="headerLineIndex">Header index line in file.</param>
        /// <param name="separators">File separators</param>
        /// <param name="defines">Defines used to transform.</param>
        /// <param name="texts">Texts used to transform.</param>
        public ResourceTableFile(string path, int headerLineIndex, char[] separators, IDictionary<string, int> defines, IDictionary<string, string> texts)
            : base(path, FileMode.Open, FileAccess.Read)
        {
            this._datas = new List<IEnumerable<string>>();
            this._reader = new StreamReader(this);
            this._headerLineIndex = headerLineIndex;
            this._separators = separators;
            this._defines = defines;
            this._texts = texts;
            this._ignoreHeader = this._headerLineIndex < 0;

            if (!this._ignoreHeader)
                this._headers = this.ReadHeader();
            
            this.ReadContent();
        }

        /// <summary>
        /// Gets the list of all records mapped for the type passed as template parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetRecords<T>() where T : class, new()
        {
            var records = new List<T>();
            var typeProperties = this.GetPropertiesWithDataMemberAttribute<T>();

            foreach (var record in this._datas)
            {
                var obj = (T)Activator.CreateInstance(typeof(T));

                foreach (var property in typeProperties)
                {
                    DataMemberAttribute attribute = this.GetPropertyAttribute<DataMemberAttribute>(property);
                    DefaultValueAttribute defaultValue = this.GetPropertyAttribute<DefaultValueAttribute>(property);
                    int index = -1;
                    
                    if (attribute != null)
                    {
                        if (!string.IsNullOrEmpty(attribute.Name) && !this._ignoreHeader)
                            index = this._headers.IndexOf(attribute.Name);
                        else
                            index = attribute.Order;
                    }

                    if (index != -1)
                    {
                        object value = record.ElementAt(index);

                        if (value.ToString() == UndefinedValue)
                        {
                            value = defaultValue != null ? defaultValue.Value : this.GetTypeDefaultValue(property.PropertyType);
                        }
                        else
                        {
                            if (property.PropertyType.BaseType == typeof(Enum))
                                value = Enum.ToObject(property.PropertyType, Convert.ToInt32(value));

                            if (property.PropertyType == typeof(bool))
                                value = Convert.ToBoolean(Convert.ToInt32(value));
                        }

                        property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
                    }
                }

                records.Add(obj);
            }

            return records;
        }

        /// <summary>
        /// Reads and returns a list with the <see cref="ResourceTableFile"/> headers.
        /// </summary>
        /// <returns></returns>
        private IList<string> ReadHeader()
        {
            for (int i = 0; i < this._headerLineIndex; i++)
                this._reader.ReadLine();

            return this._reader.ReadLine()
                        .Replace("/", string.Empty)
                        .Split(this._separators, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
        }

        /// <summary>
        /// Reads the content of the <see cref="ResourceTableFile"/>.
        /// </summary>
        private void ReadContent()
        {
            while (!this._reader.EndOfStream)
            {
                string line = this._reader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !line.StartsWith(FileTokenScanner.SingleLineComment))
                {
                    if (line.Contains(FileTokenScanner.SingleLineComment))
                        line = line.Remove(line.IndexOf(FileTokenScanner.SingleLineComment));

                    line = line.Replace(",,", ",=,").Replace(",", "\t");
                    string[] content = line.Split(new[] { '\t', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (!this._ignoreHeader)
                    {
                        if (content.Length == this._headers.Count)
                        {
                            for (int i = 0; i < content.Length; i++)
                                content[i] = this.Transform(content[i]);

                            this._datas.Add(content);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < content.Length; i++)
                            content[i] = this.Transform(content[i]);

                        this._datas.Add(content);
                    }
                }
            }
        }

        /// <summary>
        /// Check if the data passed as parameter exists in the defines or texts dictionnary.
        /// If it exists replace the data by the first occurence, else transform it.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string Transform(string data)
        {
            if (this._defines != null && this._defines.ContainsKey(data))
                return this._defines[data].ToString();
            else if (this._texts != null && this._texts.ContainsKey(data))
                return this._texts[data];

            return data
                .Replace("=", UndefinedValue)
                .Replace(",", ".")
                .Replace("\"", string.Empty);
        }

        /// <summary>
        /// Gets the properties of the type passed as template parameter that has the custom attribute <see cref="DataMemberAttribute"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetPropertiesWithDataMemberAttribute<T>()
        {
            return from x in typeof(T).GetProperties()
                   where x.GetCustomAttribute(typeof(DataMemberAttribute)) != null
                   select x;
        }

        /// <summary>
        /// Gets the generic attribute value from the given property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private T GetPropertyAttribute<T>(PropertyInfo property) where T : Attribute 
            => property.GetCustomAttribute(typeof(T)) as T;

        /// <summary>
        /// Gets the type default value.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetTypeDefaultValue(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._headers?.Clear();
                this._datas?.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
