using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.Community.FixedWidthFlatFile.Tests
{
    [TestClass]
    public class ResultObjectTests
    {
        #region Test data creation helper methods

        private ParseResult CreateResultWithEmptyValues()
        {
            string fileContent =
                @"First;Second;Third
firstValue    ThirdValue";

            var columnSpec = new ColumnSpecification[] {
                new ColumnSpecification{Length= 10, Type = ColumnType.String},
                new ColumnSpecification{Length= 4, Type = ColumnType.String},
                new ColumnSpecification{Length= 10, Type = ColumnType.String}
            };

            var parseInput = new ParseInput
            {
                ColumnSpecifications = columnSpec,
                FlatFileContent = fileContent,
                HeaderDelimiter = ";",
                HeaderRow = HeaderRowType.Delimited
            };
            var options = new ParseOptions { SkipRows = false };

            var result = FixedWidthFlatFileTask.Parse(parseInput, options);

            return result;
        }
        private ParseResult CreateResultWithDateValue()
        {
            string fileContent =
                @"First;Second;Third
firstValue    ThirdValue190315";

            var columnSpec = new ColumnSpecification[] {
                new ColumnSpecification{Length= 10, Type = ColumnType.String},
                new ColumnSpecification{Length= 4, Type = ColumnType.String},
                new ColumnSpecification{Length= 10, Type = ColumnType.String},
                new ColumnSpecification{Length= 6, Type = ColumnType.DateTime, DateTimeFormat ="yyMMdd"}
            };

            var parseInput = new ParseInput
            {
                ColumnSpecifications = columnSpec,
                FlatFileContent = fileContent,
                HeaderDelimiter = ";",
                HeaderRow = HeaderRowType.Delimited
            };
            var options = new ParseOptions { SkipRows = false };

            var result = FixedWidthFlatFileTask.Parse(parseInput, options);

            return result;
        }

        #endregion

        [TestMethod]
        public void ToXml_WithEmptyValues_DoesNotThrowException()
        {
            var parseResult = CreateResultWithEmptyValues();

            var xmlResult = parseResult.ToXml();
        }

        [TestMethod]
        public void ToJson_WithEmptyValues_DoesNotThrowException()
        {
            var parseResult = CreateResultWithEmptyValues();

            var jsonResult = parseResult.ToJson();
        }

        [TestMethod]
        public void ToXml_WithDateTimeValue_DoesNotThrowException()
        {
            var parseResult = CreateResultWithDateValue();

            var xmlResult = parseResult.ToXml();
        }

        [TestMethod]
        public void ToJson_WithDateTimeValue_DoesNotThrowException()
        {
            var parseResult = CreateResultWithDateValue();

            var jsonResult = parseResult.ToJson();
        }

    }
}
