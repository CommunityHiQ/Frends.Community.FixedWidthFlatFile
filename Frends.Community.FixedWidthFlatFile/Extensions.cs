using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#pragma warning disable 1591

namespace Frends.Community.FixedWidthFlatFile
{
    public static class Extensions
    {
        /// <summary>
        /// Reads all lines to string List
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<string> ReadLinesToList(this StringReader reader)
        {
            var allLines = new List<string>();
            string line;
            while (null != (line = reader.ReadLine()))
            {
                //skip empty lines
                if (!string.IsNullOrWhiteSpace(line))
                    allLines.Add(line);
            }

            return allLines;
        }

        public static List<string> SplitToList(this string row, char delimiter)
        {
            var values = new List<string>();

            foreach (var value in row.Split(new[] { delimiter }, StringSplitOptions.None))
            {
                values.Add(value.Trim());
            }

            return values;
        }

        /// <summary>
        /// Splits data row according to given column specification
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnSpecifications"></param>
        /// <returns>List of values</returns>
        public static List<string> SplitToList(this string row, ColumnSpecification[] columnSpecifications)
        {
            try
            {
                var values = new List<string>();

                int startIndex = 0;
                foreach (var columnSpec in columnSpecifications)
                {
                    var value = row.Substring(startIndex, columnSpec.Length);
                    values.Add(value.Trim());
                    // move substring start index
                    startIndex += columnSpec.Length;
                }

                return values;
            }
            catch (Exception ex)
            {
                // throw custom exception for more descriptive information
                throw new InvalidDataException("Data row did not match column specifications.", ex);
            }
        }

        /// <summary>
        /// Adds key and value. If key already exists, it is renamed with '_1', '_2' etc suffix.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddKeyValuePair(this Dictionary<string, dynamic> dictionary, string key, dynamic value)
        {
            var originalKey = key;
            int renameIndex = 1;
            while (dictionary.ContainsKey(key))
            {
                key = $"{originalKey}_{renameIndex.ToString()}";
                renameIndex++;
            }
            dictionary.Add(key, value);
        }

    }
}
