#pragma warning disable 1591

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Frends.Community.FixedWidthFlatFile
{
    public enum HeaderRowType { None, FixedWidth, Delimited }
    public enum ColumnType { String, Int, Long, Decimal, Double, Boolean, DateTime, Char }

    public class ParseInput
    {
        /// <summary>
        /// Fixed width flat file content
        /// </summary>
        [DisplayFormat(DataFormatString = "Expression")]
        public string FlatFileContent { get; set; }

        /// <summary>
        /// None: Flat file does not contain header row
        /// FixedWidth: Header row is parsed using column specification
        /// Delimited: Header row is parsed using delimiter char
        /// </summary>
        [DefaultValue(HeaderRowType.FixedWidth)]
        public HeaderRowType HeaderRow { get; set; }

        /// <summary>
        /// If header row uses delimiter set it here
        /// </summary>
        [UIHint(nameof(HeaderRow), "", HeaderRowType.Delimited)]
        [DisplayFormat(DataFormatString = "Text")]
        public string HeaderDelimiter { get; set; }

        public ColumnSpecification[] ColumnSpecifications { get; set; }
    }

    public class ColumnSpecification
    {
        /// <summary>
        /// Column name. If input data contains Header row and value is left empty, header value is used as name.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Name { get; set; }
        public ColumnType Type { get; set; }

        /// <summary>
        /// Exact format of DateTime value
        /// Example: yyyy-MM-ddTHH:mm:ss
        /// </summary>
        [UIHint(nameof(Type), "", ColumnType.DateTime)]
        [DisplayFormat(DataFormatString = "Text")]
        [DefaultValue("yyyy-MM-ddTHH:mm:ss")]
        public string DateTimeFormat { get; set; }
        public int Length { get; set; }
    }

    public class ParseOptions
    {
        /// <summary>
        /// Skip data rows?
        /// </summary>
        [DefaultValue(false)]
        public bool SkipRows { get; set; }

        /// <summary>
        /// Count of data rows to skip from top
        /// </summary>
        [UIHint(nameof(SkipRows), "", true)]
        [DefaultValue(0)]
        public int SkipRowsFromTop { get; set; }

        /// <summary>
        /// Count of data rows to skip from bottom
        /// </summary>
        [UIHint(nameof(SkipRows), "", true)]
        [DefaultValue(0)]
        public int SkipRowsFromBottom { get; set; }

        /// <summary>
        /// Specify the culture info to be used when parsing result to JTOKEN. If this is left empty InvariantCulture will be used. List of cultures: https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx Use the Language Culture Name.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string CultureInfo { get; set; }
    }

    public class ParseResult
    {
        public List<Dictionary<string, dynamic>> Data { get; set; }

        private readonly Lazy<JToken> _jToken;
        public JToken ToJson() { return _jToken.Value; }

        private readonly Lazy<string> _xml;
        public string ToXml() { return _xml.Value; }

        private CultureInfo _culture;

        public ParseResult(List<Dictionary<string, dynamic>> data, string cultureInfo = null)
        {
            Data = data;
            _culture = string.IsNullOrWhiteSpace(cultureInfo) ? CultureInfo.InvariantCulture : new CultureInfo(cultureInfo);

            _jToken = new Lazy<JToken>(() => WriteToJToken(Data, _culture));
            _xml = new Lazy<string>(() => WriteToXmlString(Data));
        }

        private static JToken WriteToJToken(List<Dictionary<string, dynamic>> data, CultureInfo culture)
        {
            using (var writer = new JTokenWriter())
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                writer.Culture = culture;

                writer.WriteStartArray(); // root start

                foreach (var row in data)
                {
                    writer.WriteStartObject(); // start row object
                    foreach (var key in row.Keys)
                    {
                        writer.WritePropertyName(key);
                        // null check
                        if (row[key] != null)
                            writer.WriteValue(row[key]);
                        else //write empty string value for null fields
                            writer.WriteValue("");
                    }

                    writer.WriteEndObject(); // end row

                }

                writer.WriteEndArray(); // root array end

                return writer.Token;
            }
        }

        private static string WriteToXmlString(List<Dictionary<string, dynamic>> data)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(ms, new UTF8Encoding(false)) { Formatting = Formatting.Indented })
                {
                    writer.WriteStartDocument(); // start doc
                    writer.WriteStartElement("Root");
                    writer.WriteStartElement("Rows");

                    foreach (var row in data)
                    {
                        writer.WriteStartElement("Row");

                        foreach (var key in row.Keys)
                        {
                            // value null check
                            if (row[key] != null)
                            {
                                if (row[key].GetType() == typeof(DateTime))
                                    writer.WriteElementString(key, row[key].ToString("s"));
                                else
                                    writer.WriteElementString(key, row[key].ToString());
                            }
                            else // write empty string for null values
                            {
                                writer.WriteElementString(key, "");
                            }
                        }

                        writer.WriteEndElement(); // end Row
                    }


                    writer.WriteEndElement(); // end Rows
                    writer.WriteEndElement(); // end Root
                    writer.WriteEndDocument(); // end doc
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
