using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rhisis.Core.Resources
{
    public class ResourceTableNew : FileStream, IDisposable
    {
        private readonly IDictionary<string, int> _defines;
        private readonly IDictionary<string, string> _texts;
        private readonly StreamReader _reader;

        private IList<string> _headers;
        private IList<IEnumerable<string>> _datas;
        private int _headerIndex;

        public ResourceTableNew(string path)
            : this(path, 0, null, null)
        {
        }

        public ResourceTableNew(string path, int headerIndex)
            : this(path, headerIndex, null, null)
        {
        }

        public ResourceTableNew(string path, int headerIndex, IDictionary<string, int> defines, IDictionary<string, string> texts)
            : base(path, FileMode.Open, FileAccess.Read)
        {
            this._reader = new StreamReader(this);
            this._headerIndex = headerIndex;
            this._defines = defines;
            this._texts = texts;
            this._headers = new List<string>();
            this._datas = new List<IEnumerable<string>>();

            this.ReadHeader();
            this.ReadContent();
        }

        private void ReadHeader()
        {
            for (int i = 0; i < this._headerIndex; i++)
                this._reader.ReadLine();

            this._headers = this._reader.ReadLine()
                .Replace("/", string.Empty)
                .Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        private void ReadContent()
        {
            while (!this._reader.EndOfStream)
            {
                string line = this._reader.ReadLine();

                if (!string.IsNullOrEmpty(line) && !line.StartsWith("//"))
                {
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

        private string Transform(string data)
        {
            if (this._defines.ContainsKey(data))
                return this._defines[data].ToString();
            else if (this._texts.ContainsKey(data))
                return this._texts[data].ToString();

            return data.Replace("=", "0").Replace(",", ".").Replace("\"", "");
        }

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
                    {
                        property.SetValue(obj, Convert.ChangeType(record.ElementAt(index), property.PropertyType));
                    }
                }

                records.Add(obj);
            }

            return records;
        }

        private IEnumerable<PropertyInfo> GetPropertiesWithDataMemberAttribute<T>()
        {
            return from x in typeof(T).GetProperties()
                   where x.GetCustomAttribute(typeof(DataMemberAttribute)) != null
                   select x;
        }

        private string GetDataMemberName(PropertyInfo property)
        {
            var dataMemberAttribute = property.GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute;

            return dataMemberAttribute?.Name;
        }

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
