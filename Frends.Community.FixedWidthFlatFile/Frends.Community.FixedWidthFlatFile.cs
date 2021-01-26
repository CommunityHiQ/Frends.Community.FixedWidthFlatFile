using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CSharp; // You can remove this if you don't need dynamic type in .NET Standard frends Tasks

#pragma warning disable 1591

namespace Frends.Community.FixedWidthFlatFile
{
    public class FixedWidthFlatFileTask
    {
        /// <summary>
        /// Parse Fixed Width data to object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="options"></param>
        /// <returns>Object { List&lt;Dictionary&lt;string Key, dynamic Value&gt;&gt; Data, JToken ToJson(), string ToXml()</returns>
        public static ParseResult Parse([PropertyTab] ParseInput input, [PropertyTab] ParseOptions options)
        {
            var inputRows = new List<string>();
            var headers = new List<string>();
            var outputData = new List<Dictionary<string, dynamic>>();

            //== Read input data
            using (var reader = new StringReader(input.FlatFileContent))
            {
                inputRows = reader.ReadLinesToList();
            }

            //== Parse header row
            switch (input.HeaderRow)
            {
                case HeaderRowType.Delimited:
                    char headerDelimiter = Convert.ToChar(input.HeaderDelimiter);
                    headers = inputRows.First().SplitToList(headerDelimiter);
                    inputRows.RemoveAt(0);
                    break;
                case HeaderRowType.FixedWidth:
                    headers = inputRows.First().SplitToList(input.ColumnSpecifications);
                    inputRows.RemoveAt(0);
                    break;
            }
            if (headers.Count > 0)
            {
                // add header values as name if not set in column specification
                var index = 0;
                foreach (var header in headers)
                {
                    if (string.IsNullOrEmpty(input.ColumnSpecifications[index].Name))
                        input.ColumnSpecifications[index].Name = header;
                    index++;
                }
            }

            //== Parse data rows

            // Skip rows?
            if (options.SkipRows)
            {
                if (options.SkipRowsFromTop > 0)
                {
                    // skipping more rows that exist?
                    if (options.SkipRowsFromTop >= inputRows.Count)
                        inputRows.Clear();
                    else
                        inputRows = inputRows.Skip(options.SkipRowsFromTop).ToList();
                }
                if (options.SkipRowsFromBottom > 0)
                {
                    // skipping more rows that exist?
                    if (options.SkipRowsFromBottom >= inputRows.Count)
                        inputRows.Clear();
                    else
                        inputRows = inputRows.Take((inputRows.Count - options.SkipRowsFromBottom)).ToList();
                }
            }
            // process data rows
            foreach (var dataRow in inputRows)
            {
                outputData.Add(Utils.ParseDataRow(dataRow, input.ColumnSpecifications));
            }


            return new ParseResult(outputData, options.CultureInfo);
        }
    }
}
