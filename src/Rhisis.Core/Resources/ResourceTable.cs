using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Represents the FlyFF Resource table parser (files such as propItem.txt, propMover.txt, etc...)
    /// </summary>
    public class ResourceTable : FileStream, IDisposable
    {
        private readonly IDictionary<string, int> _defines;
        private readonly IDictionary<string, string> _texts;
        private readonly IList<string> _headers;
        private readonly IList<IEnumerable<string>> _datas;
        private readonly StreamReader _reader;

        private int _headerIndex;

        /// <summary>
        /// Gets the amount of valid data within the <see cref="ResourceTable"/>.
        /// </summary>
        public int Count => this._datas.Count;

        /// <summary>
        /// Creates a new <see cref="ResourceTable"/> instance.
        /// </summary>
        /// <param name="path">Resource path</param>
        public ResourceTable(string path)
            : this(path, 0, null, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTable"/> instance.
        /// </summary>
        /// <param name="path">Resource path</param>
        /// <param name="headerIndex">Header index in file</param>
        public ResourceTable(string path, int headerIndex)
            : this(path, headerIndex, null, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTable"/> instance.
        /// </summary>
        /// <param name="path">Resource path</param>
        /// <param name="headerIndex">Header index in file</param>
        /// <param name="defines">Defines used to transform</param>
        /// <param name="texts">Texts used to transform</param>
        public ResourceTable(string path, int headerIndex, IDictionary<string, int> defines, IDictionary<string, string> texts)
            : base(path, FileMode.Open, FileAccess.Read)
        {
            this._reader = new StreamReader(this);
            this._headerIndex = headerIndex;
            this._defines = defines;
            this._texts = texts;
            this._headers = this.ReadHeader();
            this._datas = new List<IEnumerable<string>>();
            
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
                T obj = (T)Activator.CreateInstance(typeof(T));

                foreach (var property in typeProperties)
                {
                    string dataMemberName = this.GetDataMemberName(property);
                    int index = this._headers.IndexOf(dataMemberName);

                    if (index != -1)
                        property.SetValue(obj, Convert.ChangeType(record.ElementAt(index), property.PropertyType));
                }

                records.Add(obj);
            }

            return records;
        }

        /// <summary>
        /// Reads and returns a list with the <see cref="ResourceTable"/> headers.
        /// </summary>
        /// <returns></returns>
        private IList<string> ReadHeader()
        {
            for (int i = 0; i < this._headerIndex; i++)
                this._reader.ReadLine();

            return this._reader.ReadLine()
                        .Replace("/", string.Empty)
                        .Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
        }

        /// <summary>
        /// Reads the content of the <see cref="ResourceTable"/>.
        /// </summary>
        private void ReadContent()
        {
            while (!this._reader.EndOfStream)
            {
                string line = this._reader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !line.StartsWith("//"))
                {
                    if (line.Contains("//"))
                        line.Remove(line.IndexOf("//"));

                    line = line.Replace(",,", ",=,").Replace(",", "\t");
                    string[] content = line.Split(new[] { '\t', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (content.Length == this._headers.Count)
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
                return this._texts[data].ToString();

            return data.Replace("=", "0").Replace(",", ".").Replace("\"", "");
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
        /// Gets the <see cref="DataMemberAttribute"/> name value from the given property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string GetDataMemberName(PropertyInfo property)
        {
            var dataMemberAttribute = property.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute;

            return dataMemberAttribute?.Name;
        }

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._headers.Clear();
                this._datas.Clear();
            }

            base.Dispose(disposing);
        }
    }
}
