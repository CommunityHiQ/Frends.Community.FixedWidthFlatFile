using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#pragma warning disable 1591

namespace Frends.Community.FixedWidthFlatFile
{
    public static class Utils
    {
        public static Dictionary<string, dynamic> ParseDataRow(string row, ColumnSpecification[] columnSpecification)
        {
            var parsedData = new Dictionary<string, dynamic>();

            var rowValues = row.SplitToList(columnSpecification);

            for (var i = 0; i < rowValues.Count; i++)
            {
                var columnSpec = columnSpecification[i];
                var columnValue = rowValues[i];
                var columnName = columnSpec.Name;
                if (string.IsNullOrEmpty(columnName))
                    columnName = $"Field_{i + 1}";

                if (string.IsNullOrWhiteSpace(columnValue))
                    parsedData.AddKeyValuePair(columnSpec.Name, null);
                else
                {
                    switch (columnSpec.Type)
                    {
                        case ColumnType.Boolean:
                            parsedData.AddKeyValuePair(columnName, bool.Parse(columnValue));
                            break;
                        case ColumnType.Char:
                            parsedData.AddKeyValuePair(columnName, char.Parse(columnValue));
                            break;
                        case ColumnType.DateTime:
                            parsedData.AddKeyValuePair(columnName, string.IsNullOrEmpty(columnSpec.DateTimeFormat) ?
                                DateTime.Parse(columnValue) :
                                DateTime.ParseExact(columnValue, columnSpec.DateTimeFormat, CultureInfo.InvariantCulture));
                            break;
                        case ColumnType.Decimal:
                            parsedData.AddKeyValuePair(columnName, decimal.Parse(columnValue, columnValue.Contains(",") ? CultureInfo.GetCultureInfo("fi-FI") : CultureInfo.InvariantCulture));
                            break;
                        case ColumnType.Double:
                            parsedData.AddKeyValuePair(columnName, double.Parse(columnValue, columnValue.Contains(",") ? CultureInfo.GetCultureInfo("fi-FI") : CultureInfo.InvariantCulture));
                            break;
                        case ColumnType.Int:
                            parsedData.AddKeyValuePair(columnName, int.Parse(columnValue));
                            break;
                        case ColumnType.Long:
                            parsedData.AddKeyValuePair(columnName, long.Parse(columnValue));
                            break;
                        default:
                            parsedData.AddKeyValuePair(columnName, columnValue);
                            break;
                    }
                }
            }
            return parsedData;
        }


    }
}
