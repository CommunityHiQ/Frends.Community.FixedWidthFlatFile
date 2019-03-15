using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
                @"First;Second;Third;Date
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

            Assert.IsTrue(parseResult.Data[0]["Second"] == null);

            var xmlResult = parseResult.ToXml();
            //check that empty XML element is created
            Assert.IsTrue(xmlResult.Contains("<Second />"));
        }

        [TestMethod]
        public void ToJson_WithEmptyValues_DoesNotThrowException()
        {
            var parseResult = CreateResultWithEmptyValues();

            Assert.IsTrue(parseResult.Data[0]["Second"] == null);

            var jsonResult = parseResult.ToJson();
            // check that empty value is set
            Assert.AreEqual(jsonResult[0]["Second"], string.Empty);
        }

        // Test that null check does not fail with DateTime Type values
        [TestMethod]
        public void ToXml_NullCheckWithDateTimeValue_DoesNotThrowException()
        {
            var parseResult = CreateResultWithDateValue();
            Assert.IsTrue(parseResult.Data[0]["Date"].GetType() == typeof(DateTime));
            Assert.IsTrue(parseResult.Data[0]["Second"] == null);

            var xmlResult = parseResult.ToXml();

            //check that empty XML element is created
            Assert.IsTrue(xmlResult.Contains("<Second />"));
        }

        // Test that null check does not fail with DateTime Type values
        [TestMethod]
        public void ToJson_NullCheckWithDateTimeValue_DoesNotThrowException()
        {
            var parseResult = CreateResultWithDateValue();

            Assert.IsTrue(parseResult.Data[0]["Date"].GetType() == typeof(DateTime));
            Assert.IsTrue(parseResult.Data[0]["Second"] == null);

            var jsonResult = parseResult.ToJson();
            // check that empty value is set
            Assert.AreEqual(jsonResult[0]["Second"], string.Empty);
        }

    }
}
