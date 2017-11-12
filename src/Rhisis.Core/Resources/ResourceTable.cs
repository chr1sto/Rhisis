using Rhisis.Core.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// This class is used to parse and exploit the FlyFF resources like propItem.txt, propSkills.txt, etc...
    /// </summary>
    public class ResourceTable : FileStream
    {
        private readonly List<string> _headers;
        private readonly ICollection<ResourceTableData> _tableData;
        private IDictionary<string, int> _defines;
        private IDictionary<string, string> _texts;

        /// <summary>
        /// Gets the reading index.
        /// </summary>
        public int ReadingIndex { get; private set; }

        /// <summary>
        /// Creates a new ResourceTable instance using a file path.
        /// </summary>
        /// <param name="filePath"></param>
        public ResourceTable(string filePath)
            : base(filePath, FileMode.Open, FileAccess.ReadWrite)
        {
            this.ReadingIndex = -1;
            this._headers = new List<string>();
            this._tableData = new List<ResourceTableData>();
        }

        /// <summary>
        /// Set the table headers.
        /// </summary>
        /// <param name="headers"></param>
        public void SetTableHeaders(params string[] headers) => this._headers.AddRange(headers);

        /// <summary>
        /// Add a dictionary of defines to this resource table.
        /// </summary>
        /// <param name="definesToAdd"></param>
        public void AddDefines(IDictionary<string, int> definesToAdd) => this._defines = definesToAdd;

        /// <summary>
        /// Add a dictionary of texts to this resource table.
        /// </summary>
        /// <param name="textsToAdd"></param>
        public void AddTexts(IDictionary<string, string> textsToAdd) => this._texts = textsToAdd;

        /// <summary>
        /// Read the resource table.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            ++this.ReadingIndex;

            if (this.ReadingIndex > this._tableData.Count - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Parse the resource table raw data.
        /// </summary>
        public void Parse()
        {
            var reader = new StreamReader(this);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Trim();

                // Remove comments from line
                if (line.StartsWith("//"))
                    continue;
                if (line.StartsWith("/*"))
                {
                    while (line.Contains("*/") == false)
                        line = reader.ReadLine();
                    continue;
                }
                if (line.Contains("//"))
                    line = line.Remove(line.IndexOf("/"));

                line = line.Replace(",,", ",=,").Replace(",", "\t");
                string[] lineData = line.Split(new[] { '\t', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (lineData.Length == this._headers.Count)
                {
                    var data = new ResourceTableData();

                    for (int i = 0; i < lineData.Length; ++i)
                    {
                        string dataValue = lineData[i].Trim();

                        if (this._defines.ContainsKey(dataValue))
                            dataValue = this._defines[dataValue].ToString();
                        else if (this._texts.ContainsKey(dataValue))
                            dataValue = this._texts[dataValue];

                        dataValue = dataValue.Replace("=", "0").Replace(",", ".").Replace("\"", "");
                        data[this._headers[i]] = dataValue;
                    }

                    this._tableData.Add(data);
                }
            }
        }

        /// <summary>
        /// Gets the number of data inside this resource table.
        /// </summary>
        public int Count() => this._tableData.Count;

        /// <summary>
        /// Gets the value of the current line using a header key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = this._tableData.ElementAt(this.ReadingIndex)?[key];

            try
            {
                if (string.IsNullOrEmpty(value))
                    return default(T);

                if (value.StartsWith("0x"))
                {
                    value = value.Remove(0, 2);
                    value = uint.Parse(value, NumberStyles.HexNumber).ToString();
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception e)
            {
                Logger.Error("Unable to get the value of key: {0}.", key);
                Logger.Debug("StackTrace: {0}", e.StackTrace);
            }

            return default(T);
        }

        /// <summary>
        /// Dispose the <see cref="ResourceTable"/> resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._tableData.Clear();
                this._headers.Clear();
            }

            base.Dispose(disposing);
        }
    }

    public class ResourceTableData
    {
        private readonly Dictionary<string, string> _tableData;

        /// <summary>
        /// Gets or sets the resource table data.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get { return this._tableData.ContainsKey(key) ? this._tableData[key] : string.Empty; }
            set
            {
                if (this._tableData.ContainsKey(key))
                    this._tableData[key] = value;
                else
                    this._tableData.Add(key, value);
            }
        }

        /// <summary>
        /// Creates a new <see cref="ResourceTableData"/> instance.
        /// </summary>
        public ResourceTableData()
        {
            this._tableData = new Dictionary<string, string>();
        }
    }
}
