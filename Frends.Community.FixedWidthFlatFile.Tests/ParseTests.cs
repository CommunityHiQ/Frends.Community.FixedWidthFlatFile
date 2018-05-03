using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Frends.Community.FixedWidthFlatFile.Tests
{
    [TestClass]
    public class ParseTests
    {
        ParseInput _parseInput;
        ParseOptions _options;

        [TestInitialize]
        public void Init()
        {
            //_parseInput = new ParseInput { }
        }

        [TestMethod]
        public void Parse_DataWithHeader_Test()
        {
            string fileContent =
                @"Name    Street    StartDate
Veijo   FrendsStr 20180527 
Hodor   HodorsStr 20180101 ";

            var columnSpecs = new ColumnSpecification[] {
            new ColumnSpecification{Length = 8, Type = ColumnType.String },
            new ColumnSpecification{Length = 10, Type = ColumnType.String },
            new ColumnSpecification {Length = 9, Type = ColumnType.DateTime, DateTimeFormat = "yyyyMMdd" } };

            _parseInput = new ParseInput { ColumnSpecifications = columnSpecs, FlatFileContent = fileContent, HeaderRow = HeaderRowType.FixedWidth };
            _options = new ParseOptions { SkipRows = false };

            var result = FixedWidthFlatFileTask.Parse(_parseInput, _options);

            Assert.AreEqual(2, result.Data.Count);

            var firstRow = result.Data.First();
            Assert.IsTrue(firstRow.ContainsKey("Name"));
            Assert.AreEqual("Veijo", firstRow["Name"]);
            Assert.IsTrue(firstRow.ContainsKey("Street"));
            Assert.AreEqual("FrendsStr", firstRow["Street"]);
            Assert.IsTrue(firstRow.ContainsKey("StartDate"));
        }

        [TestMethod]
        public void Parse_DataWithDelimitedHeader_Test()
        {
            string fileContent =
                @"Name;Street;StartDate
Veijo   FrendsStr 20180527 
Hodor   HodorsStr 20180101 ";

            var columnSpecs = new ColumnSpecification[] {
            new ColumnSpecification{Length = 8, Type = ColumnType.String },
            new ColumnSpecification{Length = 10, Type = ColumnType.String },
            new ColumnSpecification {Length = 9, Type = ColumnType.DateTime, DateTimeFormat = "yyyyMMdd" } };

            _parseInput = new ParseInput { ColumnSpecifications = columnSpecs, FlatFileContent = fileContent, HeaderRow = HeaderRowType.Delimited, HeaderDelimiter = ";" };
            _options = new ParseOptions { SkipRows = false };

            var result = FixedWidthFlatFileTask.Parse(_parseInput, _options);

            Assert.AreEqual(2, result.Data.Count);

            var firstRow = result.Data.First();
            Assert.IsTrue(firstRow.ContainsKey("Name"));
            Assert.AreEqual("Veijo", firstRow["Name"]);
            Assert.IsTrue(firstRow.ContainsKey("Street"));
            Assert.AreEqual("FrendsStr", firstRow["Street"]);
            Assert.IsTrue(firstRow.ContainsKey("StartDate"));
        }

        [TestMethod]
        public void Parse_WithoutHeader_Test()
        {
            string fileContent = @"Veijo   FrendsStr 20180527 
Hodor   HodorsStr 20180101 ";

            var columnSpecs = new ColumnSpecification[] {
            new ColumnSpecification{Length = 8, Type = ColumnType.String, Name = "Name" },
            new ColumnSpecification{Length = 10, Type = ColumnType.String, Name ="Street" },
            new ColumnSpecification {Length = 9, Type = ColumnType.DateTime, DateTimeFormat = "yyyyMMdd", Name = "StartDate" } };

            _parseInput = new ParseInput { ColumnSpecifications = columnSpecs, FlatFileContent = fileContent, HeaderRow = HeaderRowType.None};
            _options = new ParseOptions { SkipRows = false };

            var result = FixedWidthFlatFileTask.Parse(_parseInput, _options);

            Assert.AreEqual(2, result.Data.Count);

            var firstRow = result.Data.First();
            Assert.IsTrue(firstRow.ContainsKey("Name"));
            Assert.AreEqual("Veijo", firstRow["Name"]);
            Assert.IsTrue(firstRow.ContainsKey("Street"));
            Assert.AreEqual("FrendsStr", firstRow["Street"]);
            Assert.IsTrue(firstRow.ContainsKey("StartDate"));
        }

        [TestMethod]
        public void Parse_AddsGenericKeys_ForValuesWithoutName()
        {
            string fileContent = @"Veijo   FrendsStr 20180527 
Hodor   HodorsStr 20180101 ";

            var columnSpecs = new ColumnSpecification[] {
            new ColumnSpecification{Length = 8, Type = ColumnType.String, Name = "Name" },
            new ColumnSpecification{Length = 10, Type = ColumnType.String },
            new ColumnSpecification {Length = 9, Type = ColumnType.DateTime, DateTimeFormat = "yyyyMMdd"} };

            _parseInput = new ParseInput { ColumnSpecifications = columnSpecs, FlatFileContent = fileContent, HeaderRow = HeaderRowType.None };
            _options = new ParseOptions { SkipRows = false };

            var result = FixedWidthFlatFileTask.Parse(_parseInput, _options);

            Assert.AreEqual(2, result.Data.Count);

            var firstRow = result.Data.First();
            Assert.IsTrue(firstRow.ContainsKey("Name"));
            Assert.AreEqual("Veijo", firstRow["Name"]);
            Assert.IsTrue(firstRow.ContainsKey("Field_2"));
            Assert.AreEqual("FrendsStr", firstRow["Field_2"]);
            Assert.IsTrue(firstRow.ContainsKey("Field_3"));
        }
    }
}
